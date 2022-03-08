using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Dtos.ResultDtos;
using Questionnaire.Dll.Entities;
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

        public Task<bool> UserQuestionnaireExists(string userId, int questionnaireId);

        public Task<QuestionnaireResultDto> GetQuestionnaireResult(string userId, int userQuestionnaireId);

        public Task<QuestionnaireResultListDto> GetQuestionnaireResultList(int questionnaireId);

        public Task<UserQuestionnaireAnswerDetailsDto> GetUserQuestionnaireAnswerDetails(string userId, int userQuestionnaireAnswerId);

        public Task EvaluateUserQuestionnaireAnswer(string userId, UserQuestionnaireAnswerEvaluationDto evaluationDto);

        public Task<UserGroup> GetUserGroupByUserAndQuestionnaire(string userId, int questionnaireId);

        public Task<UserGroup> GetUserGroupByUserAndUserQuestionnaire(string userId, int userQuestionnaireId);

        public Task<UserGroup> GetUserGroupByUserAndUserQuestionnaireAnswer(string userId, int userQuestionnaireAnswerId);
    }
}
