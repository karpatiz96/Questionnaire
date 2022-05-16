using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class GroupHeaderDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastPost { get; set; }

        public string LastPostName { get; set; }

        public bool QuestionnairePosted { get; set; }

        public int Members { get; set; }
    }
}
