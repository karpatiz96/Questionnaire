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
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=QuestionnaireUnitTest;Trusted_Connection=True;MultipleActiveResultSets=true";

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

                        UnitTestSeedHelper.InitializeDatabase(context);
                    }

                    Initialized = true;
                }
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
