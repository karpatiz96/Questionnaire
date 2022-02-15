using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class UserGroup
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public bool MainAdmin { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
