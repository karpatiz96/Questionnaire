using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class Invitation
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public InvitationStatus Status { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public enum InvitationStatus
        {
            Undecided = 0,
            Accepted = 1,
            Declined = 2
        }
    }
}
