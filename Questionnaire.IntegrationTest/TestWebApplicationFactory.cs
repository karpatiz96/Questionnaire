using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Questionnaire.Dll;
using Questionnaire.IntegrationTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.IntegrationTest
{
    public class TestWebApplicationFactory<TStartup>: 
        WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var development = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<QuestionnaireDbContext>));

                services.Remove(development);

                services.AddDbContext<QuestionnaireDbContext>((options, context) 
                    => context.UseSqlServer(""));

                var serviceProvider = services.BuildServiceProvider();
                using(var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<QuestionnaireDbContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    try
                    {
                        await TestSeedHelper.InitializeDbForTesting(db);
                    } 
                    catch (Exception e)
                    {
                        Console.Write(e.StackTrace);
                    }
                }

            });
        }
    }
}
