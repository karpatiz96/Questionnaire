using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class UserQuestionAnswerHeaderDto
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }

        public QuestionType Type { get; set; }

        public int MaximumPoints { get; set; }

        public int Points { get; set; }

        public bool Evaluated { get; set; }

        public DateTime Finished { get; set; }

        public bool Completed { get; set; }
    }
}
