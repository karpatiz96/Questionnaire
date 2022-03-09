using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.Enums;
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
    public class QuestionService: IQuestionService
    {
        private QuestionnaireDbContext _dbContext;

        public QuestionService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<Question, QuestionDetailsDto>> SelectQuestionDetails { get; } = q => new QuestionDetailsDto
        {
            Id = q.Id,
            QuestionnaireId = q.QuestionnaireSheetId,
            Name = q.Name,
            Description = q.Description,
            Number = q.Number,
            Type = q.Type,
            SuggestedTime = q.SuggestedTime,
            Value = q.MaximumPoints,
            VisibleToGroup = q.QuestionnaireSheet.VisibleToGroup,
            Answers = q.Answers.Select(q => new AnswerHeaderDto 
            {
                Id = q.Id,
                Name = q.Name,
                Type = q.Type ? AnswerType.Correct : AnswerType.False,
                Value = q.Points
            }).ToList()
        };

        public static Expression<Func<Question, QuestionnaireQuestionDto>> SelectQuestionnaireQuestion { get; } = g => new QuestionnaireQuestionDto
        {
            Id = g.QuestionnaireSheetId,
            QuestionId = g.Id,
            QuestionnaireTitle = g.QuestionnaireSheet.Name,
            QuestionTitle = g.Name,
            Description = g.Description,
            Points = g.MaximumPoints,
            Type = g.Type,
            Answers = g.Answers.Select(a => new QuestionAnswerDto
            {
                Id = a.Id,
                Name = a.Name,
                QuestionId = a.QuestionId
            }).ToList()
        };

        public async Task<QuestionDto> CreateQuestion(string userId, QuestionDto questionDto)
        {
            var userGroup = await GetUserGroupByQuestionnaireAndUser(userId, questionDto.QuestionnaireId);

            if(userGroup == null)
            {
                throw new UserGroupNotFoundExcetpion("User is not member of group!");
            }

            if(userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionDto.QuestionnaireId)
                .SingleOrDefaultAsync();

            if (questionnaire.VisibleToGroup)
            {
                //Todo error Questionnaire can't be edited
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

            var question = new Question
            {
                Name = questionDto.Name,
                Description = questionDto.Description,
                Type = questionDto.Type,
                MaximumPoints = questionDto.Value,
                SuggestedTime = questionDto.SuggestedTime,
                Number = questionDto.Number,
                QuestionnaireSheetId = questionDto.QuestionnaireId
            };

            questionnaire.Questions.Add(question);

            _dbContext.Questions.Add(question);

            await _dbContext.SaveChangesAsync();

            if(question.Type == Question.QuestionType.TrueOrFalse)
            {
                var result = await CreateTrueOrFalseAnswers(question.Id);
            }

            return questionDto;
        }

        public async Task<Question> DeleteQuestion(string userId, int questionId)
        {
            var userGroup = await GetUserGroupByQuestionAndUser(userId, questionId);

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var question = await _dbContext.Questions
               .Include(q => q.Answers)
               .FirstOrDefaultAsync(q => q.Id == questionId);

            var answers = await _dbContext.Answers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync();

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Include(q => q.UserQuestionnaires)
                .Where(q => q.Id == question.QuestionnaireSheetId)
                .FirstOrDefaultAsync();

            if(questionnaire.VisibleToGroup || questionnaire.UserQuestionnaires.Any())
            {
                //Todo throw error questionnaire can't be edited
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

            question.Answers = null;
            questionnaire.Questions.Remove(question);
            
            foreach(var answer in answers)
            {
                _dbContext.Answers.Remove(answer);
            }

            _dbContext.Questions.Remove(question);

            await _dbContext.SaveChangesAsync();

            return question;
        }

        public async Task<QuestionDetailsDto> GetQuestion(int questionId)
        {
            var question = await _dbContext.Questions
                .Include(q => q.QuestionnaireSheet)
                .Where(q => q.Id == questionId)
                .Select(SelectQuestionDetails)
                .FirstOrDefaultAsync();

            return question;
        }

        //Not needed
        public async Task<QuestionnaireQuestionDto> GetQuestionnaireQuestion(int questionId)
        {
            var question = await _dbContext.Questions
                .Include(q => q.QuestionnaireSheet)
                .Include(q => q.Answers)
                .Where(q => q.Id == questionId)
                .Select(SelectQuestionnaireQuestion)
                .FirstOrDefaultAsync();

            return question;
        }

        public async Task<IEnumerable<QuestionnaireQuestionDto>> GetQuestionnaireQuestions(int questionnaireId)
        {
            var questions = await _dbContext.Questions
                .Include(q => q.QuestionnaireSheet)
                .Include(q => q.Answers)
                .Where(q => q.QuestionnaireSheetId == questionnaireId)
                .OrderBy(q => q.Id)
                .Select(SelectQuestionnaireQuestion)
                .ToListAsync();

            return questions;
        }

        public async Task<QuestionDto> UpdateQuestion(string userId, QuestionDto questionDto)
        {
            var userGroup = await GetUserGroupByQuestionAndUser(userId, questionDto.Id);

            if (userGroup == null)
            {
                throw new UserGroupNotFoundExcetpion("User is not member of group!");
            }

            if (userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin in group!");
            }

            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .Include(q => q.QuestionnaireSheet)
                .FirstOrDefaultAsync(q => q.Id == questionDto.Id);

            if (question.QuestionnaireSheet.VisibleToGroup)
            {
                //throw error Questionnaire can't be edited
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

            question.Name = questionDto.Name;
            question.MaximumPoints = questionDto.Value;
            question.Description = questionDto.Description;
            question.Number = questionDto.Number;
            question.SuggestedTime = questionDto.SuggestedTime;

            if (question.Type != questionDto.Type)
            {
                question.Type = questionDto.Type;
                question.Answers = null;

                var answers = await _dbContext.Answers
                    .Where(a => a.QuestionId == question.Id)
                    .ToListAsync();

                _dbContext.Answers.RemoveRange(answers);

                await _dbContext.SaveChangesAsync();

                if (question.Type == Question.QuestionType.TrueOrFalse)
                {
                    await CreateTrueOrFalseAnswers(question.Id);
                }
            } else {

                //Update Correct points if maximum changed
                if (question.Type == Question.QuestionType.TrueOrFalse)
                {
                    var answers = await _dbContext.Answers
                    .Where(a => a.QuestionId == question.Id)
                    .ToListAsync();

                    foreach(var answer in answers)
                    {
                        if(answer.Type && answer.Points != question.MaximumPoints)
                        {
                            answer.Points = question.MaximumPoints;
                        }
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            return questionDto;
        }

        private async Task<Question> CreateTrueOrFalseAnswers(int questionId)
        {
            var newQuestion = await _dbContext.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            var correctAnswer = new Answer
            {
                Name = "True",
                Type = true,
                TrueOrFalse = true,
                Points = newQuestion.MaximumPoints,
                QuestionId = questionId,
                UserAnswer = ""
            };

            var falseAnswer = new Answer()
            {
                Name = "False",
                Type = false,
                TrueOrFalse = false,
                Points = 0,
                QuestionId = questionId,
                UserAnswer = ""
            };

            newQuestion.Answers.Add(correctAnswer);
            newQuestion.Answers.Add(falseAnswer);

            _dbContext.Answers.Add(correctAnswer);
            _dbContext.Answers.Add(falseAnswer);

            await _dbContext.SaveChangesAsync();

            return newQuestion;
        }

        public async Task<UserGroup> GetUserGroupByQuestionAndUser(string userId, int questionId)
        {
            var question = await _dbContext.Questions
                .Include(q => q.QuestionnaireSheet)
                .Where(q => q.Id == questionId)
                .FirstOrDefaultAsync();

            if(question == null)
            {
                return null;
            }

            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == question.QuestionnaireSheet.GroupId)
                .FirstOrDefaultAsync();

            return userGroup;
        }

        private async Task<UserGroup> GetUserGroupByQuestionnaireAndUser(string userId, int questionnaireId)
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
    }
}
