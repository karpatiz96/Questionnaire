using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class Question
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public QuestionType Type { get; set; }

        public int MaximumPoints { get; set; }

        //minutes
        public int SuggestedTime { get; set; }

        public int QuestionnaireSheetId { get; set; }

        public QuestionnaireSheet QuestionnaireSheet { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public enum QuestionType
        {
                TrueOrFalse = 0,
                MultipleChoice = 1,
                FreeText = 2,
                ConcreteText = 3
        }
    }
}
