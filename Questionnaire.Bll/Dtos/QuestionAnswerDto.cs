using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionAnswerDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Name { get; set; }
    }
}
