using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questionnaire.Dll.Entities;
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
        public async Task Get_GroupList()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/group");

            //var message = new StringContent(JsonConvert.SerializeObject(new Group { }), Encoding.UTF8, "application/json");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
