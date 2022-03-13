using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Bll.IServices;
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
    public class UserGroupController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;

        public UserGroupController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        [HttpGet("admin/{id}")]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userGroup = await _userGroupService.UpdateUserGroupRole(userId, id, "Admin");

            return Ok();
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> MakeUser(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userGroup = await _userGroupService.UpdateUserGroupRole(userId, id, "User");

            return Ok();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {


            return Ok();
        }
    }
}
