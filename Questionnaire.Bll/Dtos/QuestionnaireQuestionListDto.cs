using Questionnaire.Bll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionnaireQuestionListDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Limited { get; set; }

        public int TimeLimit { get; set; }

        public ICollection<QuestionListDto> Questions { get; set; }
    }
}
