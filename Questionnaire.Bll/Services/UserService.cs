using Questionnaire.Bll.IServices;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Services
{
    public class UserService : IUserService
    {
        private readonly QuestionnaireDbContext _dbContext;

        public UserService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<UserGroup> GetUserGroup(string userId, int groupId)
        {
            throw new NotImplementedException();
        }
    }
}
