using GameTools.NPCAccess.AzureTables.StorageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.NPCAccess.AzureTables.Transformers
{
	internal static class ModelExtensions
	{
		public static NpcAccessModel ToDto(this NpcTableEntity entity)
		{
			NpcAccessModel dto = new()
			{
				NpcId = entity.NpcId,
				UserId = entity.UserId,
				Species = entity.SpeciesName,
				Vocation = entity.VocationName,
				CharacterName = entity.CharacterName,
				CharacterDetails = entity.CharacterDetails,
				IsPublic = entity.IsPublic
			};

			return dto;
		}
	}
}
