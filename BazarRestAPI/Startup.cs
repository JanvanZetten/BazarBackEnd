using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using infrastructure;
using infrastructure.Development;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace BazarRestAPI
{
    public class Startup
    {
        public IConfiguration _conf { get; }
        public IHostingEnvironment _env { get; }

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _conf = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // Scopes all Services and Repositories
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepository<Booth>, BoothRepository>();
            services.AddScoped<IBoothRepository, BoothRepository>();
            services.AddScoped<IBoothService, BoothService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRepository<WaitingListItem>, WaitingListItemRepository>();
            services.AddScoped<IWaitingListRepository, WaitingListItemRepository>();
            services.AddScoped<IImageURLRepository, ImageURLRepository>();
            services.AddScoped<IImageURLService, ImageURLService>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IResetRepository, ResetRepository>();
            services.AddScoped<IResetService, ResetService>();


            // Creates a random array of bytes for use of passwords.
            Byte[] secretBytes = new byte[40];
            Random rand = new Random();
            rand.NextBytes(secretBytes);

            TokenValidationParameters validationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                        ValidateLifetime = true, //validate the expiration and not before values in the token
                        ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                    };

            // Add secret bytes for encryption and validationParameters to validate tokens.
            services.AddSingleton<IAuthenticationService>(new AuthenticationService(secretBytes, validationParameters));

            // Add JWT based authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = validationParameters;
            });

            if (_env.IsDevelopment())
            {
                // In-memory database:
                services.AddDbContext<BazarContext>(opt => opt.UseSqlite("Data Source = BazarLocalDB.db").EnableSensitiveDataLogging());
            }
            else
            {
                // SQL Server on Azure:
                services.AddDbContext<BazarContext>(opt => opt.UseSqlServer(_conf.GetConnectionString("defaultConnection")));
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Runs when the build is in development (IIS Express)
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<BazarContext>();
                    // Deletes and creates a new database with mock data.
                    DatabaseInitialize.Initialize(ctx);
                }
                app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            }
            // Runs when the build is in production (Azure)
            else
            {
                app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<BazarContext>();
                    ctx.Database.EnsureCreated();
                }
                app.UseHsts();
                app.UseCors(builder => builder.WithOrigins("https://hoejerbazar.firebaseapp.com").AllowAnyMethod().AllowAnyHeader());
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
