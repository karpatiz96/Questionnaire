using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionnaireQuestionDto
    {
        //Questionnaire id
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string QuestionnaireTitle { get; set; }

        public string QuestionTitle { get; set; }

        public string Description { get; set; }

        public int Points { get; set; }

        public QuestionType Type { get; set; }

        public ICollection<QuestionAnswerDto> Answers { get; set; }

    }
}
