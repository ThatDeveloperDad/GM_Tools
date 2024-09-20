using GameTools.Framework.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.Framework.Converters
{
	public static class EnumLabelConverters
	{
		public static string ToUiLabel(this MeteredResourceKind resourceKind)
		{
			string uiLabel = string.Empty;

			uiLabel = resourceKind switch
			{
				MeteredResourceKind.NpcStorage => "Stored NPCs",
				MeteredResourceKind.NpcAiDetail => "Ai Details",
				_ => string.Empty
			};

			return uiLabel;
		}
	}
}
