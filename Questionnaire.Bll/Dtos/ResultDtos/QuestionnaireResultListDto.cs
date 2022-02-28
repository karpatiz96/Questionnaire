using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class QuestionnaireResultListDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Begining { get; set; }

        public DateTime Finish { get; set; }

        public int Questions { get; set; }

        public int Solved { get; set; }

        public int Members { get; set; }

        public ICollection<QuestionnaireResultHeaderDto> Results { get; set; }
    }
}
