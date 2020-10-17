using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShortVideo.API.Controllers;
using ShortVideo.API.Data;
using ShortVideo.API.Data.Authentication;
using ShortVideo.API.Data.DbModels;
using System;
using System.Linq;

namespace ShortVideo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            using (var scope = builder.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    
                    context.Database.Migrate();

                    if (!context.Users.Any())
                    {
                        var auth = services.GetRequiredService<IAuthRepository>();
                        var user = auth.Register(new User() { Username = "narn", Authority = Authority.Admin }, "narn@537");
                        //context.Users.Add(new Data.DbModels.User() { Authority = Data.DbModels.Authority.User,  })
                    }
                    //var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    //Seed.SeedData(context, userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occur in migration.");
                }
            }
            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}