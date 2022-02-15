using System;
using System.Collections.Generic;
using System.Text;
using static Questionnaire.Dll.Entities.Invitation;

namespace Questionnaire.Bll.Dtos
{
    public class InvitationDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public String GroupName { get; set; }

        public String UserId { get; set; }

        public String Email { get; set; }

        public DateTime GroupCreated { get; set; }

        public DateTime Date { get; set; }

        public String AdminName { get; set; }

        public InvitationStatus Status { get; set; }
    }
}
