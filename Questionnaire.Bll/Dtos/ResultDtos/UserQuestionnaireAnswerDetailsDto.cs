using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class UserQuestionnaireAnswerDetailsDto
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string QuestionnaireTitle { get; set; }

        public string QuestionTitle { get; set; }

        public string Description { get; set; }

        public QuestionType Type { get; set; }

        public string UserAnswer { get; set; }

        public int AnswerId { get; set; }

        public int MaximumPoints { get; set; }

        public int Points { get; set; }

        public string UserName { get; set; }

        public bool Evaluated { get; set; }

        public DateTime Finished { get; set; }

        public bool Completed { get; set; }

        public ICollection<QuestionAnswerResultDto> Answers { get; set; }
    }
}
