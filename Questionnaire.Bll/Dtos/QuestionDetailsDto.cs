using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionDetailsDto : QuestionDto
    {
        public ICollection<AnswerHeaderDto> Answers { get; set; }
    }
}
