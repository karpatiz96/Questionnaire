using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Exceptions;
using Questionnaire.Bll.Services;
using Questionnaire.Dll;
using Questionnaire.UnitTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questionnaire.UnitTest.UnitTests
{
    public class GroupServiceUnitTestInMemory
    {
        private readonly DbContextOptions<QuestionnaireDbContext> options;

        public GroupServiceUnitTestInMemory()
        {
            options = new DbContextOptionsBuilder<QuestionnaireDbContext>()
                .UseInMemoryDatabase("QuestionnaireUnitTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var storeOptions = new OperationalStoreOptions();

            IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

            using var context = new QuestionnaireDbContext(options, operationalStoreOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            UnitTestSeedHelper.InitializeDatabase(context);

            context.SaveChanges();
        }

        [Fact]
        public async Task Get_GroupDetails()
        {
            using (var context = CreateContext())
            {
                var groupService = new GroupService(context);

                var groupDetails = await groupService.GetGroup("123", 1);

                Assert.NotNull(groupDetails);
            }
        }

        [Fact]
        public async Task Get_GroupList()
        {
            using (var context = CreateContext())
            {
                var groupService = new GroupService(context);

                var groupList = await groupService.GetGroups("123");

                Assert.Equal(4, groupList.Count());
            }
        }

        [Fact]
        public async Task Create_Group()
        {
            using (var context = CreateContext())
            {
                var groupService = new GroupService(context);

                var group = await groupService.CreateGroup(new Bll.Dtos.GroupDto { Name = "Group5", Description = "Description5" }, "123");

                var result = context.Groups.Single(g => g.Name == "Group5");

                Assert.Equal("Description5", result.Description);
            }
        }

        [Fact]
        public async Task Update_Group()
        {
            using (var context = CreateContext())
            {
                var groupService = new GroupService(context);

                var old = context.Groups.Single(g => g.Name == "Group3");

                await groupService.UpdateGroup("123", new GroupDto { Id = old.Id, Name = "Group3", Description = "Description5" });

                var result = context.Groups.Single(g => g.Name == "Group3");

                Assert.Equal("Description5", result.Description);
            }
        }

        [Fact]
        public async Task Get_Members_Ok()
        {
            using (var context = CreateContext())
            {
                var groupService = new GroupService(context);

                var old = context.Groups.Single(g => g.Name == "Group1");

                var members = await groupService.GetGroupMembers("123", old.Id);

                Assert.NotNull(members);
            }
        }

        [Fact]
        public async Task Get_Members_NotAdmin()
        {
            using (var context = CreateContext())
            {
                var groupService = new GroupService(context);

                var old = context.Groups.Single(g => g.Name == "Group1");

                await Assert.ThrowsAsync<UserNotAdminException>(() => groupService.GetGroupMembers("124", old.Id));
            }
        }

        QuestionnaireDbContext CreateContext()
        {
            var storeOptions = new OperationalStoreOptions();

            IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

            return new QuestionnaireDbContext(options, operationalStoreOptions);
        }
    }
}
