using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Exceptions;
using Questionnaire.Bll.IServices;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Services
{
    public class GroupService : IGroupService
    {
        private QuestionnaireDbContext _dbContext;

        public static Expression<Func<Group, GroupDetailsDto>> GroupDetailsSelector { get; } = g => new GroupDetailsDto 
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            Created = g.Created,
            GroupRole = "User",
            LastPost = DateTime.UtcNow, //To be updated
            Members = g.UserGroups.Count,
            Questionnaires = new List<QuestionnaireHeaderDto>()
        };

        public static Expression<Func<Group, GroupHeaderDto>> GroupHeaderSelector { get; } = g => new GroupHeaderDto
        {
            Id = g.Id,
            Name = g.Name,
            Created = g.Created,
            Members = g.UserGroups.Count,
            LastPost = DateTime.UtcNow
        };

        public static Expression<Func<Group, GroupListDto>> GroupListSelector { get; } = g => new GroupListDto
        {
            Id = g.Id,
            Name = g.Name
        };

        public static Expression<Func<UserGroup, UserGroupDto>> UserGroupSelector { get; } = g => new UserGroupDto
        {
            Id = g.Id,
            UserId = g.UserId,
            Name = g.User.UserName,
            Role = g.Role,
            Main = g.MainAdmin
        };

        public static Expression<Func<Invitation, InvitationGroupDto>> InvitationGroupSelector { get; } = g => new InvitationGroupDto
        {
            Id = g.Id,
            UserId = g.UserId,
            UserName = g.User.UserName,
            Date = g.Created,
            Status = g.Status
        };

        public static Expression<Func<QuestionnaireSheet, QuestionnaireHeaderDto>> QuestionnaireHeaderSelector { get; } = q => 
        new QuestionnaireHeaderDto
        {
            Id = q.Id,
            UserQuestionnaireId = -1,
            Title = q.Name,
            Begining = q.Begining,
            Finish = q.Finish,
            Start = null,
            CompletedTime = null,
            Completed = false,
            Evaluated = false,
            Created = q.Created,
            VisibleToGroup = q.VisibleToGroup
        };

        public GroupService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GroupDto> CreateGroup(GroupDto groupDto, string userId)
        {
            var group = new Group
            {
                Name = groupDto.Name,
                Description = groupDto.Description,
                Created = DateTime.UtcNow
            };

            _dbContext.Groups.Add(group);

            await _dbContext.SaveChangesAsync();

            var userGroup = new UserGroup
            {
                GroupId = group.Id,
                UserId = userId,
                MainAdmin = true,
                Role = "Admin",
                IsDeleted = false
            };

            _dbContext.UserGroups.Add(userGroup);

            await _dbContext.SaveChangesAsync();

            return groupDto;
        }

        public async Task<GroupDto> GetGroupById(string userId, int groupId)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(g => g.UserId == userId && g.GroupId == groupId)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var groupDto = await _dbContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => new GroupDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description
                })
                .FirstOrDefaultAsync();

            return groupDto;
        }

        public async Task<GroupDetailsDto> GetGroup(string userId, int groupId)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(g => g.UserId == userId && g.GroupId == groupId)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if(userGroup == null)
            {
                throw new UserNotMemberException("User is not member of the group.");
            }

            var groupDetailsDto = await _dbContext.Groups
                .Include(g => g.UserGroups)
                    .ThenInclude(g => g.User)
                .Include(g => g.QuestionnaireSheets)
                .Where(g => g.Id == groupId)
                .Select(GroupDetailsSelector)
                .FirstOrDefaultAsync();

            groupDetailsDto.GroupRole = userGroup.Role;

            var questionnaires = _dbContext.QuestionnaireSheets.Where(q => q.GroupId == groupId);
            if (userGroup.Role != "Admin")
                questionnaires = questionnaires.Where(q => q.VisibleToGroup);

            groupDetailsDto.Questionnaires = await questionnaires
                .OrderByDescending(q => q.Begining)
                .Select(QuestionnaireHeaderSelector)
                .ToListAsync();

            foreach (var questionnaire in groupDetailsDto.Questionnaires)
            {
                var userQuestionnaire = await _dbContext.UserQuestionnaires
                    .Include(u => u.UserQuestionnaireAnswers)
                    .Where(u => u.UserId == userId && u.QuestionnaireSheetId == questionnaire.Id)
                    .FirstOrDefaultAsync();

                questionnaire.UserQuestionnaireId = userQuestionnaire?.Id ?? -1;
                questionnaire.Start = userQuestionnaire?.Started;
                questionnaire.CompletedTime = userQuestionnaire?.Finished;
                questionnaire.Completed = userQuestionnaire != null ? userQuestionnaire.Completed : false;
                questionnaire.Evaluated = userQuestionnaire != null ? userQuestionnaire.UserQuestionnaireAnswers.Any(u => !u.AnswerEvaluated) : false;
            }

            return groupDetailsDto;
        }

        public async Task<GroupMemberDto> GetGroupMembers(string userId, int groupId)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(g => g.UserId == userId && g.GroupId == groupId)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var group = await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.Id == groupId);

            var members = await _dbContext.UserGroups
                .Include(ug => ug.User)
                .Where(ug => ug.GroupId == groupId)
                .Select(UserGroupSelector)
                .ToListAsync();

            var invited = await _dbContext.Invitations
                .Include(i => i.User)
                .Where(i => i.GroupId == groupId)
                .Select(InvitationGroupSelector)
                .ToListAsync();

            var groupMemberDto = new GroupMemberDto
            {
                Id = group.Id,
                Name = group.Name,
                Users = members,
                Invitations = invited
            };

            return groupMemberDto;
        }

        public async Task<IEnumerable<GroupHeaderDto>> GetGroups(string userId)
        {
            var groups = await _dbContext.Groups
                .Include(g => g.UserGroups)
                .Where(g => g.UserGroups.Any(ug => ug.UserId == userId && ug.IsDeleted == false))
                .Select(GroupHeaderSelector)
                .ToListAsync();

            return groups;
        }

        public async Task<IEnumerable<GroupHeaderDto>> GetMyGroups(string userId)
        {
            var groups = await _dbContext.Groups
               .Include(g => g.UserGroups)
               .Where(g => g.UserGroups.Any(ug => ug.UserId == userId && ug.MainAdmin && ug.IsDeleted == false))
               .Select(GroupHeaderSelector)
               .ToListAsync();

            return groups;
        }

        public async Task<IEnumerable<GroupListDto>> GetGroupsList(string userId)
        {
            var groups = await _dbContext.Groups
               .Include(g => g.UserGroups)
               .Where(g => g.UserGroups.Any(ug => ug.UserId == userId && ug.Role == "Admin" && ug.IsDeleted == false))
               .Select(GroupListSelector)
               .ToListAsync();

            return groups;
        }

        public async Task UpdateGroup(string userId, GroupDto groupDto)
        {
            var group = await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.Id == groupDto.Id);

            var userGroup = await _dbContext.UserGroups
                .Where(g => g.UserId == userId && g.GroupId == groupDto.Id)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            group.Name = groupDto.Name;
            group.Description = groupDto.Description;

            _dbContext.Attach(group).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
