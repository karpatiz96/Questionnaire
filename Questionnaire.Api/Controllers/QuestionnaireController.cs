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
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IUserQuestionnaireService _userQuestionnaireService;
        private readonly UserManager<User> userManager;

        public QuestionnaireController(IQuestionnaireService questionnaireService, IUserQuestionnaireService userQuestionnaireService, UserManager<User> UserManager)
        {
            _questionnaireService = questionnaireService;
            _userQuestionnaireService = userQuestionnaireService;
            userManager = UserManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionnaireHeaderDto>>> GetQuestionnaires(int groupId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var questionnairesDto = await _questionnaireService.GetQuestionnaires(groupId, userId);

            if (questionnairesDto == null)
            {
                return NotFound();
            }

            return Ok(questionnairesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionnaireDetailsDto>> GetQuestionnaire(int id)
        {
            var questionnaireDto = await _questionnaireService.GetQuestionnaire(id);

            if (questionnaireDto == null)
            {
                return NotFound();
            }

            return Ok(questionnaireDto);
        }

        [HttpGet("start/{id}")]
        public async Task<ActionResult<QuestionnaireStartDto>> GetQuestionnaireStart(int id)
        {
            var questionnaireStartDto = await _questionnaireService.GetQuestionnaireStart(id);

            if(questionnaireStartDto == null)
            {
                return NotFound();
            }

            return Ok(questionnaireStartDto);
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

        [HttpPost]
        public async Task<ActionResult<QuestionnaireDto>> Post([FromBody] QuestionnaireDto questionnaireDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //check user is admin

            var questionnaire = await _questionnaireService.CreateQuestionnaire(questionnaireDto);

            return Ok(questionnaire);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuestionnaireDto questionnaireDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //check user is admin and questionnaire is not started

            var questionnaire = await _questionnaireService.UpdateQuestionnaire(questionnaireDto);

            return NoContent();
        }
    }
}
