using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos.ResultDtos
{
    public class UserQuestionnaireAnswerEvaluationDto
    {
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Points can't be less than 0")]
        public int Points { get; set; }
    }
}
