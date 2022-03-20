using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.ResultDtos;
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
    public class UserQuestionnaireService : IUserQuestionnaireService
    {
        private QuestionnaireDbContext _dbContext;

        public UserQuestionnaireService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<UserQuestionnaire, QuestionnaireResultDto>> SelectQuestionnaireResult { get; } = u =>
        new QuestionnaireResultDto
        {
            Id = u.Id,
            UserName = u.User.UserName,
            Title = u.QuestionnaireSheet.Name,
            Description = u.QuestionnaireSheet.Description,
            Begining = u.QuestionnaireSheet.Begining,
            Finish = u.QuestionnaireSheet.Finish,
            Questions = u.UserQuestionnaireAnswers.Count,
            Answers = u.UserQuestionnaireAnswers.Select(a => new UserQuestionAnswerHeaderDto
            {
                Id = a.Id,
                Index = a.Question.Number,
                Name = a.Question.Name,
                Type = a.Question.Type,
                Points = a.UserPoints,
                MaximumPoints = a.Question.MaximumPoints,
                Evaluated = a.AnswerEvaluated,
                Finished = a.Completed,
                Completed = a.QuestionCompleted
            }).ToList(),
            MaximumPoints = u.UserQuestionnaireAnswers.Sum(q => q.Question.MaximumPoints),
            Points = u.UserQuestionnaireAnswers.Sum(q => q.UserPoints)
        };

        public static Expression<Func<QuestionnaireSheet, QuestionnaireResultListDto>> SelectQuestionnaireResultList { get; } = u => 
        new QuestionnaireResultListDto
        {
            Id = u.Id,
            Title = u.Name,
            Description = u.Description,
            Begining = u.Begining,
            Finish = u.Finish,
            Questions = u.Questions.Count,
            Solved = u.UserQuestionnaires.Count,
            Members = u.Group.UserGroups.Count,
            Results = u.UserQuestionnaires.Select(q => new QuestionnaireResultHeaderDto
            {
                Id = q.Id,
                UserName = q.User.UserName,
                Points = q.UserQuestionnaireAnswers.Sum(a => a.UserPoints),
                MaximumPoints = u.Questions.Sum(u => u.MaximumPoints)
            }).ToList()
        };

        public static Expression<Func<UserQuestionnaireAnswer, UserQuestionnaireAnswerDetailsDto>> SelectUserQuestionnaireAnswerDetails { get; } = u =>
        new UserQuestionnaireAnswerDetailsDto
        {
            Id = u.Id,
            Role = "User",
            QuestionnaireTitle = u.Question.QuestionnaireSheet.Name,
            QuestionTitle = u.Question.Name,
            Type = u.Question.Type,
            Description = u.Question.Description,
            MaximumPoints = u.Question.MaximumPoints,
            Points = u.UserPoints,
            UserName = u.UserQuestionnaire.User.UserName,
            Evaluated = u.AnswerEvaluated,
            Finished = u.Completed,
            Completed = u.QuestionCompleted,
            AnswerId = u.AnswerId ?? -1,
            UserAnswer = u.UserAnswer,
            Answers = u.Question.Answers.Select(a => new QuestionAnswerResultDto
            {
                Id = a.Id,
                Name = a.Name,
                Correct = a.Type
            }).ToList()
        };

        public async Task CreateUserQuestionnaire(string userId, int questionnaireId)
        {
            var user = await _dbContext.Users.Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionnaireId).FirstOrDefaultAsync();

            if(!questionnaire.VisibleToGroup || questionnaire.Begining > DateTime.UtcNow)
            {
                throw new QuestionnaireStartValidationException("Questionnaire can't be started yet!");
            }

            if(questionnaire.Finish < DateTime.UtcNow)
            {
                throw new QuestionnaireStartValidationException("Questionnaire is already finished!");
            }

            var userQuestionnaire = new UserQuestionnaire
            {
                QuestionnaireSheetId = questionnaire.Id,
                UserId = user.Id,
                Started = DateTime.UtcNow,
                UserQuestionnaireAnswers = new List<UserQuestionnaireAnswer>()
            };

            var questions = questionnaire.Questions.ToList();

            foreach(var question in questions)
            {
                userQuestionnaire.UserQuestionnaireAnswers.Add(new UserQuestionnaireAnswer
                {
                    QuestionId = question.Id,
                    AnswerId = null,
                    UserAnswer = "",
                    UserPoints = 0,
                    QuestionCompleted = false,
                    AnswerEvaluated = false
                });
            }

            await _dbContext.UserQuestionnaires.AddAsync(userQuestionnaire);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AnswerQuestion(UserQuestionnaireAnswerDto answerDto, string userId)
        {
            var userQuestionnaire = await _dbContext.UserQuestionnaires
                .Where(u => u.QuestionnaireSheetId == answerDto.Id && u.UserId == userId)
                .FirstOrDefaultAsync();

            var userQuestionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.Question)
                    .ThenInclude(q => q.Answers)
                .Where(u => u.UserQuestionnaireId == userQuestionnaire.Id && u.QuestionId == answerDto.QuestionId)
                .FirstOrDefaultAsync();

            var answer = userQuestionnaireAnswer.Question.Answers.Where(a => a.Id == answerDto.AnswerId).FirstOrDefault();

            if (userQuestionnaire == null || userQuestionnaireAnswer == null)
            {
                throw new QuestionAnswerValidationException("Questionnaire is not started yet!");
            }

            if (userQuestionnaireAnswer.QuestionCompleted)
            {
                throw new QuestionAnswerValidationException("Question is already answered");
            }

            switch (userQuestionnaireAnswer.Question.Type)
            {
                case Question.QuestionType.FreeText:
                    userQuestionnaireAnswer.UserAnswer = answerDto.UserAnswer;
                    userQuestionnaireAnswer.Completed = DateTime.UtcNow;
                    userQuestionnaireAnswer.QuestionCompleted = true;
                    break;
                case Question.QuestionType.ConcreteText:
                    userQuestionnaireAnswer.UserAnswer = answerDto.UserAnswer;
                    if (userQuestionnaireAnswer.Question.Answers.Any(a => a.UserAnswer.Trim() == answerDto.UserAnswer.Trim()))
                    {
                        userQuestionnaireAnswer.UserPoints = userQuestionnaireAnswer.Question.MaximumPoints;
                    }
                    userQuestionnaireAnswer.Completed = DateTime.UtcNow;
                    userQuestionnaireAnswer.QuestionCompleted = true;
                    break;
                case Question.QuestionType.TrueOrFalse:
                    if(answer != null)
                    {
                        userQuestionnaireAnswer.AnswerId = answerDto.AnswerId;
                        userQuestionnaireAnswer.UserPoints = answer.Points;
                        userQuestionnaireAnswer.Completed = DateTime.UtcNow;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    } else {
                        userQuestionnaireAnswer.UserPoints = 0;
                        userQuestionnaireAnswer.Completed = DateTime.UtcNow;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    }
                    break;
                case Question.QuestionType.MultipleChoice:
                    if (answer != null)
                    {
                        userQuestionnaireAnswer.AnswerId = answerDto.AnswerId;
                        userQuestionnaireAnswer.UserPoints = answer.Points;
                        userQuestionnaireAnswer.Completed = DateTime.UtcNow;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    }
                    else
                    {
                        userQuestionnaireAnswer.UserPoints = 0;
                        userQuestionnaireAnswer.Completed = DateTime.UtcNow;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    }
                    break;
                default:
                    break;
            }

            var finalAnswer = await _dbContext.UserQuestionnaireAnswers
                .Where(u => u.UserQuestionnaireId == userQuestionnaire.Id)
                .AnyAsync(u => u.QuestionCompleted == false);

            if (finalAnswer)
            {
                userQuestionnaire.Finished = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<QuestionnaireResultDto> GetQuestionnaireResult(string userId, int userQuestionnaireId)
        {
            //check user is admin or solver
            var userGroup = await GetUserGroupByUserAndUserQuestionnaire(userId, userQuestionnaireId);
            if(userGroup == null)
            {
                throw new UserGroupNotFoundExcetpion("User is not member of group!");
            }

            var userQuestionnaire = await _dbContext.UserQuestionnaires
                .Where(u => u.Id == userQuestionnaireId)
                .FirstOrDefaultAsync();

            if(userGroup.Role != "Admin" && userQuestionnaire.UserId != userId)
            {
                //Todo exception
                throw new QuestionnaireResultValidationException("User is not admin or solver!");
            }

            var userQuestionnaireResult = await _dbContext.UserQuestionnaires
                .Include(u => u.User)
                .Include(u => u.QuestionnaireSheet)
                .Include(u => u.UserQuestionnaireAnswers)
                    .ThenInclude(q => q.Question)
                .Where(u => u.Id == userQuestionnaireId)
                .Select(SelectQuestionnaireResult)
                .FirstOrDefaultAsync();

            return userQuestionnaireResult;
        }

        public async Task<QuestionnaireResultListDto> GetQuestionnaireResultList(int questionnaireId)
        {
            var questionnaireResultList = await _dbContext.QuestionnaireSheets
                .Include(q => q.UserQuestionnaires)
                    .ThenInclude(u => u.User)
                .Include(q => q.UserQuestionnaires)
                    .ThenInclude(u => u.UserQuestionnaireAnswers)
                .Include(q => q.Questions)
                .Include(q => q.Group)
                    .ThenInclude(g => g.UserGroups)
                .Where(q => q.Id == questionnaireId)
                .Select(SelectQuestionnaireResultList)
                .FirstOrDefaultAsync();

            return questionnaireResultList;
        }

        public async Task<UserQuestionnaireAnswerDetailsDto> GetUserQuestionnaireAnswerDetails(string userId, int userQuestionnaireAnswerId)
        {
            var userGroup = await GetUserGroupByUserAndUserQuestionnaireAnswer(userId, userQuestionnaireAnswerId);
            if (userGroup == null)
            {
                throw new UserGroupNotFoundExcetpion("User is not memeber of group!");
            }

            var questionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.UserQuestionnaire)
                .Where(u => u.Id == userQuestionnaireAnswerId)
                .FirstOrDefaultAsync();

            if(userGroup.Role != "Admin" && questionnaireAnswer.UserQuestionnaire.UserId != userId)
            {
                throw new QuestionnaireResultValidationException("User is not admin or solver!");
            }

            var userQuestionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.Question)
                    .ThenInclude(q => q.QuestionnaireSheet)
                .Include(u => u.Question)
                    .ThenInclude(q => q.Answers)
                .Include(u => u.UserQuestionnaire)
                    .ThenInclude(q => q.User)
                .Where(u => u.Id == userQuestionnaireAnswerId)
                .Select(SelectUserQuestionnaireAnswerDetails)
                .FirstOrDefaultAsync();

            if(userGroup.Role == "Admin")
            {
                userQuestionnaireAnswer.Role = "Admin";
            }

            return userQuestionnaireAnswer;
        }

        public async Task EvaluateUserQuestionnaireAnswer(string userId, UserQuestionnaireAnswerEvaluationDto evaluationDto)
        {
            var userGroup = await GetUserGroupByUserAndUserQuestionnaireAnswer(userId, evaluationDto.Id);

            if(userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var userQuestionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.Question)
                .Where(u => u.Id == evaluationDto.Id)
                .FirstOrDefaultAsync();

            if(evaluationDto.Points > userQuestionnaireAnswer.Question.MaximumPoints || evaluationDto.Points < 0)
            {
                //Todo Error
            }

            userQuestionnaireAnswer.UserPoints = evaluationDto.Points;
            userQuestionnaireAnswer.AnswerEvaluated = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UserQuestionnaireExists(string userId, int questionnaireId)
        {
            var userQuestionnaire = await _dbContext.UserQuestionnaires
                .Where(u => u.UserId == userId && u.QuestionnaireSheetId == questionnaireId)
                .FirstOrDefaultAsync();

            if (userQuestionnaire == null)
            {
                return false;
            }

            return true;
        }

        public async Task<UserGroup> GetUserGroupByUserAndQuestionnaire(string userId, int questionnaireId)
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

        public async Task<UserGroup> GetUserGroupByUserAndUserQuestionnaire(string userId, int userQuestionnaireId)
        {
            var userQuestionnaire = await _dbContext.UserQuestionnaires
                .Include(u => u.QuestionnaireSheet)
                .Where(u => u.Id == userQuestionnaireId)
                .FirstOrDefaultAsync();

            if(userQuestionnaire == null)
            {
                return null;
            }

            var userGroup = await _dbContext.UserGroups
               .Where(u => u.UserId == userId && u.GroupId == userQuestionnaire.QuestionnaireSheet.GroupId)
               .FirstOrDefaultAsync();

            return userGroup;
        }

        public async Task<UserGroup> GetUserGroupByUserAndUserQuestionnaireAnswer(string userId, int userQuestionnaireAnswerId)
        {
            var userQuestionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.UserQuestionnaire)
                    .ThenInclude(q => q.QuestionnaireSheet)
                .Where(u => u.Id == userQuestionnaireAnswerId)
                .FirstOrDefaultAsync();

            if(userQuestionnaireAnswer == null)
            {
                return null;
            }

            var userGroup = await _dbContext.UserGroups
               .Where(u => u.UserId == userId && u.GroupId == userQuestionnaireAnswer.UserQuestionnaire.QuestionnaireSheet.GroupId)
               .FirstOrDefaultAsync();

            return userGroup;
        }
    }
}
