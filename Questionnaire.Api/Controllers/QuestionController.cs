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
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var questionDetailsDto = await _questionService.GetQuestion(id);

            return Ok(questionDetailsDto);
        }

        [HttpGet("list/{id}")]
        public async Task <ActionResult<IEnumerable<QuestionnaireQuestionDto>>> GetQuestionnaireQuestions(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var questions = await _questionService.GetQuestionnaireQuestions(id);

            return Ok(questions);
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

        /*[HttpGet("answer/{id}")]
        public async Task<ActionResult<QuestionnaireQuestionDto>> GetQuestionnaireQuestion(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //check user questionnaire exists, there is any remaining time

            var question = await _questionService.GetQuestionnaireQuestion(id);

            return Ok(question);
        }*/

        [HttpPost("answer")]
        public async Task<IActionResult> PostQuestionAnswer([FromBody]UserQuestionnaireAnswerDto answerDto)
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

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Post([FromBody] QuestionDto questionDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var question = await _questionService.CreateQuestion(questionDto);

            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuestionDto questionDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var question = await _questionService.UpdateQuestion(questionDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var question = await _questionService.DeleteQuestion(id);

            return Ok();
        }
    }
}
