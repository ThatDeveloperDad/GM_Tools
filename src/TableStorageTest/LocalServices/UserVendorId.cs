using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTest.LocalServices
{
	/// <summary>
	/// Holds a user's ID for the identified external vendor's systems.
	/// </summary>
	internal class UserVendorId
	{
		public string VendorName { get; set; }
		public string UserIdAtVendor { get; set; }
	}
}
