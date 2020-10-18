using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ShortVideo.API.Data;
using ShortVideo.API.Data.Authentication;
using ShortVideo.API.Services;
using System;
using System.Text;

namespace ShortVideo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(context => context.UseSqlite(Configuration.GetConnectionString("defaultConnection")));

            services.AddHttpClient()
                .AddControllersWithViews()
                .AddNewtonsoftJson();
            services.AddSwaggerGen();

            services.AddScoped<IGoogleDriveService, GoogleDriveService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddMemoryCache();

            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSetting:Token").Value)),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });

            // In production, the React files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/build";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Console.WriteLine(env.EnvironmentName);

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Short Video");
            });

            app.UseCors(a => a.AllowAnyOrigin()
          .AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller}/{action=Index}/{id?}");
            //});

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});
        }
    }
}