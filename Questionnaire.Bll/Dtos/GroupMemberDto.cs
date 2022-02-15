using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class GroupMemberDto
    {
        //Group Id
        public int Id { get; set; }
        //Group Name
        public String Name { get; set; }

        public ICollection<InvitationGroupDto> Invitations { get; set; }

        public ICollection<UserGroupDto> Users { get; set; }
    }
}
