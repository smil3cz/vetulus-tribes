using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Helpers;
using GreenFoxAcademy.SpaceSettlers.Logging;
using GreenFoxAcademy.SpaceSettlers.Services;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using GreenFoxAcademy.SpaceSettlers.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GreenFoxAcademy.SpaceSettlers
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBuildingService, BuildingService>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<IRestrictionsService, RestrictionsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IShipService, ShipService>();
            services.AddTransient<ITimeService, TimeService>();
            services.AddTransient<ILogsService, LogsService>();
            services.AddSingleton<PriceCollection>();
            services.AddTransient<IKingdomService, KingdomService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<ILeaderboardService, LeaderboardService>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Server",
                    ValidAudience = "Server",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my secret"))
                };
            });

            ConfigureDatabase(services);

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddSwaggerDocumentation();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                app.UseExceptionHandler("/");
            }

            app.UseSwaggerDocumentation();
            app.UseLoggingMiddleware();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<TokenMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql("User ID=zvbwibuysceflh;Password=a3c8288211740061dec2c76675eae1527f2fcbe018cdaeb9033d9932937fce93;Host=ec2-54-246-87-132.eu-west-1.compute.amazonaws.com;Port=5432;Database=d8o6tq9uqfpeb2;Pooling=true;SSL Mode=Require;TrustServerCertificate=True;"));
        }
    }
}
