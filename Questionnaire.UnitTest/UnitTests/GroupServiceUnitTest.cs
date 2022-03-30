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

        }

        [Fact]
        public async Task Get_GroupList()
        {
            using(var context = Fixture.CreateContext())
            {
                var groupService = new GroupService(context);

                var groupList = await groupService.GetGroups("123");

                Assert.Equal(2, groupList.Count());
            }
        }

        [Fact]
        public async Task Create_Group()
        {
            using(var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var groupService = new GroupService(context);

                var group = await groupService.CreateGroup(new Bll.Dtos.GroupDto { Name = "Group3", Description = "Description3" }, "123");

                context.ChangeTracker.Clear();
                var result = context.Groups.Single(g => g.Name == "Group3");

                Assert.Equal("Description3", result.Description);
            }
        }

        [Fact]
        public async Task Update_Group()
        {

        }

        [Fact]
        public async Task Get_Members_Ok()
        {

        }

        [Fact]
        public async Task Get_Members_NotAdmin()
        {

        }
    }
}
