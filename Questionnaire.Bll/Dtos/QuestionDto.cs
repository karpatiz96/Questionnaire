using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public int QuestionnaireId { get; set; }

        public int Number { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public QuestionType Type { get; set; }

        public int Value { get; set; }

        public int SuggestedTime { get; set; }
    }
}
