using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class QuestionAnswerResultDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Correct { get; set; }
    }
}
