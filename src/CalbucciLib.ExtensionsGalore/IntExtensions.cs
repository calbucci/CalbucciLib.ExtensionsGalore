using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	public static class IntExtensions
	{
		private static readonly string[][] _RomanMapping = new[]
		{
			new[] {"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"},
			new[] {"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"},
			new[] {"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"},
			new[] {"", "M", "MMM", "MMM"}
		};


		public static int FromHex(string str)
		{
			if (string.IsNullOrWhiteSpace(str))
				return 0;
			str = str.Trim();
			if (str.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
			{
				str = str.Substring(2);
				if (str.Length == 0)
					return 0;
			}
			if (str.Length > 8)
				str = str.Substring(0, 8);
			try
			{
				// between 1 and 8 hex decimals
				return Convert.ToInt32(str, 16);
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public static string ToHex(this int i, bool useLeadingZeroes = false)
		{
			if (useLeadingZeroes)
				return i.ToString("x8");
			return i.ToString("x");
		}

		public static int CountBits(this int i)
		{
			if (i == 0)
				return 0;

			i = i - ((i >> 1) & 0x55555555);
			i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
			return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
		}

		public static string ToRomanNumeral(this int i)
		{
			if (i <= 0 || i > 3999)
				return "";

			var digits = i.ToString().Reverse().ToArray();

			StringBuilder sb = new StringBuilder();
			for (int j = digits.Length - 1; j >= 0; j--)
			{
				int pos = (digits[j]) - '0';
				string roman = _RomanMapping[j][pos];
				sb.Append(roman);
			}

			return sb.ToString();
		}

		public static string ToRoundedMemorySize(this int i, int maxDecimalPlaces = 2)
		{
			return ((long) i).ToRoundedMemorySize(maxDecimalPlaces);
		}

		public static string Pluralize(this int i, string one, string twoOrMore, string zero = null)
		{
			if (i == 1 || i == -1)
				return i + " " + one;

			if (i == 0)
			{
				if (zero == null)
					return i + " " + twoOrMore; // English standard
				return i + " " + zero;
			}

			return i.ToString("N0") + " " + twoOrMore;
		}

		public static string ToLiteral(this int i)
		{
			return ((long) i).ToLiteral();
		}


	}
}
