using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.IntegrationTest.Helpers
{
    public class TestSeedHelper
    {
        public static async Task InitializeDbForTesting(QuestionnaireDbContext dbContext)
        {
            var userStore = new UserStore<User>(dbContext);
            var passwordHasher = new PasswordHasher<User>();
            var users = GetTestUsers();
            foreach(var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "Test123+");
                await userStore.CreateAsync(user);
            }

            await dbContext.SaveChangesAsync();

        }

        public static List<User> GetTestUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = "123",
                    UserName = "test@test.com",
                    Email = "test@test.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    NormalizedEmail = "TEST@TEST.COM",
                    NormalizedUserName = "TEST@TEST.COM",
                    LockoutEnabled = false
                }
            };
        }
    }
}
