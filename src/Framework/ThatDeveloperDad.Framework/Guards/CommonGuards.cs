using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Guards
{
	public static class CommonGuards
	{
		/// <summary>
		/// Can be used wherever we want to throw an exception when the passed in reference is null.
		/// 
		/// Because this throws exceptions, it's recommended that this Guard clause be used
		/// only during application spin up.  We want to avoid throwing Exceptions around all
		/// willy-nilly once the application is running.
		/// </summary>
		/// <param name="reference"></param>
		/// <param name="messageIfNull"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void GuardNullReference(this object? reference, string messageIfNull)
		{
			if(reference == null) throw new ArgumentNullException(messageIfNull);
		}
	}
}
