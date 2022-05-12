using Questionnaire.Bll.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos
{
    public class AnswerDetailsDto
    {
        public int Id { get; set; }

        public QuestionType QuestionType { get; set; }

        public String Name { get; set; }

        public String UserAnswer { get; set; }

        public AnswerType Type { get; set; }

        public int Value { get; set; }

        public bool VisibleToGroup { get; set; }
    }
}
