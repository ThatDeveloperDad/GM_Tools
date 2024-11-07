using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.AzureTablesProvider.StorageModels
{
	internal class UserEntity : ITableEntity
	{
		public const string BaseTableName = "BusinessObjects";
		public const string TablePartitionId = "Users";

        public string UserId { get; set; }

        public string DisplayName { get; set; }

		public string SubscriptionStatus { get; set; }

		public string IdsJson { get; set; } = string.Empty;

		public string CurrentSubscriptionJson { get; set; } = string.Empty;

		public string SubscriptionActivityJson { get; set; } = string.Empty;

        #region ITableEntity implementation
        /// <summary>
        /// Using Dummy fields to catch the values that come in via the
        /// interface required setter.  The real values of these properties are
        /// calculated.
        /// </summary>
        private string? _partitionKeyDummy;
		public string PartitionKey { get => TablePartitionId; set => _partitionKeyDummy = value; }

		/// <summary>
		/// Same reasoning here as for _partitionKeyDummy.
		/// </summary>
		private string? _rowKeyDummy;
		public string RowKey { get => UserId; set => _rowKeyDummy = value; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }
#endregion

	}
}
