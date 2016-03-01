using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	public static class ByteExtensions
	{
		private static int[] _BitsMap;
		private static byte[] _HexMap;

		static ByteExtensions()
		{
			var b4Count = new[] {0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4};
			_BitsMap = new int[256];
			for (int i = 1; i < 256; i++)
			{
				var b4a = i & 0xF;
				var b4b = (i & 0xF0) >> 4;

				_BitsMap[i] = b4Count[b4a] + b4Count[b4b];
			}

			_HexMap = new byte[(int) 'f' + 1];
			for (int i = 0; i < _HexMap.Length; i++)
				_HexMap[i] = byte.MaxValue; // invalid value

			for (int i = 0; i < 10; i++)
				_HexMap[((int) '0') + i] = (byte) i;
			for (int i = 0; i < 6; i++)
			{
				_HexMap[((int) 'a') + i] = (byte) (10 + i);
				_HexMap[((int) 'A') + i] = (byte) (10 + i);
			}
		}

		// ==========================================================================
		//
		//    Hex Encoding
		//
		// ==========================================================================

		public static byte FromHex(string str)
		{
			if (str == null)
				return 0;

			if (str.Length == 1)
			{
				// assume a single hex code
				if (str[0] >= _HexMap.Length)
					return 0;
				var b = _HexMap[str[0]];
				return b == (byte)255 ? (byte)0 : b;
			}

			if (str.Length != 2)
				return 0;

			char h1 = str[0];
			char h2 = str[1];

			if (h1 >= _HexMap.Length || h2 >= _HexMap.Length)
				return 0;

			var b1 = _HexMap[h1];
			var b2 = _HexMap[h2];

			if (b1 == byte.MaxValue || b2 == byte.MaxValue)
				return 0;

			return (byte)((b1 << 4) + b2);
		}

		public static string ToHex(this byte b)
		{
			return b.ToString("x2");
		}


		// ==========================================================================
		//
		//    Others
		//
		// ==========================================================================
		public static int CountBits(this byte b)
		{
			return _BitsMap[b];
		}


	}
}
