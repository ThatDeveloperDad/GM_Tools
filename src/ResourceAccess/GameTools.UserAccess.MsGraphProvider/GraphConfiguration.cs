using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.UserAccess.MsGraphProvider
{
	public class GraphConfiguration
	{
		internal const string SettingNode = "MicrosoftGraph";
		internal const string SettingKey_TenantId = "TenantId";
        internal const string SettingKey_ClientId = "ClientId";
        internal const string SettingKey_ClientSecret = "ClientSecret";
        internal const string SettingKey_AppGrpPrefix = "AppGroupPrefixPattern";
		
		public string? TenantId { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        /// <summary>
        /// For User directories that control permission groups for multiple applications,
        /// those app-specific groups should all use a consistent prexif pattern in their naming.
        /// That way, we can apply the naming pattern as a prefix when an application is
        /// querying permissions sets for a user, for controlling that user's access to
        /// Application Features.
        /// </summary>
        public string? ApplicationGroupPrefix { get; set; }

    }
}
