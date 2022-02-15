using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public ICollection<QuestionnaireSheet> QuestionnaireSheets { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }

        public ICollection<Invitation> Invitations { get; set; }
    }
}
