using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.LlmAccess.Contracts;

namespace ThatDeveloperDad.LlmAccess
{
	public static class DependencyBuilder
	{
		private const string LmSectionName = "LmConfig";
		internal const string apiKeySource = "UseManagedId";

		public static IServiceCollection UseSemanticKernelProvider(
			this IServiceCollection services,
			IConfiguration config,
			string? hostEnv = null)
		{
			var lmCfgSection = config.GetRequiredSection(LmSectionName);

			if(lmCfgSection == null)
			{
				throw new InvalidOperationException("The configuration for the LanguageModel Provider is missing.");
			}

			services.AddScoped<ILlmProvider, LlmProvider>((sp) => {
				string modelId = lmCfgSection["modelId"] ?? string.Empty;
				string endpoint = lmCfgSection["endpoint"] ?? string.Empty;
				string apiKey = lmCfgSection["apiKey"] ?? string.Empty;
				
				//if(hostEnv == "Production")
				//{
				//	apiKey = apiKeySource;
				//	if(endpoint == string.Empty)
				//	{
    //                    endpoint = config["AZURE_OPENAI_ENDPOINT"] ?? string.Empty;
    //                }
    //            }

				var lmCfg= new SemanticKernelConfiguration
					(
						modelId: modelId,
						endpoint: endpoint,
						apiKey: apiKey
					);

				return new LlmProvider(lmCfg);
			});

			return services;
		}
	}
}
