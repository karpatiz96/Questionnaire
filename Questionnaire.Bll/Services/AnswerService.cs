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
    public class AnswerService: IAnswerService
    {
        private QuestionnaireDbContext _dbContext;

        public AnswerService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<Answer, AnswerDetailsDto>> SelectAnswerDetails { get; } = a => 
        new AnswerDetailsDto
        {
            Id = a.Id,
            Name = a.Name,
            Value = a.Points,
            Type = a.Type ? AnswerType.Correct : AnswerType.False,
            QuestionType = a.Question.Type,
            UserAnswer = a.UserAnswer
        };

        public static Expression<Func<Answer, AnswerHeaderDto>> SelectAnswerHeader { get; } = a =>
        new AnswerHeaderDto
        {
            Id = a.Id,
            Name = a.Name,
            Value = a.Points,
            Type = a.Type ? AnswerType.Correct : AnswerType.False
        };

        public async Task<AnswerDto> CreateAnswer(AnswerDto answerDto)
        {
            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .Where(q => q.Id == answerDto.QuestionId)
                .FirstOrDefaultAsync();

            var answer = new Answer
            {
                Name = answerDto.Name,
                QuestionId = answerDto.QuestionId,
                Question = question,
                TrueOrFalse = true,
                Type = answerDto.Type == 0 ? true : false,
                Points = answerDto.Value,
                UserAnswer = answerDto.UserAnswer
            };

            question.Answers.Add(answer);

            _dbContext.Answers.Add(answer);

            await _dbContext.SaveChangesAsync();

            return answerDto;
        }

        public async Task<Answer> DeleteAnswer(int answerId)
        {
            var answer = await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == answerId);

            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);

            question.Answers.Remove(answer);

            answer.Question = null;

            _dbContext.Answers.Remove(answer);

            await _dbContext.SaveChangesAsync();

            return answer;
        }

        public async Task<AnswerDetailsDto> GetAnswer(int answerId)
        {
            var result = await _dbContext.Answers
                .Include(a => a.Question)
                .Where(a => a.Id == answerId)
                .Select(SelectAnswerDetails)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<AnswerHeaderDto>> GetAnswers(int questionId)
        {
            var result = await _dbContext.Answers
               .Include(a => a.Question)
               .Where(a => a.QuestionId == questionId)
               .Select(SelectAnswerHeader)
               .ToListAsync();

            return result;
        }

        public async Task<Answer> UpdateAnswer(AnswerDto answerDto)
        {
            var answer = await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == answerDto.Id);

            answer.Name = answerDto.Name;
            answer.UserAnswer = answerDto.UserAnswer;
            answer.Points = answerDto.Value;

            if (answerDto.Type == AnswerType.Correct) {
                answer.Type = true;
                answer.TrueOrFalse = true;
            } else {
                answer.Type = false;
                answer.TrueOrFalse = false;
            }

            await _dbContext.SaveChangesAsync();

            return answer;
        }
    }
}
