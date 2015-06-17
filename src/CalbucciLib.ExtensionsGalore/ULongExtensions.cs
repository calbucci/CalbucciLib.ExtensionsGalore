using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	static public class ULongExtensions
	{
		static public string ToLiteral(this ulong u)
		{
			return ((long)u).ToLiteral();
		}

		static public int CountBits(this ulong u)
		{
			var uiLow = (uint)(u & 0xFFFFFFFF);
			var uiHigh = (uint)(u >> 32);

			return uiLow.CountBits() + uiHigh.CountBits();
		}

	}
}
