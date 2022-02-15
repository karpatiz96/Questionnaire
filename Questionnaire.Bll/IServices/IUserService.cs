using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IUserService
    {
        Task<UserGroup> GetUserGroup(string userId, int groupId);
    }
}
