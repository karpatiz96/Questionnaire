using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class Answer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Type { get; set; }

        public bool TrueOrFalse { get; set; }

        public string UserAnswer { get; set; }

        public int Points { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
