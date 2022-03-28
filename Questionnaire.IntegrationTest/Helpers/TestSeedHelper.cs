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

            var groups = GetGroups();

            dbContext.SaveChanges();

            dbContext.Groups.AddRange(groups);

            var userGroups = GetUserGroups();

            dbContext.UserGroups.AddRange(userGroups);

            dbContext.SaveChanges();

            var questionnaires = GetQuestionnaires();

            dbContext.QuestionnaireSheets.AddRange(questionnaires);

            dbContext.SaveChanges();

            var questions = GetQuestions();

            dbContext.Questions.AddRange(questions);

            dbContext.SaveChanges();

            var answers = GetAnswers();

            dbContext.Answers.AddRange(answers);

            dbContext.SaveChanges();
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
                new Group { Id = 1, Created = DateTime.UtcNow, Name = "Group1", Description = "Group1 Description" },
                new Group { Id = 2, Created = DateTime.UtcNow, Name = "Group2", Description = "Group2 Description" },
            };
        }

        public static List<UserGroup> GetUserGroups()
        {
            return new List<UserGroup> 
            {
                new UserGroup { Id = 1, UserId = "123", GroupId = 1, MainAdmin = true, Role = "Admin", IsDeleted = false },
                new UserGroup { Id = 2, UserId = "124", GroupId = 1, MainAdmin = false, Role = "User", IsDeleted = false },
                new UserGroup { Id = 3, UserId = "123", GroupId = 2, MainAdmin = false, Role = "User", IsDeleted = false },
                new UserGroup { Id = 4, UserId = "124", GroupId = 2, MainAdmin = true, Role = "Admin", IsDeleted = false }
            };
        }

        public static List<QuestionnaireSheet> GetQuestionnaires()
        {
            var yesterday = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
            var tommorrow = DateTime.UtcNow.AddDays(1);

            return new List<QuestionnaireSheet>
            {
                new QuestionnaireSheet { Id = 1, GroupId = 1, Created = DateTime.UtcNow, Name = "Questionnaire1", Description = "Questionnaire1 Description", 
                    Begining = yesterday, Finish = tommorrow, LastEdited = DateTime.UtcNow, LimitedTime = false, TimeLimit = 1, RandomQuestionOrder = false, VisibleToGroup = true
                },
                new QuestionnaireSheet { Id = 2, GroupId = 2, Created = DateTime.UtcNow, Name = "Questionnaire2", Description = "Questionnaire2 Description",
                    Begining = yesterday, Finish = tommorrow, LastEdited = DateTime.UtcNow, LimitedTime = false, TimeLimit = 1, RandomQuestionOrder = false, VisibleToGroup = true
                },
            };
        }

        public static List<Question> GetQuestions()
        {
            return new List<Question> 
            {
                new Question { Id = 1, Name = "Question1", Description = "Question1 Description", MaximumPoints = 5, Number = 1, QuestionnaireSheetId = 1, SuggestedTime = 5, Type = Question.QuestionType.TrueOrFalse },
                new Question { Id = 2, Name = "Question2", Description = "Question2 Description", MaximumPoints = 5, Number = 1, QuestionnaireSheetId = 2, SuggestedTime = 5, Type = Question.QuestionType.TrueOrFalse },
            };
        }

        public static List<Answer> GetAnswers()
        {
            return new List<Answer> 
            {
                new Answer { Id = 1, Name = "True", Type = true, TrueOrFalse = true, Points = 5, QuestionId = 1, UserAnswer = "" },
                new Answer { Id = 2, Name = "False", Type = false, TrueOrFalse = false, Points = 0, QuestionId = 1, UserAnswer = "" },
                new Answer { Id = 3, Name = "True", Type = true, TrueOrFalse = true, Points = 5, QuestionId = 2, UserAnswer = "" },
                new Answer { Id = 4, Name = "False", Type = false, TrueOrFalse = false, Points = 0, QuestionId = 2, UserAnswer = "" },
            };
        }
    }
}
