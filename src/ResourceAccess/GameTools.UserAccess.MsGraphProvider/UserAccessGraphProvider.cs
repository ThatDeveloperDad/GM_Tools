using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Linq;
namespace GameTools.UserAccess.MsGraphProvider
{
	public class UserAccessGraphProvider : IUserAccess
	{
        //TODO: Turn these back to private after working the migration code.
		protected readonly GraphConfiguration _config;
        protected readonly ClientSecretCredential? _secretCred;

		private readonly ILogger? _logger;

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
        public string IdentityVendor => "MS-Entra";

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
					var graphCallResult = await proxy.Users[userId]
						.MemberOf
						.GetAsync();
                    
                    List<DirectoryObject> graphGroups = new List<DirectoryObject>();
                    graphGroups.AddRange(graphCallResult?.Value ?? Enumerable.Empty<DirectoryObject>());

                    var memberGroupNames = new List<string>();
                    foreach(var group in graphGroups)
                    {
                        Group? grp = group as Group;
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

        public async Task<List<AppUser>> LoadAppUsersAsync(string baseAppGroupId)
        {
            List<AppUser> users = new();

            var token = await GetTokenAsync();
            if(token == null)
            {
                throw new Exception("Could not get a token to the Graph Service.");
            }

            using(GraphServiceClient client 
                = new(_secretCred, new[] {"https://graph.microsoft.com/.default" }))
            {
                var groupMembers = await client
                    .Groups[baseAppGroupId]
                    .Members
					.GetAsync((requestConfiguration) =>
					{
						requestConfiguration.QueryParameters.Select = new string[] { "displayName", "id", "identities", "memberof" };
					});

                foreach(var item in groupMembers.Value)
                {
                    var user = item as User;

                    if(user!=null)
                    {
                        AppUser appuser = new()
                            {
							UserId = user.Id,
							
							DisplayName = user.DisplayName ?? string.Empty
						};
                        users.Add(appuser);
                    }
                }


                //var graphUsers = await client
                //    .Users
                //    .GetAsync();

                //foreach(var item in graphUsers.Value)
                //{
                //    users.Add
                //        (
                //            new AppUser()
                //            {
                //                UserId = item.Id,
                //                Email = item.Mail ?? string.Empty,
                //                DisplayName = item.DisplayName ?? string.Empty
                //            }
                //        );
                //}
            }

            return users;
        }

        public async Task<AppUser?> LoadAppUserAsync(string userId)
        {
            AppUser? user = null;

            //         var token = await GetTokenAsync();
            //         if(token == null)
            //{
            //	throw new Exception("Could not get a token to the Graph Service.");
            //}

            using (GraphServiceClient client = new GraphServiceClient
                (_secretCred, new[] { "https://graph.microsoft.com/.default" }))
            {
                var graphUser = await client
                    .Users[userId]
                    .GetAsync();

                if(graphUser != null)
                {
                    user = new()
                    {
                        UserId = graphUser.Id??userId,
                        DisplayName = graphUser.DisplayName ?? string.Empty
                    };
                }
            }

			return user;
        }

  //      public async Task LoopEntraGroups()
  //      {
		//	List<AppUser> users = new();

		//	var token = await GetTokenAsync();
		//	if (token == null)
		//	{
		//		throw new Exception("Could not get a token to the Graph Service.");
		//	}

  //          using (GraphServiceClient client
  //              = new(_secretCred, new[] { "https://graph.microsoft.com/.default" }))
  //          {
  //              var groups = await client.Groups.GetAsync();

  //              foreach(var group in groups.Value)
  //              {
  //                  Console.WriteLine($"{group.DisplayName} {group.Id}");
  //              }
  //          }
		//}

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
