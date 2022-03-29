using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questionnaire.Bll.Dtos;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
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
    public class GroupControllerTest: IClassFixture<TestWebApplicationFactory<Questionnaire.Api.Startup>>
    {
        private readonly TestWebApplicationFactory<Questionnaire.Api.Startup> _factory;

        public GroupControllerTest(TestWebApplicationFactory<Questionnaire.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_GroupDetails()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/group/1");

            var content = await response.Content.ReadAsStringAsync();
            var group = JsonConvert.DeserializeObject<GroupDetailsDto>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, group.Id);
        }

        [Fact]
        public async Task Get_GroupDetails_Forbidden()
        {
            var provider = TestClaimProvider.WithUserClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/group/1");

            //Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Get_GroupList()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/group");
            var content = await response.Content.ReadAsStringAsync();
            var groups = JsonConvert.DeserializeObject<List<GroupHeaderDto>>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, groups.Count);
        }

        [Fact]
        public async Task Post_Group()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            var groupDto = new GroupDto { Name = "Group3", Description = "Group3 Description" };
            var message = new StringContent(JsonConvert.SerializeObject(groupDto), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/group", message);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
