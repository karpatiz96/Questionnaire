using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.Enums;
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
            Answers = q.Answers.Select(q => new AnswerHeaderDto 
            {
                Id = q.Id,
                Name = q.Name,
                Type = q.Type ? AnswerType.Correct : AnswerType.False,
                Value = q.Points
            }).ToList()
        };

        public async Task<QuestionDto> CreateQuestion(QuestionDto questionDto)
        {
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

            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionDto.QuestionnaireId)
                .SingleOrDefaultAsync();

            questionnaire.Questions.Add(question);

            _dbContext.Questions.Add(question);

            await _dbContext.SaveChangesAsync();

            if(question.Type == Question.QuestionType.TrueOrFalse)
            {
                var result = await CreateTrueOrFalseAnswers(question.Id);
            }

            return questionDto;
        }

        public async Task<Question> DeleteQuestion(int questionId)
        {
            var question = await _dbContext.Questions
               .Include(q => q.Answers)
               .FirstOrDefaultAsync(q => q.Id == questionId);

            return question;
        }

        public async Task<QuestionDetailsDto> GetQuestion(int questionId)
        {
            var question = await _dbContext.Questions
                .Where(q => q.Id == questionId)
                .Select(SelectQuestionDetails)
                .FirstOrDefaultAsync();

            return question;
        }

        public async Task<QuestionDto> UpdateQuestion(QuestionDto questionDto)
        {
            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionDto.Id);

            question.Name = questionDto.Name;
            question.MaximumPoints = questionDto.Value;
            question.Description = questionDto.Description;
            question.Number = questionDto.Number;
            question.SuggestedTime = questionDto.SuggestedTime;

            if(question.Type != questionDto.Type)
            {
                question.Type = questionDto.Type;
                question.Answers = null;

                var answers = await _dbContext.Answers
                    .Where(a => a.QuestionId == question.Id)
                    .ToListAsync();

                _dbContext.Answers.RemoveRange(answers);
            }

            await _dbContext.SaveChangesAsync();

            if(question.Type == Question.QuestionType.TrueOrFalse)
            {
                await CreateTrueOrFalseAnswers(question.Id);
            }

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
    }
}
