using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionnaireHeaderDto
    {
        public int Id { get; set; }

        public int UserQuestionnaireId { get; set; }

        public String Title { get; set; }

        public DateTime Created { get; set; }

        public DateTime Begining { get; set; }

        public DateTime Finish { get; set; }

        public bool VisibleToGroup { get; set; }
    }
}
