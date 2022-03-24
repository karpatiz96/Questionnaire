using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionnaireDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Begining { get; set; }

        public DateTime Finish { get; set; }

        public bool VisibleToGroup { get; set; }

        public bool RandomQuestionOrder { get; set; }

        public bool Limited { get; set; }

        public int TimeLimit { get; set; }
    }
}
