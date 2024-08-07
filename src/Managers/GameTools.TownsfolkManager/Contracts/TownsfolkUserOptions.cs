using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.TownsfolkManager.Contracts
{
	/// <summary>
	/// Carries the user-selected values for these options.
	/// A null value means that attribute will be randomly selected.
	/// </summary>
	public class TownsfolkUserOptions
	{
		public string? Species { get; set; }

		public string? Gender { get; set; }

		public string? Background { get; set; }

		public string? Vocation { get; set; }

		public bool? IsRetired { get; set; }
	}
}
