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
    public class QuestionnaireServiceUnitTest: IClassFixture<TestQuestionnaireDatabaseFixture>
    {
        public QuestionnaireServiceUnitTest(TestQuestionnaireDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestQuestionnaireDatabaseFixture Fixture { get; }
    
        [Fact]
        public async Task Get_Questionnaire_Details()
        {
            using(var context = Fixture.CreateContext())
            {
                var questionnaireService = new QuestionnaireService(context);

                var questionnaireId = 1;
                var questionnaireDetails = await questionnaireService.GetQuestionnaire(questionnaireId);

                Assert.NotNull(questionnaireDetails);
                Assert.Equal("Questionnaire1", questionnaireDetails.Title);
            }
        }

        [Theory]
        [InlineData("124", 1)]
        public async Task Create_Questionnaire_NotAdmin(string userId, int groupId)
        {
            using(var context = Fixture.CreateContext())
            {
                var questionnaireService = new QuestionnaireService(context);

                var questionnaireDto = new QuestionnaireDto { 
                    GroupId = groupId,
                    Begining = DateTime.UtcNow.AddHours(1),
                    Finish = DateTime.UtcNow.AddHours(2),
                    Title = "Questionnaire3",
                    Description = "Description3",
                    Limited = false,
                    TimeLimit = 1,
                    RandomQuestionOrder = false,
                    VisibleToGroup = false
                };

                await Assert.ThrowsAsync<UserNotAdminException>(() => questionnaireService.CreateQuestionnaire(userId, questionnaireDto));
            }
        }

        [Theory]
        [InlineData("123", 1)]
        public async Task Create_Questionnaire_Invalid(string userId, int groupId)
        {
            using (var context = Fixture.CreateContext())
            {
                var questionnaireService = new QuestionnaireService(context);

                var questionnaireDto = new QuestionnaireDto
                {
                    GroupId = groupId,
                    Begining = DateTime.UtcNow.AddHours(3),
                    Finish = DateTime.UtcNow.AddHours(1),
                    Title = "Questionnaire3",
                    Description = "Description3",
                    Limited = false,
                    TimeLimit = 1,
                    RandomQuestionOrder = false,
                    VisibleToGroup = false
                };

                await Assert.ThrowsAsync<QuestionnaireValidationException>(() => questionnaireService.CreateQuestionnaire(userId, questionnaireDto));
            }
        }

        [Theory]
        [InlineData("123", 1, "Questionnaire3")]
        public async Task Create_Questionnaire_Ok(string userId, int groupId, string title)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionnaireService = new QuestionnaireService(context);

                var questionnaireDto = new QuestionnaireDto
                {
                    GroupId = groupId,
                    Begining = DateTime.UtcNow.AddHours(1),
                    Finish = DateTime.UtcNow.AddHours(2),
                    Title = title,
                    Description = "Description3",
                    Limited = false,
                    TimeLimit = 1,
                    RandomQuestionOrder = false,
                    VisibleToGroup = false
                };

                var questionnaire = await questionnaireService.CreateQuestionnaire(userId, questionnaireDto);

                context.ChangeTracker.Clear();
                var result = context.QuestionnaireSheets.Single(q => q.Name == questionnaire.Title);

                Assert.Equal(title, result.Name);
            }
        }

        [Theory]
        [InlineData("124", 1, 1)]
        public async Task Update_Questionnaire_NotAdmin(string userId, int questionnaireId, int groupId)
        {
            using (var context = Fixture.CreateContext())
            {
                var questionnaireService = new QuestionnaireService(context);
                
                var questionnaireDto = new QuestionnaireDto
                {
                    Id = questionnaireId,
                    GroupId = groupId,
                    Title = "Questionnaire1",
                    Description = "Description",
                    Begining = DateTime.UtcNow.AddHours(1),
                    Finish = DateTime.Today.AddHours(2),
                    Limited = false,
                    TimeLimit = 1,
                    RandomQuestionOrder = false,
                    VisibleToGroup = false

                };

                await Assert.ThrowsAsync<UserNotAdminException>(() => questionnaireService.UpdateQuestionnaire(userId, questionnaireDto));
            }
        }

        [Theory]
        [InlineData("123", 1, 1)]
        public async Task Update_Questionnaire_Ok(string userId, int questionnaireId, int groupId)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionnaireService = new QuestionnaireService(context);

                var questionnaireDto = new QuestionnaireDto
                {
                    Id = questionnaireId,
                    GroupId = groupId,
                    Title = "Questionnaire1",
                    Description = "Description",
                    Begining = DateTime.UtcNow.AddHours(1),
                    Finish = DateTime.UtcNow.AddHours(2),
                    Limited = false,
                    TimeLimit = 1,
                    RandomQuestionOrder = false,
                    VisibleToGroup = false

                };

                var questionnaire = await questionnaireService.UpdateQuestionnaire(userId, questionnaireDto);

                context.ChangeTracker.Clear();
                var result = context.QuestionnaireSheets.Single(q => q.Id == questionnaireId);

                Assert.Equal("Description", result.Description);
            }
        }
    }
}
