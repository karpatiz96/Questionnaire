using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class QuestionnaireResultDto
    {
        //UserQuestionnaire
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Begining { get; set; }

        public DateTime Finish { get; set; }

        public DateTime Start { get; set; }

        public DateTime CompletedTime { get; set; }

        public bool Completed { get; set; }

        public int Questions { get; set; }

        public int MaximumPoints { get; set; }

        public int Points { get; set; }

        public ICollection<UserQuestionAnswerHeaderDto> Answers { get; set; }
    }
}
