using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.UnitTest.Helpers
{
    public class UnitTestSeedHelper
    {
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
                },
                new User
                {
                    Id = "124",
                    UserName = "user@test.com",
                    Email = "user@test.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    NormalizedEmail = "USER@TEST.COM",
                    NormalizedUserName = "USER@TEST.COM",
                    LockoutEnabled = false
                }
            };
        }

        public static List<Group> GetGroups()
        {
            return new List<Group>
            {
                new Group { Created = DateTime.UtcNow, Name = "Group1", Description = "Description1" },
                new Group { Created = DateTime.UtcNow, Name = "Group2", Description = "Description2" },
                new Group { Created = DateTime.UtcNow, Name = "Group3", Description = "Description3" }
            };
        }

        public static List<UserGroup> GetUserGroups(List<Group> groups)
        {
            return new List<UserGroup>
            {
                new UserGroup { UserId = "123", GroupId = groups[0].Id, MainAdmin = true, Role = "Admin", IsDeleted = false },
                new UserGroup { UserId = "123", GroupId = groups[1].Id, MainAdmin = false, Role = "User", IsDeleted = false },
                new UserGroup { UserId = "124", GroupId = groups[1].Id, MainAdmin = true, Role = "Admin", IsDeleted = false },
                new UserGroup { UserId = "123", GroupId = groups[2].Id, MainAdmin = true, Role = "Admin", IsDeleted = false }
            };
        }

        public static List<QuestionnaireSheet> GetQuestionnaires(List<Group> groups)
        {
            var yesterday = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
            var tommorrow = DateTime.UtcNow.AddDays(1);

            return new List<QuestionnaireSheet>
            {
                new QuestionnaireSheet { GroupId = groups[0].Id, Created = DateTime.UtcNow, Name = "Questionnaire1", Description = "Description1",
                    Begining = yesterday, Finish = tommorrow, LastEdited = DateTime.UtcNow, LimitedTime = false, TimeLimit = 1, RandomQuestionOrder = false, VisibleToGroup = true
                },
                new QuestionnaireSheet { GroupId = groups[1].Id, Created = DateTime.UtcNow, Name = "Questionnaire2", Description = "Description2",
                    Begining = yesterday, Finish = tommorrow, LastEdited = DateTime.UtcNow, LimitedTime = false, TimeLimit = 1, RandomQuestionOrder = false, VisibleToGroup = true
                },
            };
        }

        public static List<Question> GetQuestions(List<QuestionnaireSheet> questionnaireSheets)
        {
            return new List<Question>
            {
                new Question { Name = "Question1", Description = "Description1", MaximumPoints = 5, Number = 1, QuestionnaireSheetId = questionnaireSheets[0].Id, SuggestedTime = 5, Type = Question.QuestionType.TrueOrFalse },
                new Question { Name = "Question2", Description = "Description2", MaximumPoints = 5, Number = 1, QuestionnaireSheetId = questionnaireSheets[1].Id, SuggestedTime = 5, Type = Question.QuestionType.TrueOrFalse },
            };
        }

        public static List<Answer> GetAnswers(List<Question> questions)
        {
            return new List<Answer>
            {
                new Answer { Name = "True", Type = true, TrueOrFalse = true, Points = 5, QuestionId = questions[0].Id, UserAnswer = "" },
                new Answer { Name = "False", Type = false, TrueOrFalse = false, Points = 0, QuestionId = questions[0].Id, UserAnswer = "" },
                new Answer { Name = "True", Type = true, TrueOrFalse = true, Points = 5, QuestionId = questions[1].Id, UserAnswer = "" },
                new Answer { Name = "False", Type = false, TrueOrFalse = false, Points = 0, QuestionId = questions[1].Id, UserAnswer = "" },
            };
        }
    }
}
