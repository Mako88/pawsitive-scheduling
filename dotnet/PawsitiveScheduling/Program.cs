using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PawsitiveScheduling.API;
using PawsitiveScheduling.Initialization;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.DI;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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
            var builder = WebApplication.CreateBuilder(args);

            ConfigureBuilder(builder);

            var app = builder.Build();

            await Initialize(app);

            SetupCustomMiddleware(app);

            app.UseHttpLogging();

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

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpLogging(x =>
            {
                x.LoggingFields = HttpLoggingFields.RequestHeaders |
                                  HttpLoggingFields.RequestBody |
                                  HttpLoggingFields.ResponseHeaders |
                                  HttpLoggingFields.ResponseBody;
            });

            builder.Host.UseSerilog();
        }

        /// <summary>
        /// Method for performing global initialization tasks
        /// </summary>
        private static async Task Initialize(WebApplication app)
        {
            IoC.Initialize(app);

            DotNetEnv.Env.TraversePath().Load();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            using (var scope = IoC.BeginLifetimeScope())
            {
                var initializers = scope.Resolve<IEnumerable<IInitializer>>();
                var log = scope.Resolve<ILog>();

                foreach (var initializer in initializers)
                {
                    try
                    {
                        await initializer.Initialize();
                    }
                    catch (Exception ex)
                    {
                        log.Error($"An error occurred during initialization: {ex.Message}", ex);
                    }
                }

                Log.Logger = scope.Resolve<ILogger>();
            }
        }

        /// <summary>
        /// Perform setup of custom middleware
        /// </summary>
        private static void SetupCustomMiddleware(WebApplication app)
        {
            // Error handling
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                    var exceptionInfo = context.Features.Get<IExceptionHandlerPathFeature>();

                    var body = new
                    {
                        Error = exceptionInfo.Error.Message,
                        Stack = exceptionInfo.Error.StackTrace,
                    };

                    var json = JsonConvert.SerializeObject(body);

                    await context.Response.WriteAsync(json);
                });
            });
        }

        /// <summary>
        /// Map endpoints to handlers
        /// </summary>
        private static void MapEndpoints(WebApplication app)
        {
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
