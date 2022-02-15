using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionHeaderDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public String Name { get; set; }

        public QuestionType Type { get; set; }

        public int Value { get; set; }
    }
}
