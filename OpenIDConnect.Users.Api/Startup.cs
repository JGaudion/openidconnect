using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIDConnect.Users.Domain;
using OpenIDConnect.Users.Data.AspNetIdentity.Repositories;
using Newtonsoft.Json.Serialization;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenIDConnect.Users.Data.AspNetIdentity.Models;
using System;
using OpenIDConnect.Users.Domain.Repositories;

namespace OpenIDConnect.Users.Api
{
    public class Startup
    {
        private readonly UsersApiBootstrap bootstrap;

        public Startup(IHostingEnvironment env)
        {
            this.bootstrap = new UsersApiBootstrap();
            this.Configuration = this.bootstrap.GetConfiguration();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.bootstrap.ConfigureServices(services, this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.bootstrap.Configure(app, env, loggerFactory, this.Configuration);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    public class UsersApiBootstrap
    {
        private IConfigurationRoot configuration;

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    using (var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        db.Database.EnsureCreated();
                        db.Database.Migrate();
                    }
                }
            }
            catch (Exception exception)
            {
            }

            app.UseCors("AllowAllOrigins");         // TODO: allow collection of allowed origins per client
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddEntityFramework()
              .AddSqlServer()
              .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });

            // Add CORS support
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddScoped<IUsersRepository, AspNetIdentityUsersRepository>();
            services.AddScoped<IUserClaimsRepository, AspNetIdentityUserClaimsRepository>();
        }

        public IConfigurationRoot GetConfiguration()
        {
            if (this.configuration == null)
            {
                // Set up configuration sources.
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
                this.configuration = builder.Build();
            }

            return this.configuration;
        }
    }
}