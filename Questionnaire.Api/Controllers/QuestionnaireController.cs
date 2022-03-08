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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userGroup = await _questionnaireService.GetUserGroupByQuestionnaireAndUser(userId, id);

            if(userGroup == null)
            {

            }

            if(userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaireDto = await _questionnaireService.GetQuestionnaire(id);

            if (questionnaireDto == null)
            {
                return NotFound("Questionnaire doesn't exists!");
            }

            return Ok(questionnaireDto);
        }

        [HttpGet("start/{id}")]
        public async Task<ActionResult<QuestionnaireStartDto>> GetQuestionnaireStart(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userGroup = await _questionnaireService.GetUserGroupByQuestionnaireAndUser(userId, id);

            if (userGroup == null)
            {
                throw new UserGroupNotFoundExcetpion("User is not member of group!");
            }

            var questionnaireStartDto = await _questionnaireService.GetQuestionnaireStart(id);

            if(questionnaireStartDto == null)
            {
                return NotFound();
            }

            //Todo error if questionnaire is visible

            return Ok(questionnaireStartDto);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionnaireDto>> Post([FromBody] QuestionnaireDto questionnaireDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var questionnaire = await _questionnaireService.CreateQuestionnaire(userId, questionnaireDto);

            return Ok(questionnaire);
        }

        [HttpPost("hide")]
        public async Task<IActionResult> PostHide([FromBody] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _questionnaireService.HideQuestionnaire(userId, id);

            return Ok();
        }

        [HttpPost("show")]
        public async Task<IActionResult> PostShow([FromBody] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _questionnaireService.ShowQuestionnaire(userId, id);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuestionnaireDto questionnaireDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //check user is admin and questionnaire is not started

            var questionnaire = await _questionnaireService.UpdateQuestionnaire(userId, questionnaireDto);

            return NoContent();
        }
    }
}
