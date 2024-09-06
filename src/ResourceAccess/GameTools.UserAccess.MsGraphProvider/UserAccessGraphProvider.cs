using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
namespace GameTools.UserAccess.MsGraphProvider
{
	public class UserAccessGraphProvider : IUserAccess
	{
		private readonly GraphConfiguration _config;
        private readonly ILogger? _logger;
        private readonly ClientSecretCredential? _secretCred;
        private readonly GraphServiceClient _graphProxy;

        public UserAccessGraphProvider(GraphConfiguration config,
            ILogger<UserAccessGraphProvider>? logger = null)
        {
            _logger = logger;
            _config = config;

            _secretCred = new ClientSecretCredential
                (
                    tenantId: _config.TenantId,
                    clientId: _config.ClientId,
                    clientSecret: _config.ClientSecret
                );
        }

#region Implements IUserAccess
		/// <summary>
        /// Connects to Microsoft Graph to obtain the list of security groups
        /// the identified user object is a member of.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> LoadUserPermissionGroups(string? userId)
		    => LoadUserPermissionGroupsAsync(userId).Result;

        public async Task<List<string>> LoadUserPermissionGroupsAsync(string? userId)
        {
			List<string> groups = new List<string>();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return groups;
            }

            //TODO:  Wrap in a try catch, log any exceptions.
            var token = await GetTokenAsync();

            using(GraphServiceClient proxy = new GraphServiceClient
                (_secretCred, new[] { "https://graph.microsoft.com/.default" }))
            {
				var user = await proxy.Users[userId].GetAsync();
				if (user != null)
				{
					var userGroups = await proxy.Users[userId]
						.MemberOf
						.GetAsync();

                    var memberGroupNames = new List<string>();
                    foreach(var group in userGroups.Value)
                    {
                        Microsoft.Graph.Models.Group? grp = group as Microsoft.Graph.Models.Group;
                        if(grp?.DisplayName != null)
                        {
                            memberGroupNames.Add(grp.DisplayName);
                        }
					}

                    if (string.IsNullOrWhiteSpace(_config.ApplicationGroupPrefix) == false)
                    {
                        var filteredGroups =
                            memberGroupNames
                                .Where(n => n.StartsWith(_config.ApplicationGroupPrefix ?? string.Empty))
                                .ToList();

                        groups.AddRange(filteredGroups);

                    }
                    else
                    {
                        groups.AddRange(memberGroupNames);
                    }
				}
            }

			return groups;
		}

		#endregion

		private async Task<string> GetTokenAsync()
        {
            if(_secretCred == null)
            {
                throw new NullReferenceException("The Client Secret credential for the MS Graph call is null.");
            }

            var tokenCtx = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
            var tokenResponse = await _secretCred.GetTokenAsync(tokenCtx);
            return tokenResponse.Token;
        }


    }
}
