using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questionnaire.IntegrationTest.IntegrationTests
{
    public class InvitationControllerTest: IClassFixture<TestWebApplicationFactory<Questionnaire.Api.Startup>>
    {
        private readonly TestWebApplicationFactory<Questionnaire.Api.Startup> _factory;

        public InvitationControllerTest(TestWebApplicationFactory<Questionnaire.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Invitations()
        {

        }

        [Fact]
        public async Task Post_Invitation_UserNotExist()
        {

        }

        [Fact]
        public async Task Post_Invitation_NotMember()
        {

        }

        [Fact]
        public async Task Post_Invitation_Ok()
        {

        }

        [Fact]
        public async Task Accept_Invitation_Ok()
        {

        }
    }
}
