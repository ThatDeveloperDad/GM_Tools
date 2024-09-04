using GameTools.API.WorkloadProvider;
using GameTools.BlazorClient.Components;
using GameTools.BlazorClient.Middleware;
using GameTools.BlazorClient.Services;
using GameTools.DiceEngine;
using GameTools.NPCAccess.SqlServer;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.RulesetAccess;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using GameTools.UserAccess.MsGraphProvider;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using ThatDeveloperDad.AIWorkloadManager;
using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.LlmAccess;

namespace GameTools.BlazorClient
{
	public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder = SetupLogging(builder);

            var startupLog = CreateStartupLogger(builder);
            startupLog.LogInformation("Startup Log has been created.  Let's see what's going on.");

            
            builder = SetUpConfiguration(builder, startupLog);

            // Add the local, custom services that are use-case relevant
            builder = CreateServices(builder, startupLog);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder = ConfigureAppSec(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
			app.MapControllers();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }

        private static WebApplicationBuilder SetupLogging(WebApplicationBuilder builder)
        {
            builder.Logging.AddConsole();
            builder.Logging.AddAzureWebAppDiagnostics();

            return builder;
        }

        private static ILogger<Program> CreateStartupLogger(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            return logger;
        }

        private static WebApplicationBuilder SetUpConfiguration(
            WebApplicationBuilder builder,
            ILogger<Program> startupLogger)
        {
            var environment = builder.Environment.EnvironmentName;

            startupLogger.LogInformation($"Loading configuration for {environment}");

            try
            {
                builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
                builder.Configuration.AddJsonFile("appsettings.json");
                builder.Configuration.AddJsonFile($"appsettings.{environment}.json", true);
                // If we're in the development environment, use appsettings.

                // If we're in production, we want to bring in other config sources as well.
                // i.e.:  Environment vars.
                // Remember, there's some kind override that happens when 
                // "stacking" configuration sources.  (I believe it's Last One Added has precedence.)
                
                if(environment == "Production")
                {
                    builder.Configuration.AddEnvironmentVariables();
                }

                // CHeck to make sure the config has what it needs.
                var sqlCn = builder.Configuration.GetConnectionString("userdata");
                if (sqlCn == null || sqlCn?.Contains("gmtool-data") == false)
                {
                    startupLogger.LogError("Sql Connection string is sus.");
                }

                startupLogger.LogInformation("I THINK(???) I have the configuration settings.");
            }
            catch (Exception ex)
            {
                startupLogger.LogError(ex, "An exception occurred while getting app configuration.");
            }

            return builder;
        }

        private static WebApplicationBuilder CreateServices(
            WebApplicationBuilder builder,
            ILogger<Program> startupLog)
        {
            var hostEnv = builder.Environment.EnvironmentName;

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<HttpContextAccessor>();

            // Get an instance of IConfiguration for setting up dependencies.
            IConfiguration cfg = builder.Configuration;

            // Do the MS Graph setup
            var graphCfg = cfg.LoadMsGraphConfiguration(hostEnv);
            builder.Services.UseMsGraphUserProvider(graphCfg);

			// Set up "Townsfolk" services and dependencies.
			builder.Services.AddScoped<IRulesetAccess>((sp) =>
			{
				IRulesetAccess service =
					new RulesetProvider()
						.Use5eSRD();

				return service;
			});

			builder.Services.AddScoped<ICardDeck, DeckSimulator>();

			builder.Services.AddScoped<IDiceBag, DiceBag>();

            try
            {
                builder.Services.UseNpcSqlServerAccess(builder.Configuration);
            }
            catch(Exception ex)
            {
                startupLog.LogError(ex, "Could not attach the SqlProvider.");
                throw;
            }
            
            builder.Services.AddScoped<ITownsfolkManager, TownsfolkMgr>();

            // Set up AI Subsystem and dependencies.
            try
            {
                builder.Services.UseSemanticKernelProvider(builder.Configuration, hostEnv);
                builder.Services.AddScoped<IAiWorkloadManager, AiWorkloadManager>();
            }
            catch(Exception ex)
            {
                startupLog.LogError(ex, "Could not access the LM Configurations.");
            }
			
            builder.Services.AddScoped<ICharacterWorkloads, CharacterWorkloads>();

            builder.Services.AddScoped<NpcServiceProxy>();

            return builder;
        }
    
        private static WebApplicationBuilder ConfigureAppSec(WebApplicationBuilder builder)
        {
            builder.Services.AddMicrosoftIdentityConsentHandler();
            builder.Services.AddCascadingAuthenticationState();

			// This is where we set up the Authentication goop, including the reading of the
			// incoming claims ticket.
			builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    // This is going to be a problem when we move to the cloud.
                    // The necessary settings will be in Environment Variables,
                    // not appSetting.Json format
                    builder.Configuration.Bind("AzureAdB2C", options);

                    options.Events = new OpenIdConnectEvents
                    {
                        // Some of these even handlers can get pretty lengthy.
                        // I moved the implementations into the OidcEventHandlers class
                        // just to shrink the size of that document scrollbar.
                        OnRedirectToIdentityProvider = OidcEventHandlers.OnRedirectToIdP,
                        OnAuthenticationFailed = OidcEventHandlers.OnAuthenticationfailed,
                        OnSignedOutCallbackRedirect = OidcEventHandlers.OnSignedOutCallbackRedirect,
                        OnTicketReceived = OidcEventHandlers.OnTicketReceived

                    };
                });

            builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

            builder.Services.SetAuthorizationPolicies();

            return builder;
        }
    }
}
