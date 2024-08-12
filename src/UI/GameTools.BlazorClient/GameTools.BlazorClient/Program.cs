using GameTools.API.WorkloadProvider;
using GameTools.BlazorClient.Client.Pages;
using GameTools.BlazorClient.Components;
using GameTools.DiceEngine;
using GameTools.RulesetAccess;
using GameTools.RulesetAccess.Contracts;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using GameTools.NPCAccess;
using GameTools.NPCAccess.SqlServer;
using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.LlmAccess.Contracts;
using ThatDeveloperDad.LlmAccess;
using ThatDeveloperDad.AIWorkloadManager;
using GameTools.BlazorClient.Services;

namespace GameTools.BlazorClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder = SetUpConfiguration(builder);

            builder = CreateServices(builder);

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

        private static WebApplicationBuilder SetUpConfiguration(WebApplicationBuilder builder)
        {
            var environment = builder.Environment.EnvironmentName;
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile($"appsettings.{environment}.json", true);


            return builder;
        }

        private static WebApplicationBuilder CreateServices(WebApplicationBuilder builder)
        {
            builder.Logging.AddConsole();

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

            builder.Services.UseNpcSqlServerAccess(builder.Configuration);

            builder.Services.AddScoped<ITownsfolkManager, TownsfolkMgr>();

			// Set up AI Aubsystem and dependencies.
			builder.Services.AddScoped<ILlmProvider, LlmProvider>();
			builder.Services.AddScoped<IAiWorkloadManager, AiWorkloadManager>();

			
            builder.Services.AddScoped<ICharacterWorkloads, CharacterWorkloads>();

            builder.Services.AddScoped<NpcServiceProvider>();

            return builder;
        }
    }
}
