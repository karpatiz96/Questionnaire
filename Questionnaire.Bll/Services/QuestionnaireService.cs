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
    public class QuestionnaireService : IQuestionnaireService
    {
        private QuestionnaireDbContext _dbContext;

        public QuestionnaireService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<QuestionnaireSheet, QuestionnaireDetailsDto>> QuestionnaireDetailsSelector { get; } = q => 
        new QuestionnaireDetailsDto
        {
            Id = q.Id,
            Title = q.Name,
            Description = q.Description,
            Begining = q.Begining,
            Finish = q.Finish,
            VisibleToGroup = q.VisibleToGroup,
            Created = q.Created,
            LastEdited = q.LastEdited,
            Questions = q.Questions.Select(q => new QuestionHeaderDto
            {
                Id = q.Id,
                Name = q.Name,
                Number = q.Number,
                Type = q.Type,
                Value = q.MaximumPoints
            }).ToList()
        };

        public static Expression<Func<QuestionnaireSheet, QuestionnaireStartDto>> QuestionnaireStartSelector { get; } = q => new QuestionnaireStartDto
        {
            Id = q.Id,
            Title = q.Name,
            Description = q.Description,
            Begining = q.Begining,
            Finish = q.Finish,
            Questions = q.Questions.Count
        };

        public static Expression<Func<QuestionnaireSheet, QuestionnaireHeaderDto>> QuestionnaireHeaderSelector { get; } = q =>
        new QuestionnaireHeaderDto
        {
            Id = q.Id,
            Title = q.Name,
            Begining = q.Begining,
            Finish = q.Finish,
            VisibleToGroup = q.VisibleToGroup,
            Created = q.Created
        };

        public async Task<QuestionnaireDto> CreateQuestionnaire(string userId, QuestionnaireDto questionnaireDto)
        {
            var userGroup = await GetUserGroupByGroupAndUser(userId, questionnaireDto.GroupId);

            if(userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaire = new QuestionnaireSheet
            {
                GroupId = questionnaireDto.GroupId,
                Name = questionnaireDto.Title,
                Description = questionnaireDto.Description,
                Begining = questionnaireDto.Begining,
                Finish = questionnaireDto.Finish,
                VisibleToGroup = questionnaireDto.VisibleToGroup,
                Created = DateTime.Now,
                LastEdited = DateTime.Now
            };

            var group = await _dbContext.Groups
                .Include(g => g.QuestionnaireSheets)
                .FirstOrDefaultAsync(g => g.Id == questionnaireDto.GroupId);

            group.QuestionnaireSheets.Add(questionnaire);

            await _dbContext.SaveChangesAsync();

            return questionnaireDto;
        }

        public async Task<QuestionnaireDetailsDto> GetQuestionnaire(int questionnaireId)
        {
            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionnaireId)
                .Select(QuestionnaireDetailsSelector)
                .FirstOrDefaultAsync();

            return questionnaire;
        }

        public async Task<QuestionnaireStartDto> GetQuestionnaireStart(int questionnaireId)
        {
            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionnaireId)
                .Select(QuestionnaireStartSelector)
                .FirstOrDefaultAsync();

            return questionnaire;
        }

        public async Task<IEnumerable<QuestionnaireHeaderDto>> GetQuestionnaires(int groupId, string userId)
        {
            var questionnaires = await _dbContext.QuestionnaireSheets
                .Include(q => q.Group)
                    .ThenInclude(g => g.UserGroups)
                .Where(q => q.GroupId == groupId && q.Group.UserGroups.Any(ug => ug.UserId == userId))
                .Select(QuestionnaireHeaderSelector)
                .ToListAsync();

            return questionnaires;
        }

        public async Task<QuestionnaireDto> UpdateQuestionnaire(string userId, QuestionnaireDto questionnaireDto)
        {
            var userGroup = await GetUserGroupByQuestionnaireAndUser(userId, questionnaireDto.Id);

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaire = await _dbContext.QuestionnaireSheets
                .FirstOrDefaultAsync(g => g.Id == questionnaireDto.Id);

            //Todo Questionnaire is visible exception
            if (questionnaire.VisibleToGroup)
            {
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

            questionnaire.Name = questionnaireDto.Title;
            questionnaire.Description = questionnaireDto.Description;
            questionnaire.Begining = questionnaireDto.Begining;
            questionnaire.Finish = questionnaireDto.Finish;
            //questionnaire.VisibleToGroup = questionnaireDto.VisibleToGroup;
            questionnaire.LastEdited = DateTime.Now;

            _dbContext.Attach(questionnaire).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return questionnaireDto;
        }

        public async Task HideQuestionnaire(string userId, int questionnaireId)
        {
            var userGroup = await GetUserGroupByQuestionnaireAndUser(userId, questionnaireId);

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.UserQuestionnaires)
                .Where(q => q.Id == questionnaireId)
                .FirstOrDefaultAsync();

            if (questionnaire.UserQuestionnaires.Any())
            {
                throw new QuestionnaireNotEditableException("Questionnaire can't be edited, it is already started!");
            }

            questionnaire.VisibleToGroup = false;

            _dbContext.Attach(questionnaire).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ShowQuestionnaire(string userId, int questionnaireId)
        {
            var userGroup = await GetUserGroupByQuestionnaireAndUser(userId, questionnaireId);

            if(userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Where(q => q.Id == questionnaireId)
                .FirstOrDefaultAsync();

            questionnaire.VisibleToGroup = true;

            _dbContext.Attach(questionnaire).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserGroup> GetUserGroupByQuestionnaireAndUser(string userId, int questionnaireId)
        {
            var questionnaire = await _dbContext.QuestionnaireSheets
                .Where(q => q.Id == questionnaireId)
                .FirstOrDefaultAsync();

            if(questionnaire == null)
            {
                return null;
            }

            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == questionnaire.GroupId)
                .FirstOrDefaultAsync();

            return userGroup;
        }

        private async Task<UserGroup> GetUserGroupByGroupAndUser(string userId, int groupId)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == groupId)
                .FirstOrDefaultAsync();

            return userGroup;
        }
    }
}
