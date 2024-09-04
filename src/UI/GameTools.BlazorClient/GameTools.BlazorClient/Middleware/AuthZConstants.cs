namespace GameTools.BlazorClient.Middleware
{
	/// <summary>
	/// AuthZ Constants holds non-sensitive values as named constants
	/// so I don't have to remember them.
	/// 
	/// I'm using prefixes to categorize these constants.
	/// pol_ = a constant that has to do with Policies
	/// role_ = a constant that defines a Role Name.
	/// </summary>
	public static class AuthZConstants
	{
		public const string pol_PublicAccess = "PublicAccess";
		public const string pol_KnownUsers = "KnownUsers";
		public const string pol_PaidUsers = "PaidUsers";

		public const string role_KnownUser = "app-gmtools-users";
		public const string role_PaidUser = "app-gmtools-paid-users";
	}
}
