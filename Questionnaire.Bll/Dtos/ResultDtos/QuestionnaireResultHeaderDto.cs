using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class QuestionnaireResultHeaderDto
    {
        //UserQuestionnaire
        public int Id { get; set; }

        public string UserName { get; set; }

        public int MaximumPoints { get; set; }

        public int Points { get; set; }

        public DateTime Start { get; set; }

        public DateTime CompletedTime { get; set; }

        public bool Completed { get; set; }
    }
}
