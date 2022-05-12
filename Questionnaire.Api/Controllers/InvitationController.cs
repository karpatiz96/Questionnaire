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
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly UserManager<User> UserManager;

        public InvitationController(IInvitationService invitationService, UserManager<User> userManager)
        {
            _invitationService = invitationService;
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvitationDto>>> GetInvitations()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var invitations = await _invitationService.GetInvitations(userId);

            return Ok(invitations);
        }

        [HttpPost]
        public async Task<ActionResult<InvitationNewDto>> Post([FromBody] InvitationNewDto invitation)
        {
            var user = await UserManager.FindByEmailAsync(invitation.Email);

            if (user == null)
            {
                return BadRequest(new { message = "User does not exist with such email!" });
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var invitationNewDto = await _invitationService.CreateInvitation(userId, invitation, user);

            return Ok(invitationNewDto);
        }

        [HttpGet("accept/{id}")]
        public async Task<ActionResult<InvitationDto>> Accept(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var invitationDto = await _invitationService.AcceptInvitation(userId, id, Invitation.InvitationStatus.Accepted);

            return Ok(invitationDto);
        }

        [HttpGet("decline/{id}")]
        public async Task<ActionResult<InvitationDto>> Decline(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var invitationDto = await _invitationService.DeclineInvitation(userId, id, Invitation.InvitationStatus.Declined);

            return Ok(invitationDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var invitation = await _invitationService.DeleteInvitation(userId, id);

            return Ok();
        }

    }
}
