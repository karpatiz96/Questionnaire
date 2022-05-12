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

        public DateTime? Start { get; set; }

        public DateTime? CompletedTime { get; set; }

        public bool Completed { get; set; }

        public bool Evaluated { get; set; }

        public bool VisibleToGroup { get; set; }
    }
}
