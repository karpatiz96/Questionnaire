﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class UserQuestionnaire
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int QuestionnaireSheetId { get; set; }

        public QuestionnaireSheet QuestionnaireSheet { get; set; }

        public ICollection<UserQuestionnaireAnswer> UserQuestionnaireAnswers { get; set; }
    }
}
