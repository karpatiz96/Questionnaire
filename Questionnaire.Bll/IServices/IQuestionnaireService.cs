using Questionnaire.Bll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IQuestionnaireService
    {
        Task<IEnumerable<QuestionnaireHeaderDto>> GetQuestionnaires(int groupId, string userId);

        Task<QuestionnaireDetailsDto> GetQuestionnaire(int questionnaireId);

        Task<QuestionnaireDto> CreateQuestionnaire(QuestionnaireDto questionnaireDto);

        Task<QuestionnaireDto> UpdateQuestionnaire(QuestionnaireDto questionnaireDto);
    }
}
