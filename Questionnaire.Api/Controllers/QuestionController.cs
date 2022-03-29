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
    public class QuestionController : ControllerBase
    {
        private IQuestionService _questionService;
        private IUserQuestionnaireService _userQuestionnaireService;
        private readonly UserManager<User> userManager;

        public QuestionController(IQuestionService questionService, IUserQuestionnaireService userQuestionnaireService, UserManager<User> UserManager)
        {
            _questionService = questionService;
            _userQuestionnaireService = userQuestionnaireService;
            userManager = UserManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDetailsDto>> GetQuestion(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userGroup = await _questionService.GetUserGroupByQuestionAndUser(userId, id);

            if(userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionDetailsDto = await _questionService.GetQuestion(id);

            return Ok(questionDetailsDto);
        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var questionDto = await _questionService.GetQuestionById(userId, id);

            return Ok(questionDto);
        }

        [HttpGet("list/{id}")]
        public async Task <ActionResult<QuestionnaireQuestionListDto>> GetQuestionnaireQuestionList(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var questions = await _questionService.GetQuestionnaireQuestionList(userId, id);

            return Ok(questions);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Post([FromBody] QuestionDto questionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var question = await _questionService.CreateQuestion(userId, questionDto);

            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuestionDto questionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var question = await _questionService.UpdateQuestion(userId, questionDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var question = await _questionService.DeleteQuestion(userId, id);

            return Ok();
        }
    }
}
