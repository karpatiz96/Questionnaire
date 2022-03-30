using System;
using System.Collections.Generic;
using System.Linq;
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

        }

        [Fact]
        public async Task Get_QuestionnaireDetails_Ok()
        {

        }

        [Fact]
        public async Task Post_Questionnaire_NotAdmin()
        {

        }

        [Fact]
        public async Task Post_Questionnaire_Ok()
        {

        }
    }
}
