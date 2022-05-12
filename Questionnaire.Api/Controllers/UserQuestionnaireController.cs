using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.ResultDtos;
using Questionnaire.Bll.Exceptions;
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userQuestionnaireAnswerDetailsDto = await _userQuestionnaireService.GetUserQuestionnaireAnswerDetails(userId, id);

            return Ok(userQuestionnaireAnswerDetailsDto);
        }

        [HttpGet("result/admin/{id}")]
        public async Task<ActionResult<QuestionnaireResultListDto>> GetResultList(int id)
        {
            //User is admin of group
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userGroup = await _userQuestionnaireService.GetUserGroupByUserAndQuestionnaire(userId, id);
            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaireResultListDto = await _userQuestionnaireService.GetQuestionnaireResultList(id);

            return Ok(questionnaireResultListDto);
        }

        [HttpGet("result/{id}")]
        public async Task<ActionResult<QuestionnaireResultDto>> GetResult(int id)
        {
            //User is admin or solver
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var questionnaireResultDto
            var questionnaireResultDto = await _userQuestionnaireService.GetQuestionnaireResult(userId, id);

            return Ok(questionnaireResultDto);
        }

        [HttpPost("start")]
        public async Task<IActionResult> PostStart([FromBody] int questionnaireId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //check user is member of group
            var userGroup = await _userQuestionnaireService.GetUserGroupByUserAndQuestionnaire(userId, questionnaireId);
            if(userGroup == null)
            {
                throw new UserNotMemberException("User is not memeber of group!");
            }

            //check userQuestionnaire exists
            var userQuestionnaireExists = await _userQuestionnaireService.UserQuestionnaireExists(userId, questionnaireId);
            if (userQuestionnaireExists)
            {
                return BadRequest(new { message = "Questionnaire is already Started!" });
            }

            await _userQuestionnaireService.CreateUserQuestionnaire(userId, questionnaireId);

            return Ok();
        }

        [HttpPost("answer")]
        public async Task<IActionResult> PostQuestionAnswer([FromBody] UserQuestionnaireAnswerDto answerDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //check user is member of group
            var userGroup = await _userQuestionnaireService.GetUserGroupByUserAndQuestionnaire(userId, answerDto.Id);
            if(userGroup == null)
            {
                throw new UserNotMemberException("User is not memeber of group!");
            }

            await _userQuestionnaireService.AnswerQuestion(answerDto, userId);

            return Ok();
        }

        [HttpPost("answer/evaluate")]
        public async Task<IActionResult> PostUserQuestionnaireAnswerEvaluation([FromBody] UserQuestionnaireAnswerEvaluationDto evaluationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _userQuestionnaireService.EvaluateUserQuestionnaireAnswer(userId, evaluationDto);

            return Ok();
        }
    }
}
