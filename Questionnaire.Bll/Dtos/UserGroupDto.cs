using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Bll.Dtos
{
    public class UserGroupDto
    {
        public int Id { get; set; }

        public String UserId { get; set; }

        public String Name { get; set; }

        public String Role { get; set; }
    }
}
