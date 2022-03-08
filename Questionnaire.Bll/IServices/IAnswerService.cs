using Questionnaire.Bll.Dtos;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IAnswerService
    {
        Task<AnswerDetailsDto> GetAnswer(int answerId);

        Task<IEnumerable<AnswerHeaderDto>> GetAnswers(int questionId);

        Task<AnswerDto> CreateAnswer(string userId, AnswerDto answerDto);

        Task<Answer> UpdateAnswer(string userId, AnswerDto answerDto);

        Task<Answer> DeleteAnswer(string userId, int answerId);

        Task<UserGroup> GetUserGroupByAnswerAndUser(string userId, int answerId);
    }
}
