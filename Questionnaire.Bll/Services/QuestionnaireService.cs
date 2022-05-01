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
            Limited = q.LimitedTime,
            TimeLimit = q.TimeLimit,
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

        public static Expression<Func<QuestionnaireSheet, QuestionnaireDto>> QuestionnaireDtoSelector { get; } = q => new QuestionnaireDto
        {
            Id = q.Id,
            Title = q.Name,
            Description = q.Description,
            Begining = q.Begining,
            Finish = q.Finish,
            VisibleToGroup = q.VisibleToGroup,
            RandomQuestionOrder = q.RandomQuestionOrder,
            Limited = q.LimitedTime,
            TimeLimit = q.TimeLimit,
            GroupId = q.GroupId
        };

        public static Expression<Func<QuestionnaireSheet, QuestionnaireStartDto>> QuestionnaireStartSelector { get; } = q => new QuestionnaireStartDto
        {
            Id = q.Id,
            Title = q.Name,
            Description = q.Description,
            Begining = q.Begining,
            Finish = q.Finish,
            Questions = q.Questions.Count,
            Limited = q.LimitedTime,
            TimtLimit = q.TimeLimit
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
            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == questionnaireDto.GroupId)
                .FirstOrDefaultAsync();

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            if(questionnaireDto.Begining >= questionnaireDto.Finish)
            {
                throw new QuestionnaireValidationException("Begining can't be after Finish!");
            }

            if (questionnaireDto.Limited && questionnaireDto.TimeLimit < 1)
            {
                throw new QuestionnaireValidationException("Time limit can't be less than 1!");
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
                LimitedTime = questionnaireDto.Limited,
                TimeLimit = questionnaireDto.Limited ? questionnaireDto.TimeLimit : 1,
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

        public async Task<QuestionnaireDto> GetQuestionnaireById(string userId, int questionnaireId)
        {
            var userGroup = await GetUserGroupByQuestionnaireAndUser(userId, questionnaireId);

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Where(q => q.Id == questionnaireId)
                .Select(QuestionnaireDtoSelector)
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
                throw new UserNotMemberException("User is not member of the Group!");
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

            if (questionnaireDto.Begining >= questionnaireDto.Finish)
            {
                throw new QuestionnaireValidationException("Begining can't be after Finish!");
            }

            if (questionnaireDto.Limited && questionnaireDto.TimeLimit < 1)
            {
                throw new QuestionnaireValidationException("Time limit can't be less than 1!");
            }

            questionnaire.Name = questionnaireDto.Title;
            questionnaire.Description = questionnaireDto.Description;
            questionnaire.Begining = questionnaireDto.Begining;
            questionnaire.Finish = questionnaireDto.Finish;
            questionnaire.RandomQuestionOrder = questionnaireDto.RandomQuestionOrder;
            questionnaire.LastEdited = DateTime.UtcNow;
            questionnaire.LimitedTime = questionnaireDto.Limited;
            questionnaire.TimeLimit = questionnaireDto.Limited ? questionnaireDto.TimeLimit : 1;

            _dbContext.Attach(questionnaire).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return questionnaireDto;
        }

        public async Task<QuestionnaireDto> CopyQuestionnaire(string userId, int questionnaireId)
        {
            var userGroup = await GetUserGroupByQuestionnaireAndUser(userId, questionnaireId);

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var oldQuestionnaire = await _dbContext.QuestionnaireSheets
                .Where(q => q.Id == questionnaireId)
                .FirstOrDefaultAsync();


            var values = _dbContext.Entry(oldQuestionnaire).CurrentValues.Clone();
            var questionnaire = new QuestionnaireSheet();
            _dbContext.Entry(questionnaire).CurrentValues.SetValues(values);
            questionnaire.Id = 0;
            questionnaire.Name = questionnaire.Name + "-copy";
            questionnaire.Created = DateTime.UtcNow;
            questionnaire.LastEdited = DateTime.UtcNow;
            questionnaire.VisibleToGroup = false;
            _dbContext.QuestionnaireSheets.Add(questionnaire);
            await _dbContext.SaveChangesAsync();

            var questions = await _dbContext.Questions
                .OrderBy(q => q.Id)
                .Where(q => q.QuestionnaireSheetId == questionnaireId)
                .ToListAsync();

            foreach (var question in questions)
            {
                var questionValue = _dbContext.Entry(question).CurrentValues.Clone();
                var newQuestion = new Question();
                _dbContext.Entry(newQuestion).CurrentValues.SetValues(questionValue);
                newQuestion.Id = 0;
                newQuestion.QuestionnaireSheetId = questionnaire.Id;
                _dbContext.Questions.Add(newQuestion);

                await _dbContext.SaveChangesAsync();

                var answers = await _dbContext.Answers
                    .OrderBy(a => a.Id)
                    .Where(a => a.QuestionId == question.Id)
                    .ToListAsync();

                foreach (var answer in answers)
                {
                    var answerValue = _dbContext.Entry(answer).CurrentValues.Clone();
                    var newAnswer = new Answer();
                    _dbContext.Entry(newAnswer).CurrentValues.SetValues(answerValue);
                    newAnswer.Id = 0;
                    newAnswer.QuestionId = question.Id;
                    _dbContext.Answers.Add(newAnswer);
                }

                await _dbContext.SaveChangesAsync();
            }

            var result = await _dbContext.QuestionnaireSheets
                .Where(q => q.Id == questionnaire.Id)
                .Select(QuestionnaireDtoSelector)
                .FirstOrDefaultAsync();

            return result;
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
            questionnaire.LastEdited = DateTime.UtcNow;

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
            questionnaire.LastEdited = DateTime.UtcNow;

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
                throw new NotFoundException("Questionnaire doesn't exist!");
            }

            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == questionnaire.GroupId)
                .FirstOrDefaultAsync();

            return userGroup;
        }
    }
}
