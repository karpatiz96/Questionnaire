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
    public class AnswerController : ControllerBase
    {
        private IAnswerService _answerService;
        private readonly UserManager<User> userManager;

        public AnswerController(IAnswerService answerService, UserManager<User> UserManager)
        {
            _answerService = answerService;
            userManager = UserManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDetailsDto>> GetAnswer(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var answerDetialsDto = await _answerService.GetAnswer(id);

            return Ok(answerDetialsDto);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerDto>> Post([FromBody] AnswerDto answerDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var answer = await _answerService.CreateAnswer(answerDto);

            return Ok(answer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AnswerDto answerDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var answer = await _answerService.UpdateAnswer(answerDto);

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

            var answer = await _answerService.DeleteAnswer(id);

            return Ok();
        }
    }
}
