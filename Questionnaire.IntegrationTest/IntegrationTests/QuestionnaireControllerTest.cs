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
    public class QuestionnaireControllerTest : IClassFixture<TestWebApplicationFactory<Questionnaire.Api.Startup>>
    {
        private readonly TestWebApplicationFactory<Questionnaire.Api.Startup> _factory;

        public QuestionnaireControllerTest(TestWebApplicationFactory<Questionnaire.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_QuestionnaireDetails_NotAdmin()
        {
            var provider = TestClaimProvider.WithUserClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/questionnaire/1");

            //Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Get_QuestionnaireDetails_Ok()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var response = await client.GetAsync("/api/questionnaire/1");
            var content = await response.Content.ReadAsStringAsync();
            var questionnaireDetails = JsonConvert.DeserializeObject<QuestionnaireDetailsDto>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Questionnaire1", questionnaireDetails.Title);
        }

        [Fact]
        public async Task Post_Questionnaire_NotAdmin()
        {
            var provider = TestClaimProvider.WithUserClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var questionnaireDto = new QuestionnaireDto { Title = "Questionnaire3", GroupId = 1, Description = "Description3", Begining = DateTime.UtcNow, Finish = DateTime.UtcNow, Limited = false, RandomQuestionOrder = false, TimeLimit = 1, VisibleToGroup = false }; 
            var message = new StringContent(JsonConvert.SerializeObject(questionnaireDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/questionnaire", message);

            //Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Post_Questionnaire_Ok()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var questionnaireDto = new QuestionnaireDto { Title = "Questionnaire3", GroupId = 1, Description = "Description3", Begining = DateTime.UtcNow.AddHours(1), Finish = DateTime.UtcNow.AddDays(1), Limited = false, RandomQuestionOrder = false, TimeLimit = 1, VisibleToGroup = false };
            var message = new StringContent(JsonConvert.SerializeObject(questionnaireDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/questionnaire", message);

            var content = await response.Content.ReadAsStringAsync();
            var questionnaire = JsonConvert.DeserializeObject<QuestionnaireDto>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Questionnaire3", questionnaire.Title);
        }

        [Fact]
        public async Task Copy_Questionnaire_Ok()
        {
            var provider = TestClaimProvider.WithAdminClaim();
            var client = _factory.WithAuthentication<Questionnaire.Api.Startup>(provider)
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            //Act
            var questionnaireId = 1;
            var message = new StringContent(JsonConvert.SerializeObject(questionnaireId), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/questionnaire/copy", message);

            var content = await response.Content.ReadAsStringAsync();
            var questionnaire = JsonConvert.DeserializeObject<QuestionnaireDto>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Questionnaire1-copy", questionnaire.Title);
        }
    }
}
