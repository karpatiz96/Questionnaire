using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
            if (!dbContext.Users.Any())
            {
                var userStore = new UserStore<User>(dbContext);
                var passwordHasher = new PasswordHasher<User>();
                var users = GetTestUsers();
                foreach (var user in users)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, "Test123+");
                    await userStore.CreateAsync(user);
                }

                await dbContext.SaveChangesAsync();
            }

            if(!dbContext.Groups.Any())
            {
                var groups = GetGroups();

                dbContext.Groups.AddRange(groups);

                dbContext.SaveChanges();
            }

            if(!dbContext.UserGroups.Any())
            {
                var groups = dbContext.Groups.OrderBy(g => g.Id).ToList();

                var userGroups = GetUserGroups(groups);

                dbContext.UserGroups.AddRange(userGroups);

                dbContext.SaveChanges();
            }

            if(!dbContext.QuestionnaireSheets.Any())
            {
                var groups = dbContext.Groups.OrderBy(g => g.Id).ToList();

                var questionnaires = GetQuestionnaires(groups);

                dbContext.QuestionnaireSheets.AddRange(questionnaires);

                dbContext.SaveChanges();
            }

            if(!dbContext.Questions.Any())
            {
                var questionnaires = dbContext.QuestionnaireSheets.OrderBy(q => q.Id).ToList();

                var questions = GetQuestions(questionnaires);

                dbContext.Questions.AddRange(questions);

                dbContext.SaveChanges();
            }

            if(!dbContext.Answers.Any())
            {
                var questions = dbContext.Questions.OrderBy(q => q.Id).ToList();

                var answers = GetAnswers(questions);

                dbContext.Answers.AddRange(answers);

                dbContext.SaveChanges();
            }
        }

        public static async Task ReInitializeForTesting(QuestionnaireDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            await InitializeDbForTesting(dbContext);
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
                new Group { Created = DateTime.UtcNow, Name = "Group1", Description = "Group1 Description" },
                new Group { Created = DateTime.UtcNow, Name = "Group2", Description = "Group2 Description" },
            };
        }

        public static List<UserGroup> GetUserGroups(List<Group> groups)
        {
            return new List<UserGroup> 
            {
                new UserGroup { UserId = "123", GroupId = groups[0].Id, MainAdmin = true, Role = "Admin", IsDeleted = false },
                new UserGroup { UserId = "123", GroupId = groups[1].Id, MainAdmin = false, Role = "User", IsDeleted = false },
                new UserGroup { UserId = "124", GroupId = groups[1].Id, MainAdmin = true, Role = "Admin", IsDeleted = false }
            };
        }

        public static List<QuestionnaireSheet> GetQuestionnaires(List<Group> groups)
        {
            var yesterday = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
            var tommorrow = DateTime.UtcNow.AddDays(1);

            return new List<QuestionnaireSheet>
            {
                new QuestionnaireSheet { GroupId = groups[0].Id, Created = DateTime.UtcNow, Name = "Questionnaire1", Description = "Questionnaire1 Description", 
                    Begining = yesterday, Finish = tommorrow, LastEdited = DateTime.UtcNow, LimitedTime = false, TimeLimit = 1, RandomQuestionOrder = false, VisibleToGroup = true
                },
                new QuestionnaireSheet { GroupId = groups[1].Id, Created = DateTime.UtcNow, Name = "Questionnaire2", Description = "Questionnaire2 Description",
                    Begining = yesterday, Finish = tommorrow, LastEdited = DateTime.UtcNow, LimitedTime = false, TimeLimit = 1, RandomQuestionOrder = false, VisibleToGroup = true
                },
            };
        }

        public static List<Question> GetQuestions(List<QuestionnaireSheet> questionnaireSheets)
        {
            return new List<Question> 
            {
                new Question { Name = "Question1", Description = "Question1 Description", MaximumPoints = 5, Number = 1, QuestionnaireSheetId = questionnaireSheets[0].Id, SuggestedTime = 5, Type = Question.QuestionType.TrueOrFalse },
                new Question { Name = "Question2", Description = "Question2 Description", MaximumPoints = 5, Number = 1, QuestionnaireSheetId = questionnaireSheets[1].Id, SuggestedTime = 5, Type = Question.QuestionType.TrueOrFalse },
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
