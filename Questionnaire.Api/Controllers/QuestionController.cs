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
    public class QuestionController : ControllerBase
    {
        private IQuestionService _questionService;
        private readonly UserManager<User> userManager;

        public QuestionController(IQuestionService questionService, UserManager<User> UserManager)
        {
            _questionService = questionService;
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
