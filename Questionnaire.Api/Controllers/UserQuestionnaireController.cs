using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.ResultDtos;
using Questionnaire.Bll.IServices;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Questionnaire.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserQuestionnaireController: ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IUserQuestionnaireService _userQuestionnaireService;

        public UserQuestionnaireController(IUserQuestionnaireService userQuestionnaireService, UserManager<User> UserManager)
        {
            _userQuestionnaireService = userQuestionnaireService;
            userManager = UserManager;
        }

        [HttpGet("answer/{id}")]
        public async Task<ActionResult<UserQuestionnaireAnswerDetailsDto>> GetUserQuestionnaireAnswer(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userQuestionnaireAnswerDetailsDto = await _userQuestionnaireService.GetUserQuestionnaireAnswerDetails(id);

            return Ok(userQuestionnaireAnswerDetailsDto);
        }

        [HttpGet("result/admin/{id}")]
        public async Task<ActionResult<QuestionnaireResultListDto>> GetResultList(int id)
        {
            //User is admin of group
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var questionnaireResultListDto = await _userQuestionnaireService.GetQuestionnaireResultList(id);

            return Ok(questionnaireResultListDto);
        }

        [HttpGet("result/{id}")]
        public async Task<ActionResult<QuestionnaireResultDto>> GetResult(int id)
        {
            //User is admin or solver
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //var questionnaireResultDto
            var questionnaireResultDto = await _userQuestionnaireService.GetQuestionnaireResult(id);

            return Ok(questionnaireResultDto);
        }

        [HttpPost("start")]
        public async Task<IActionResult> PostStart([FromBody] int questionnaireId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //check user is part of questionnaire group

            //check userQuestionnaire exists

            var userQuestionnaireExists = await _userQuestionnaireService.UserQuestionnaireExists(userId, questionnaireId);
            if (userQuestionnaireExists)
            {
                BadRequest("Questionnaire is already Solved!");
            }

            await _userQuestionnaireService.CreateUserQuestionnaire(userId, questionnaireId);

            return Ok();
        }

        [HttpPost("answer")]
        public async Task<IActionResult> PostQuestionAnswer([FromBody] UserQuestionnaireAnswerDto answerDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //update answer, check time, visibility, usergroup

            await _userQuestionnaireService.AnswerQuestion(answerDto, userId);

            return Ok();
        }

        [HttpPost("answer/evaluate")]
        public async Task<IActionResult> PostUserQuestionnaireAnswerEvaluation([FromBody] UserQuestionnaireAnswerEvaluationDto evaluationDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            await _userQuestionnaireService.EvaluateUserQuestionnaireAnswer(evaluationDto);

            return Ok();
        }
    }
}
