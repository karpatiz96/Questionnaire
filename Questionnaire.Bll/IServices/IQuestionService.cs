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

        Task<QuestionnaireQuestionDto> GetQuestionnaireQuestion(int questionId);

        Task<IEnumerable<QuestionnaireQuestionDto>> GetQuestionnaireQuestions(int questionnaireId);

        Task<QuestionDto> CreateQuestion(string userId, QuestionDto questionDto);

        Task<QuestionDto> UpdateQuestion(string userId, QuestionDto questionDto);

        Task<Question> DeleteQuestion(string userId, int questionId);

        //Return null if userGroup does not exists
        Task<UserGroup> GetUserGroupByQuestionAndUser(string userId, int questionId);
    }
}
