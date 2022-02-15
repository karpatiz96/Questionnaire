using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class GroupDetailsDto
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String GroupAdmin { get; set; }

        public String Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastPost { get; set; }

        public int Members { get; set; }

        public ICollection<QuestionnaireHeaderDto> Questionnaires { get; set; }
    }
}
