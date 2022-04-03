using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class InvitationNewDto
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
