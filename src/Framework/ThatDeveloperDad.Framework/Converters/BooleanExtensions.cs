using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Converters
{
	public static class BooleanExtensions
	{
		public const string False = "false";
		public const string True = "true";
		public const string ShowItem = "shown";
		public const string HideItem = "hidden";


		public static string AsString(this bool value)
		{
			string ret = False;
			if (value) 
			{ 
				ret = True; 
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

		public static string AsVisibility(this bool value)
		{
			string ret = HideItem;
			if(value == true)
			{
				ret = ShowItem;
			}

			return ret;
		}
	}
}
