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

        Task<AnswerDto> CreateAnswer(AnswerDto answerDto);

        Task<Answer> UpdateAnswer(AnswerDto answerDto);

        Task<Answer> DeleteAnswer(int answerId);
    }
}
