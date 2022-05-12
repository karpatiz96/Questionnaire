using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.IntegrationTest
{
    public class TestClaimProvider
    {
        public IList<Claim> Claims { get; set; }

        public TestClaimProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimProvider WithAdminClaim()
        {
            var provider = new TestClaimProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "123"));
            return provider;
        }

        public static TestClaimProvider WithUserClaim()
        {
            var provider = new TestClaimProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "124"));
            return provider;
        }
    }
}
