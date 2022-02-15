using Questionnaire.Bll.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class AnswerHeaderDto
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public AnswerType Type { get; set; }

        public int Value { get; set; }
    }
}
