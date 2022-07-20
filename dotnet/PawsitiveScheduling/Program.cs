using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PawsitiveScheduling.API;
using PawsitiveScheduling.Utility.DI;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PawsitiveScheduling
{
    /// <summary>
    /// Application entry point class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();

            var builder = WebApplication.CreateBuilder(args);

            ConfigureBuilder(builder);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("cors");

            app.UseAuthentication();
            app.UseAuthorization();

            MapEndpoints(app);

            app.Urls.Add("https://localhost:5001");
            app.Urls.Add("http://localhost:5000");

            await app.RunAsync();
        }

        /// <summary>
        /// Configure the builder
        /// </summary>
        private static void ConfigureBuilder(WebApplicationBuilder builder)
        {
            // IoC
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(x =>
            {
                x.RegisterModule(new AutofacModule());
            });

            builder.Services.AddCors(options => options.AddPolicy("cors", builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()));

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = EnvironmentVariables.JwtIssuer,
                    ValidAudience = EnvironmentVariables.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtKey)),
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int) HttpStatusCode.TemporaryRedirect;
                options.HttpsPort = 5001;
            });
        }

        /// <summary>
        /// Map endpoints to handlers
        /// </summary>
        private static void MapEndpoints(WebApplication app)
        {
            // We can resolve handlers in a LifetimeScope because
            // they're singleton so they live on the root, and won't be disposed
            using (var scope = app.Services.GetAutofacRoot().BeginLifetimeScope())
            {
                var handlers = scope.Resolve<IEnumerable<IHandler>>();

                foreach (var handler in handlers)
                {
                    handler.MapEndpoint(app);
                }
            }
        }
    }
}
