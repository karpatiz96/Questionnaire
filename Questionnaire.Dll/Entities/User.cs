using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class User: IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        
        [PersonalData]
        public string LastName { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }

        public ICollection<Invitation> Invitations { get; set; }

        public ICollection<UserQuestionnaire> UserQuestionnaires { get; set; }
    }
}
