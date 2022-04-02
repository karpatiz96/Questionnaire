using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.Exceptions;
using Questionnaire.Bll.Services;
using Questionnaire.UnitTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questionnaire.UnitTest.UnitTests
{
    public class GroupServiceUnitTest: IClassFixture<TestQuestionnaireDatabaseFixture>
    {
        public GroupServiceUnitTest(TestQuestionnaireDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestQuestionnaireDatabaseFixture Fixture { get; }

        [Fact]
        public async Task Get_GroupDetails()
        {
            using(var context = Fixture.CreateContext())
            {
                var groupService = new GroupService(context);

                var groupDetails = await groupService.GetGroup("123", 1);

                Assert.NotNull(groupDetails);
            }
        }

        [Fact]
        public async Task Get_GroupList()
        {
            using(var context = Fixture.CreateContext())
            {
                var groupService = new GroupService(context);

                var groupList = await groupService.GetGroups("123");

                Assert.Equal(4, groupList.Count());
            }
        }

        [Fact]
        public async Task Create_Group()
        {
            using(var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var groupService = new GroupService(context);

                var group = await groupService.CreateGroup(new Bll.Dtos.GroupDto { Name = "Group5", Description = "Description5" }, "123");

                context.ChangeTracker.Clear();
                var result = context.Groups.Single(g => g.Name == "Group5");

                Assert.Equal("Description5", result.Description);
            }
        }

        [Fact]
        public async Task Update_Group()
        {
            using (var context = Fixture.CreateContext())
            {

                context.Database.BeginTransaction();
                var groupService = new GroupService(context);

                var old = context.Groups.Single(g => g.Name == "Group3");

                await groupService.UpdateGroup("123", new GroupDto { Id = old.Id, Name = "Group3", Description = "Description5" });

                context.ChangeTracker.Clear();
                var result = context.Groups.Single(g => g.Name == "Group3");

                Assert.Equal("Description5", result.Description);
            }
        }

        [Fact]
        public async Task Get_Members_Ok()
        {
            using (var context = Fixture.CreateContext())
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
            using (var context = Fixture.CreateContext())
            {
                var groupService = new GroupService(context);

                var old = context.Groups.Single(g => g.Name == "Group1");

                await Assert.ThrowsAsync<UserNotAdminException>(() => groupService.GetGroupMembers("124", old.Id));
            }
        }
    }
}
