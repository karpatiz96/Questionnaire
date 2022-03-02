using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.ResultDtos;
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
    public class UserQuestionnaireService : IUserQuestionnaireService
    {
        private QuestionnaireDbContext _dbContext;

        public UserQuestionnaireService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public async Task CreateUserQuestionnaire(string userId, int questionnaireId)
        {
            var user = await _dbContext.Users.Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionnaireId).FirstOrDefaultAsync();

            var userQuestionnaire = new UserQuestionnaire
            {
                QuestionnaireSheetId = questionnaire.Id,
                UserId = user.Id,
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
                    QuestionCompleted = false
                });
            }

            await _dbContext.UserQuestionnaires.AddAsync(userQuestionnaire);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UserQuestionnaireExists(string userId, int questionnaireId)
        {
            var userQuestionnaire = await _dbContext.UserQuestionnaires
                .Where(u => u.UserId == userId && u.QuestionnaireSheetId == questionnaireId)
                .FirstOrDefaultAsync();

            if(userQuestionnaire == null)
            {
                return false;
            }

            return true;
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
                //Todo
            }

            switch (userQuestionnaireAnswer.Question.Type)
            {
                case Question.QuestionType.FreeText:
                    userQuestionnaireAnswer.UserAnswer = answerDto.UserAnswer;
                    userQuestionnaireAnswer.QuestionCompleted = true;
                    break;
                case Question.QuestionType.ConcreteText:
                    userQuestionnaireAnswer.UserAnswer = answerDto.UserAnswer;
                    if (userQuestionnaireAnswer.Question.Answers.Any(a => a.UserAnswer.Trim() == answerDto.UserAnswer.Trim()))
                    {
                        userQuestionnaireAnswer.UserPoints = userQuestionnaireAnswer.Question.MaximumPoints;
                    }

                    userQuestionnaireAnswer.QuestionCompleted = true;
                    break;
                case Question.QuestionType.TrueOrFalse:
                    if(answer != null)
                    {
                        userQuestionnaireAnswer.AnswerId = answerDto.AnswerId;
                        userQuestionnaireAnswer.UserPoints = answer.Points;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    } else {
                        userQuestionnaireAnswer.UserPoints = 0;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    }
                    break;
                case Question.QuestionType.MultipleChoice:
                    if (answer != null)
                    {
                        userQuestionnaireAnswer.AnswerId = answerDto.AnswerId;
                        userQuestionnaireAnswer.UserPoints = answer.Points;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    }
                    else
                    {
                        userQuestionnaireAnswer.UserPoints = 0;
                        userQuestionnaireAnswer.QuestionCompleted = true;
                    }
                    break;
                default:
                    break;
            }

            await _dbContext.SaveChangesAsync();
        }

        public Task<int> GetUserQuestionnaireQuestion(string userId, int questionnaireId)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionnaireResultDto> GetQuestionnaireResult(int userQuestionnaireId)
        {
            var userQuestionnaireResult = await _dbContext.UserQuestionnaires
                .Include(u => u.QuestionnaireSheet)
                .Include(u => u.UserQuestionnaireAnswers)
                    .ThenInclude(q => q.Question)
                .Where(u => u.Id == userQuestionnaireId)
                .Select(u => new QuestionnaireResultDto 
                {
                    Id = u.Id,
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
                            MaximumPoints = a.Question.MaximumPoints
                        }).ToList(),
                    MaximumPoints = u.UserQuestionnaireAnswers.Sum(q => q.Question.MaximumPoints),
                    Points = u.UserQuestionnaireAnswers.Sum(q => q.UserPoints)
                })
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
                .Select(u => new QuestionnaireResultListDto
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
                        MaximumPoints = 0
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return questionnaireResultList;
        }

        public async Task<UserQuestionnaireAnswerDetailsDto> GetUserQuestionnaireAnswerDetails(int userQuestionnaireAnswerId)
        {
            var userQuestionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.Question)
                    .ThenInclude(q => q.QuestionnaireSheet)
                .Include(u => u.Question)
                    .ThenInclude(q => q.Answers)
                .Where(u => u.Id == userQuestionnaireAnswerId)
                .Select(u => new UserQuestionnaireAnswerDetailsDto
                {
                    Id = u.Id,
                    QuestionnaireTitle = u.Question.QuestionnaireSheet.Name,
                    QuestionTitle = u.Question.Name,
                    Type = u.Question.Type,
                    Description = u.Question.Description,
                    MaximumPoints = u.Question.MaximumPoints,
                    Points = u.UserPoints,
                    AnswerId = u.AnswerId ?? -1,
                    UserAnswer = u.UserAnswer,
                    Answers = u.Question.Answers.Select(a => new QuestionAnswerResultDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Correct = a.Type
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return userQuestionnaireAnswer;
        }

        public async Task EvaluateUserQuestionnaireAnswer(UserQuestionnaireAnswerEvaluationDto evaluationDto)
        {
            var userQuestionnaireAnswer = await _dbContext.UserQuestionnaireAnswers
                .Include(u => u.Question)
                .Where(u => u.Id == evaluationDto.Id)
                .FirstOrDefaultAsync();

            if(evaluationDto.Points > userQuestionnaireAnswer.Question.MaximumPoints || evaluationDto.Points < 0)
            {
                //Todo Error
            }

            userQuestionnaireAnswer.UserPoints = evaluationDto.Points;

            await _dbContext.SaveChangesAsync();
        }
    }
}
