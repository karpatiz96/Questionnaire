using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.UnitTest.Helpers
{
    public class TestQuestionnaireDatabaseFixture
    {
        private const string ConnectionString = "";

        private static readonly object _lock = new();
        private static bool Initialized;

        public TestQuestionnaireDatabaseFixture()
        {
            lock(_lock)
            {
                if(!Initialized)
                {
                    using(var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var userStore = new UserStore<User>(context);
                        var passwordHasher = new PasswordHasher<User>();
                        var users = UnitTestSeedHelper.GetTestUsers();
                        foreach (var user in users)
                        {
                            user.PasswordHash = passwordHasher.HashPassword(user, "Test123+");
                        }

                        context.Users.AddRange(users);

                        context.SaveChanges();

                        InitializeDatabase(context);
                    }

                    Initialized = true;
                }
            }
        }

        public void InitializeDatabase(QuestionnaireDbContext dbContext)
        {
            if (!dbContext.Groups.Any())
            {
                var groups = UnitTestSeedHelper.GetGroups();

                dbContext.Groups.AddRange(groups);

                dbContext.SaveChanges();
            }

            if (!dbContext.UserGroups.Any())
            {
                var groups = dbContext.Groups.OrderBy(g => g.Id).ToList();

                var userGroups = UnitTestSeedHelper.GetUserGroups(groups);

                dbContext.UserGroups.AddRange(userGroups);

                dbContext.SaveChanges();
            }

            if (!dbContext.QuestionnaireSheets.Any())
            {
                var groups = dbContext.Groups.OrderBy(g => g.Id).ToList();

                var questionnaires = UnitTestSeedHelper.GetQuestionnaires(groups);

                dbContext.QuestionnaireSheets.AddRange(questionnaires);

                dbContext.SaveChanges();
            }

            if (!dbContext.Questions.Any())
            {
                var questionnaires = dbContext.QuestionnaireSheets.OrderBy(q => q.Id).ToList();

                var questions = UnitTestSeedHelper.GetQuestions(questionnaires);

                dbContext.Questions.AddRange(questions);

                dbContext.SaveChanges();
            }

            if (!dbContext.Answers.Any())
            {
                var questions = dbContext.Questions.OrderBy(q => q.Id).ToList();

                var answers = UnitTestSeedHelper.GetAnswers(questions);

                dbContext.Answers.AddRange(answers);

                dbContext.SaveChanges();
            }
        }

        public QuestionnaireDbContext CreateContext()
        {
            var storeOptions = new OperationalStoreOptions();

            IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

            return new QuestionnaireDbContext(
                new DbContextOptionsBuilder<QuestionnaireDbContext>()
                .UseSqlServer(ConnectionString).Options, operationalStoreOptions);
        }
    }
}
