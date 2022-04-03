using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Question;

namespace Questionnaire.Bll.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public int QuestionnaireId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Points can't be less than 1")]
        public int Number { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public QuestionType Type { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Points can't be less than 0")]
        public int Value { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Points can't be less than 0")]
        public int SuggestedTime { get; set; }
    }
}
