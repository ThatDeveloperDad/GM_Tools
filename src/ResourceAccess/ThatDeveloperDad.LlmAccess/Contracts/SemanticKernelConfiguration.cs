namespace ThatDeveloperDad.LlmAccess.Contracts
{
	public record SemanticKernelConfiguration
	{
        public SemanticKernelConfiguration(string modelId, string endpoint, string apiKey)
        {
            ModelId = modelId;
            EndpointUrl = endpoint;
            ApiKey = apiKey;
        }

        public string ModelId { get; init; }

        public string EndpointUrl { get; init; }

        public string ApiKey { get; set; }
    }
}
