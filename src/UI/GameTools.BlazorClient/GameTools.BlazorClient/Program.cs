using GameTools.API.WorkloadProvider;
using GameTools.BlazorClient.Components;
using GameTools.BlazorClient.Services;
using GameTools.DiceEngine;
using GameTools.NPCAccess.SqlServer;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.RulesetAccess;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
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
            builder.Logging.AddConsole();
            builder.Logging.AddAzureWebAppDiagnostics();

            var startupLog = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            startupLog.LogInformation("Startup Log has been created.  Let's see what's going on.");

            builder = SetUpConfiguration(builder, startupLog);

            builder = CreateServices(builder, startupLog);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

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
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
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

			// Set up AI Aubsystem and dependencies.
			builder.Services.UseSemanticKernelProvider(builder.Configuration);
			builder.Services.AddScoped<IAiWorkloadManager, AiWorkloadManager>();

			
            builder.Services.AddScoped<ICharacterWorkloads, CharacterWorkloads>();

            builder.Services.AddScoped<NpcServiceProxy>();

            return builder;
        }
    }
}
