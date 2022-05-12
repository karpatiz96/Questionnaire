using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Exceptions;
using Questionnaire.Bll.IServices;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Services
{
    public class UserGroupService: IUserGroupService
    {
        private readonly QuestionnaireDbContext _dbContext;

        public UserGroupService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<UserGroup> GetUserGroup(string userId, int groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserGroup> UpdateUserGroupRole(string userId, int userGroupId, string role)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(u => u.Id == userGroupId)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if(userGroup == null)
            {
                throw new NotFoundException("User not found!");
            }

            var userGroupAdmin = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == userGroup.GroupId)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if(userGroupAdmin == null || !userGroupAdmin.MainAdmin)
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            userGroup.Role = role;

            await _dbContext.SaveChangesAsync();

            return userGroup;
        }

        public async Task<UserGroup> DeleteUserGroup(string userId, int userGroupId)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(u => u.Id == userGroupId)
                .FirstOrDefaultAsync();

            if(userGroup == null)
            {
                throw new NotFoundException("User is not member of group!");
            }

            var userGroupAdmin = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == userGroup.GroupId)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if(userGroupAdmin == null || userGroupAdmin.Role != "Admin" || !userGroup.MainAdmin)
            {
                throw new UserNotAdminException("User is not main admin in group!");
            }

            userGroup.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return userGroup;
        }
    }
}
