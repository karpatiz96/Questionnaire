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
            UserAnswer = a.UserAnswer,
            VisibleToGroup = a.Question.QuestionnaireSheet.VisibleToGroup
        };

        public static Expression<Func<Answer, AnswerHeaderDto>> SelectAnswerHeader { get; } = a =>
        new AnswerHeaderDto
        {
            Id = a.Id,
            Name = a.Name,
            Value = a.Points,
            Type = a.Type ? AnswerType.Correct : AnswerType.False
        };

        public async Task<AnswerDto> CreateAnswer(string userId, AnswerDto answerDto)
        {
            var userGroup = await GetUserGroupByQuestionAndUser(userId, answerDto.QuestionId);

            if(userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin is group!");
            }

            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .Include(q => q.QuestionnaireSheet)
                .Where(q => q.Id == answerDto.QuestionId)
                .FirstOrDefaultAsync();

            if (question.QuestionnaireSheet.VisibleToGroup)
            {
                //throw error Questionnaire can't be edited
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

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

        public async Task<Answer> DeleteAnswer(string userId, int answerId)
        {
            var userGroup = await GetUserGroupByAnswerAndUser(userId, answerId);

            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin is group!");
            }

            var answer = await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == answerId);

            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .Include(q => q.QuestionnaireSheet)
                .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);

            if (question.QuestionnaireSheet.VisibleToGroup)
            {
                //throw error Questionnaire can't be edited
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

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
                    .ThenInclude(q => q.QuestionnaireSheet)
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

        public async Task<Answer> UpdateAnswer(string userId, AnswerDto answerDto)
        {
            var userGroup = await GetUserGroupByAnswerAndUser(userId, answerDto.Id);
            
            if (userGroup == null || userGroup.Role != "Admin")
            {
                throw new UserNotAdminException("User is not admin is group!");
            }

            var answer = await _dbContext.Answers
                .Include(a => a.Question)
                    .ThenInclude(q => q.QuestionnaireSheet)
                .FirstOrDefaultAsync(a => a.Id == answerDto.Id);

            if (answer.Question.QuestionnaireSheet.VisibleToGroup)
            {
                //throw error questionnaire can't be edited
                throw new QuestionnaireNotEditableException("Questionnaire is visible, it can't be edited!");
            }

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

        public async Task<UserGroup> GetUserGroupByAnswerAndUser(string userId, int answerId)
        {
            var answer = await _dbContext.Answers
                .Include(a => a.Question)
                    .ThenInclude(q => q.QuestionnaireSheet)
                .Where(a => a.Id == answerId)
                .FirstOrDefaultAsync();

            if(answer == null)
            {
                throw new AnswerNotFoundException("Answer doesn't exist!");
            }

            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == answer.Question.QuestionnaireSheet.GroupId)
                .FirstOrDefaultAsync();

            return userGroup;
        }

        private async Task<UserGroup> GetUserGroupByQuestionAndUser(string userId, int questionId)
        {
            var question = await _dbContext.Questions
                .Include(q => q.QuestionnaireSheet)
                .Where(q => q.Id == questionId)
                .FirstOrDefaultAsync();

            if(question == null)
            {
                throw new QuestionNotFoundException("Question doesn't exist!");
            }

            var userGroup = await _dbContext.UserGroups
                .Where(u => u.UserId == userId && u.GroupId == question.QuestionnaireSheet.GroupId)
                .FirstOrDefaultAsync();

            return userGroup;
        }
    }
}
