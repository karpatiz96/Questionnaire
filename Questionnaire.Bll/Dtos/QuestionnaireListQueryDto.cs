using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionnaireListQueryDto
    {
        public int GroupId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public bool Visible { get; set; }
    }
}
