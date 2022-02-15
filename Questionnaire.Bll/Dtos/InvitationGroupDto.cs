using System;
using System.Collections.Generic;
using System.Text;
using static Questionnaire.Dll.Entities.Invitation;

namespace Questionnaire.Bll.Dtos
{
    public class InvitationGroupDto
    {
        public int Id { get; set; }

        public String UserId { get; set; }

        public String UserName { get; set; }

        public DateTime Date { get; set; }

        public InvitationStatus Status { get; set; }
    }
}
