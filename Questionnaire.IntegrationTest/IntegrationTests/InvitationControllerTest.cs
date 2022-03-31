using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Questionnaire.Bll.Dtos;
using Questionnaire.IntegrationTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var provider = TestClaimProvider.WithUserClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/invitation");
            var content = await response.Content.ReadAsStringAsync();
            var invitations = JsonConvert.DeserializeObject<List<InvitationDto>>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, invitations.Count);
        }

        [Fact]
        public async Task Post_Invitation_UserNotExist()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var invitationDto = new InvitationNewDto { Email = "non@test.com", GroupId = 1 };
            var message = new StringContent(JsonConvert.SerializeObject(invitationDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/invitation", message);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_Invitation_NotMember()
        {
            var provider = TestClaimProvider.WithUserClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var invitationDto = new InvitationNewDto { Email = "test@test.com", GroupId = 1 };
            var message = new StringContent(JsonConvert.SerializeObject(invitationDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/invitation", message);

            //Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Post_Invitation_Ok()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var invitationDto = new InvitationNewDto { Email = "user@test.com", GroupId = 4 };
            var message = new StringContent(JsonConvert.SerializeObject(invitationDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/invitation", message);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Accept_Invitation_Ok()
        {
            var provider = TestClaimProvider.WithUserClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/invitation/accept/1");
            var content = await response.Content.ReadAsStringAsync();
            var invitation = JsonConvert.DeserializeObject<InvitationDto>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Dll.Entities.Invitation.InvitationStatus.Accepted, invitation.Status);
        }
    }
}
