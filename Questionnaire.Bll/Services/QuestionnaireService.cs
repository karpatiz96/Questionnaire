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
            RandomQuestionOrder = q.RandomQuestionOrder,
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
            UserQuestionnaireId = -1,
            Title = q.Name,
            Begining = q.Begining,
            Finish = q.Finish,
            Start = null,
            CompletedTime = null,
            Completed = false,
            Evaluated = false,
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
                RandomQuestionOrder = questionnaireDto.RandomQuestionOrder,
                Created = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow
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

        public async Task<IEnumerable<QuestionnaireHeaderDto>> GetQuestionnaires(string userId, QuestionnaireListQueryDto queryDto)
        {
            var userGroup = await _dbContext.UserGroups
                .Where(g => g.UserId == userId && g.GroupId == queryDto.GroupId)
                .FirstOrDefaultAsync();

            if (userGroup == null)
            {
                throw new UserGroupNotFoundExcetpion("User is not member of Group!");
            }

            var questionnaires = _dbContext.QuestionnaireSheets
                .Include(q => q.Group)
                .Where(q => q.GroupId == queryDto.GroupId);

            if (queryDto.From != null)
                questionnaires = questionnaires.Where(q => q.Begining >= queryDto.From);
            if(queryDto.To != null)
                questionnaires = questionnaires.Where(q => q.Finish <= queryDto.To);
            if (!queryDto.Visible && userGroup.Role == "Admin")
                questionnaires = questionnaires.Where(q => q.VisibleToGroup);
            if (userGroup.Role == "User")
                questionnaires = questionnaires.Where(q => q.VisibleToGroup);

            var result = await questionnaires.OrderByDescending(q => q.Begining)
                .Select(QuestionnaireHeaderSelector)
                .ToListAsync();

            foreach (var questionnaire in result)
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

            return result;
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
            questionnaire.RandomQuestionOrder = questionnaireDto.RandomQuestionOrder;
            questionnaire.LastEdited = DateTime.UtcNow;

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
