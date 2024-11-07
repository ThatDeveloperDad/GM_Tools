using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.UserAccess
{
	public interface IUserAccess
	{
		string IdentityVendor { get; }

		/// <summary>
		/// Obtains the list of Permission Groups the current user belongs to.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		List<string> LoadUserPermissionGroups(string? userId);

		Task<List<string>> LoadUserPermissionGroupsAsync(string? userId);

		Task<List<AppUser>> LoadAppUsersAsync(string baseAppUserGroupId);

		Task<AppUser?> LoadAppUserAsync(string userId);
	}
}
