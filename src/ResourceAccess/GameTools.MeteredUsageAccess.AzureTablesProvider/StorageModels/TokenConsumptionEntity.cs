using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.AzureTablesProvider.StorageModels
{
	internal class TokenConsumptionEntity : ITableEntity
	{
		public const string BaseTableName = "BusinessObjects";
		public const string TablePartitionId = "ConsumptionLog";
		public TokenConsumptionEntity()
        {
			PartitionKey = TablePartitionId;
			RowKey = Guid.NewGuid().ToString();

			FunctionName = string.Empty;
			UserId = string.Empty;
			modelId = string.Empty;
		}

		public string UserId { get; set; }
		public DateTime InferenceTimeUtc { get; set; }
		public string FunctionName { get; set; }
		public int PromptTokens { get; set; }
		public int CompletionTokens { get; set; }
		public string modelId { get; set; }


#region ITableEntity implementation
		/// <summary>
		/// Since the actual PartitionKey is derived from the constant,
		/// and the ITableEntity interface requires a PartitionKey setter property,
		/// we'll just read that into this dummy value.
		/// </summary>
		private string? _partitionKeyDummy;
        public string PartitionKey { get => TablePartitionId; set => _partitionKeyDummy = value; }

		public string RowKey { get; set ; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }
#endregion
	}
}
