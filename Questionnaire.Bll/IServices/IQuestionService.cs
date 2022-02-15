using Questionnaire.Bll.Dtos;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IQuestionService
    {
        Task<QuestionDetailsDto> GetQuestion(int questionId);

        Task<QuestionDto> CreateQuestion(QuestionDto questionDto);

        Task<QuestionDto> UpdateQuestion(QuestionDto questionnaireDto);

        Task<Question> DeleteQuestion(int questionId);
    }
}
