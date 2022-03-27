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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<User> UserManager;

        public GroupController(IGroupService groupService, UserManager<User> userManager)
        {
            _groupService = groupService;
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupHeaderDto>>> GetGroups()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await UserManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound();
            }

            var groups = await _groupService.GetGroups(user.Id);

            return Ok(groups);
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<GroupHeaderDto>>> GetMyGroups()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var groups = await _groupService.GetMyGroups(user.Id);

            return Ok(groups);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<GroupListDto>>> GetGroupsList()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var groups = await _groupService.GetMyGroups(userId);

            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDetailsDto>> GetGroup(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var group = await _groupService.GetGroup(userId, id);

            if(group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        [HttpGet("member/{id}")]
        public async Task<ActionResult<GroupMemberDto>> GetGroupMember(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var groupMemberDto = await _groupService.GetGroupMembers(userId, id);

            if (groupMemberDto == null)
            {
                return NotFound();
            }

            return Ok(groupMemberDto);
        }

        [HttpPost]
        public async Task<ActionResult<GroupDto>> Post([FromBody] GroupDto groupDto)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var group = await _groupService.CreateGroup(groupDto, user.Id);

            return Ok(group);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GroupDto groupDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _groupService.UpdateGroup(userId, groupDto);

            return NoContent();
        }
    }
}
