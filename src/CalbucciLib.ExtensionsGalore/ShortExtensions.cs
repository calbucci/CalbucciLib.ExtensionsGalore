using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	public static class ShortExtensions
	{
		public static int CountBits(this short s)
		{
			var b1 = (byte)(s & 0xFF);
			var b2 = (byte)(s >> 8);

			return b1.CountBits() + b2.CountBits();
		}
	}
}
