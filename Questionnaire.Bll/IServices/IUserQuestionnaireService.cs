using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IUserQuestionnaireService
    {
        public Task CreateUserQuestionnaire(string userId, int questionnaireId);

        public Task AnswerQuestion(UserQuestionnaireAnswerDto answerDto, string userId);

        public Task<int> GetUserQuestionnaireQuestion(string userId, int questionnaireId);

        public Task<bool> UserQuestionnaireExists(string userId, int questionnaireId);

        public Task<QuestionnaireResultDto> GetQuestionnaireResult(int userQuestionnaireId);

        public Task<QuestionnaireResultListDto> GetQuestionnaireResultList(int questionnaireId);

        public Task<UserQuestionnaireAnswerDetailsDto> GetUserQuestionnaireAnswerDetails(int userQuestionnaireAnswerId);

        public Task EvaluateUserQuestionnaireAnswer(UserQuestionnaireAnswerEvaluationDto evaluationDto);
    }
}
