using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Bll.Dtos;
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
        private readonly UserManager<User> userManager;

        public QuestionnaireController(IQuestionnaireService questionnaireService, UserManager<User> UserManager)
        {
            _questionnaireService = questionnaireService;
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuestionnaireDto questionnaireDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //check user is admin

            var questionnaire = await _questionnaireService.UpdateQuestionnaire(questionnaireDto);

            return NoContent();
        }
    }
}
