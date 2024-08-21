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

		public static IServiceCollection UseSemanticKernelProvider(
			this IServiceCollection services,
			IConfiguration config)
		{
			var lmCfgSection = config.GetRequiredSection(LmSectionName);

			if(lmCfgSection == null)
			{
				throw new InvalidOperationException("The configuration for the LanguageModel Provider is missing.");
			}

			services.AddScoped<ILlmProvider, LlmProvider>((sp) => { 
				var lmCfg= new SemanticKernelConfiguration
					(
						modelId: lmCfgSection["modelId"] ?? string.Empty,
						endpoint: lmCfgSection["endpoint"] ?? string.Empty,
						apiKey: lmCfgSection["apiKey"] ?? string.Empty
					);
				return new LlmProvider(lmCfg);
			});

			return services;
		}
	}
}
