using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class UserQuestionnaireAnswer
    {
        public int Id { get; set; }

        public int UserPoints { get; set; }

        public string UserAnswer { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }

        public int? AnswerId { get; set; }

        public Answer Answer { get; set; }

        public int? UserQuestionnaireId { get; set; }

        public UserQuestionnaire UserQuestionnaire { get; set; }
    }
}
