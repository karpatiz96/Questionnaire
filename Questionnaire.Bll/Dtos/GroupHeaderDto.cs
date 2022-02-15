using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class GroupHeaderDto
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastPost { get; set; }

        public int Members { get; set; }
    }
}
