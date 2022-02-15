using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class InvitationNewDto
    {
        public int Id { get; set; }

        public String Email { get; set; }

        public int GroupId { get; set; }
    }
}
