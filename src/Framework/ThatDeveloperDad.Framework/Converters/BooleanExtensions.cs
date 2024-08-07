using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Converters
{
	public static class BooleanExtensions
	{
		public static string AsString(this bool value)
		{
			string ret = "false";
			if (value) 
			{ 
				ret = "true"; 
			}

			return ret;
		}

		public static string AsString(this bool value, string trueValue, string falseValue)
		{
			string ret = falseValue;
			if (value)
			{
				ret = trueValue;
			}
			return ret;
		}
	}
}
