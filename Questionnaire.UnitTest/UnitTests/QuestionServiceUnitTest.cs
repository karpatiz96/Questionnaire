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
    public class QuestionServiceUnitTest: IClassFixture<TestQuestionnaireDatabaseFixture>
    {
        public QuestionServiceUnitTest(TestQuestionnaireDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestQuestionnaireDatabaseFixture Fixture { get; }
    
        [Theory]
        [InlineData(1)]
        public async Task Get_Question_Details_OK(int questionId)
        {
            using(var context = Fixture.CreateContext())
            {
                var questionService = new QuestionService(context);
                var question = await questionService.GetQuestion(questionId);

                Assert.NotNull(question);
                Assert.Equal("Question1", question.Name);
            }
        }

        [Theory]
        [InlineData("124", 1, "Question3")]
        public async Task Create_Question_NotAdmin(string userId, int questionnaireId, string name)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionService = new QuestionService(context);

                var questionDto = new QuestionDto
                {
                    QuestionnaireId = questionnaireId,
                    Name = name,
                    Description = "Description3",
                    Number = 1,
                    SuggestedTime = 5,
                    Type = Dll.Entities.Question.QuestionType.FreeText,
                    Value = 5
                };

                await Assert.ThrowsAsync<UserNotAdminException>(() => questionService.CreateQuestion(userId, questionDto));
            }
        }

        [Theory]
        [InlineData("123", 1, "Question3")]
        public async Task Create_Question_OK(string userId, int questionnaireId, string name)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionService = new QuestionService(context);

                var questionDto = new QuestionDto
                {
                    QuestionnaireId = questionnaireId,
                    Name = name,
                    Description = "Description3",
                    Number = 1,
                    SuggestedTime = 5,
                    Type = Dll.Entities.Question.QuestionType.FreeText,
                    Value = 5
                };
                var question = await questionService.CreateQuestion(userId, questionDto);

                context.ChangeTracker.Clear();

                var result = context.Questions.Single(q => q.Name == name);

                Assert.NotNull(result);
                Assert.Equal(name, result.Name);
            }
        }

        [Theory]
        [InlineData("124", 1, 1)]
        public async Task Update_Question_NotAdmin(string userId, int questionId, int questionnaireId)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionService = new QuestionService(context);

                var questionDto = new QuestionDto
                {
                    Id = questionId,
                    QuestionnaireId = questionnaireId,
                    Name = "Question1",
                    Description = "Description",
                    Number = 1,
                    SuggestedTime = 5,
                    Type = Dll.Entities.Question.QuestionType.FreeText,
                    Value = 5
                };

                await Assert.ThrowsAsync<UserNotAdminException>(() => questionService.UpdateQuestion(userId, questionDto));
            }
        }

        [Theory]
        [InlineData("123", 1, 1)]
        public async Task Update_Question_OK(string userId, int questionId, int questionnaireId)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionService = new QuestionService(context);

                var questionDto = new QuestionDto
                {
                    Id = questionId,
                    QuestionnaireId = questionnaireId,
                    Name = "Question1",
                    Description = "Description",
                    Number = 1,
                    SuggestedTime = 5,
                    Type = Dll.Entities.Question.QuestionType.FreeText,
                    Value = 5
                };

                var question = await questionService.UpdateQuestion(userId, questionDto);

                context.ChangeTracker.Clear();

                var result = context.Questions.Single(q => q.Id == questionId);

                Assert.Equal("Description", result.Description);
            }
        }

        [Theory]
        [InlineData("124", 1)]
        public async Task Delete_Question_NotAdmin(string userId, int questionId)
        {
            using (var context = Fixture.CreateContext())
            {
                var questionService = new QuestionService(context);

                await Assert.ThrowsAsync<UserNotAdminException>(() => questionService.DeleteQuestion(userId, questionId));
            }
        }

        [Theory]
        [InlineData("123", 1)]
        public async Task Delete_Question_OK(string userId, int questionId)
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var questionService = new QuestionService(context);

                var question = await questionService.DeleteQuestion(userId, questionId);
                context.ChangeTracker.Clear();

                var result = context.Questions.SingleOrDefault(q => q.Id == questionId);
                Assert.Null(result);
            }
        }
    }
}
