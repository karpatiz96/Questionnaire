using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class UserQuestionnaireAnswerDto
    {
        //Questionnaire Id
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public string UserAnswer { get; set; }
    }
}
