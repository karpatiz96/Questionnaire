using Questionnaire.Bll.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Dtos
{
    public class AnswerDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public String Name { get; set; }

        public String UserAnswer { get; set; }

        public AnswerType Type { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Points can't be less than 0")]
        public int Value { get; set; }
    }
}
