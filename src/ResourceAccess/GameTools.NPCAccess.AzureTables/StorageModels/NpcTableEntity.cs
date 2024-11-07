using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.NPCAccess.AzureTables.StorageModels
{
	public class NpcTableEntity : ITableEntity
	{
		public const string PartitionKeyPrefix = "Npcs";

		private string _partitionKey = string.Empty;
		public string PartitionKey 
		{
			get 
			{
				var userId = UserId.Replace("-", "");
				return $"{PartitionKeyPrefix}_{userId}";
			}
			set 
			{
				_partitionKey = value;
			} 
		} 
		
		public string RowKey { get; set; }
		

        public int NpcId { get; set; }
        public string UserId { get; set; }
		public string SpeciesName { get; set; }
		public string VocationName { get; set; }
		public string? CharacterName { get; set; }
		public string CharacterDetails { get; set; }
		public bool IsPublic { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? DeletedDate { get; set; }
		public ETag ETag { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
	}

}
