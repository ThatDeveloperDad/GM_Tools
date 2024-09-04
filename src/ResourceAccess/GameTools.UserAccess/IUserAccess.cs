using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.UserAccess
{
	public interface IUserAccess
	{

		/// <summary>
		/// Obtains the list of Permission Groups the current user belongs to.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		List<string> LoadUserPermissionGroups(string? userId);

		Task<List<string>> LoadUserPermissionGroupsAsync(string? userId);
	}
}
