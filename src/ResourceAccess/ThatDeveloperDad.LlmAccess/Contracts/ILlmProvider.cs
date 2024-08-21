using ThatDeveloperDad.Framework.Wrappers;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
    /// <summary>
    /// Provides access to a Language Model using Semantic Kernel
    /// </summary>
	public interface ILlmProvider
    {


		/// <summary>
		/// Converts the prompt template found in the FunctionRequest into
		/// a semantic function, turns the function arguments into KernelArguments
		/// and uses Semantic Kernel to execute the function with the provided args.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<OpResult<LlmResponse>> ExecuteFunctionAsync(LlmRequest request);
    }
}
