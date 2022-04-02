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
using static Questionnaire.Dll.Entities.Invitation;

namespace Questionnaire.UnitTest.UnitTests
{
    public class InvitationServiceUnitTest: IClassFixture<TestQuestionnaireDatabaseFixture>
    {
        public InvitationServiceUnitTest(TestQuestionnaireDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestQuestionnaireDatabaseFixture Fixture { get; }
    
        [Fact]
        public async Task Get_Invitations()
        {
            using(var context = Fixture.CreateContext())
            {
                var invitationService = new InvitationService(context);

                var invitations = await invitationService.GetInvitations("124");

                Assert.Equal(2, invitations.Count());
            }
        }

        [Fact]
        public async Task Create_Invitation()
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var invitationService = new InvitationService(context);

                var userId = "123";
                var invitationDto = new InvitationNewDto { GroupId = 4, Email = "user@test,com" };
                var user = context.Users.Single(a => a.Id == "124");
                var result = await invitationService.CreateInvitation(userId, invitationDto, user);

                context.ChangeTracker.Clear();

                var invitation = context.Invitations
                    .SingleOrDefault(i => i.UserId == "124" && i.GroupId == invitationDto.GroupId && i.Status == InvitationStatus.Undecided);

                Assert.NotNull(invitation);
            }
        }

        [Fact]
        public async Task Delete_Invitation_Ok()
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var invitationService = new InvitationService(context);

                var userId = "123";
                var invitationId = 1;
                var old = await invitationService.DeleteInvitation(userId, invitationId);
                context.ChangeTracker.Clear();

                var invitation = context.Invitations.SingleOrDefault(i => i.Id == invitationId);

                Assert.NotNull(old);
                Assert.Null(invitation);
            }
        }

        [Fact]
        public async Task Decline_Invitation_Ok()
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var invitationService = new InvitationService(context);

                var userId = "124";
                var invitationId = 1;
                var status = InvitationStatus.Declined;
                var result = await invitationService.AcceptInvitation(userId, invitationId, status);

                context.ChangeTracker.Clear();

                Assert.NotNull(result);
                Assert.Equal(invitationId, result.Id);
                Assert.Equal(InvitationStatus.Declined, result.Status);
            }
        }

        [Fact]
        public async Task Accept_Invitation_Ok()
        {
            using (var context = Fixture.CreateContext())
            {
                context.Database.BeginTransaction();
                var invitationService = new InvitationService(context);

                var userId = "124";
                var invitationId = 1;
                var status = InvitationStatus.Accepted;
                var result = await invitationService.AcceptInvitation(userId, invitationId, status);

                context.ChangeTracker.Clear();

                Assert.NotNull(result);
                Assert.Equal(invitationId, result.Id);
                Assert.Equal(InvitationStatus.Accepted, result.Status);
            }
        }

        [Fact]
        public async Task Accept_Invitation_Validation_Exception()
        {
            using (var context = Fixture.CreateContext())
            {
                var invitationService = new InvitationService(context);

                var userId = "123";
                var invitationId = 1;
                var status = InvitationStatus.Accepted;
                await Assert.ThrowsAsync<InvitationValidationException>(() => invitationService.AcceptInvitation(userId, invitationId, status));
            }
        }
    }
}
