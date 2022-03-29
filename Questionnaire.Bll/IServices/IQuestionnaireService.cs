using Questionnaire.Bll.Dtos;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IQuestionnaireService
    {
        Task<IEnumerable<QuestionnaireHeaderDto>> GetQuestionnaires(string userId, QuestionnaireListQueryDto queryDto);

        Task<QuestionnaireDetailsDto> GetQuestionnaire(int questionnaireId);

        Task<QuestionnaireDto> GetQuestionnaireById(string userId, int questionnaireId);

        Task<QuestionnaireStartDto> GetQuestionnaireStart(int questionnaireId);

        Task<QuestionnaireDto> CreateQuestionnaire(string userId, QuestionnaireDto questionnaireDto);

        Task<QuestionnaireDto> UpdateQuestionnaire(string userId, QuestionnaireDto questionnaireDto);

        Task HideQuestionnaire(string userId, int questionnaireId);

        Task ShowQuestionnaire(string userId, int questionnaireId);

        //Returns null if user is not part of group
        Task<UserGroup> GetUserGroupByQuestionnaireAndUser(string userId, int questionnaireId);
    }
}
