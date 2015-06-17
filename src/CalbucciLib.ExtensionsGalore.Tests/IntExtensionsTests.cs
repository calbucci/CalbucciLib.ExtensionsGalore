using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalbucciLib.ExtensionsGalore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CalbucciLib.ExtensionsGalore.Tests
{
	[TestClass()]
	public class IntExtensionsTests
	{
		[TestMethod()]
		public void ToRomanNumeralTest()
		{
			List<Tuple<int, string>> tests = new List<Tuple<int, string>>
			{
				Tuple.Create(0, ""),
				Tuple.Create(1, "I"),
				Tuple.Create(2, "II"),
				Tuple.Create(3, "III"),
				Tuple.Create(4, "IV"),
				Tuple.Create(5, "V"),
				Tuple.Create(9, "IX"),
				Tuple.Create(10, "X"),
				Tuple.Create(11, "XI"),
				Tuple.Create(45, "XLV"),
				Tuple.Create(50, "L"),
				Tuple.Create(51, "LI"),
				Tuple.Create(99, "XCIX"),
				Tuple.Create(100, "C"),
				Tuple.Create(107, "CVII"),
				Tuple.Create(457, "CDLVII"),
				Tuple.Create(555, "DLV"),
				Tuple.Create(999, "CMXCIX"),
				Tuple.Create(1247, "MCCXLVII"),
			};

			foreach (var test in tests)
			{
				string expected = test.Item2;
				string actual = test.Item1.ToRomanNumeral();

				Assert.AreEqual(expected, actual, "Number: " + test.Item1);
			}
		}


		[TestMethod()]
		public void CountBitsTest()
		{
			int[] tests = new []
			{
				0, 0,
				1, 1,
				2, 1,
				3, 2,
				5, 2,
				0x55, 4,
				0xFF, 8,
				0x100, 1,
				0xFFF, 12,
				0x1000, 1,
				0xF000, 4,
				0xF00F, 8,
				0xFF0F, 12,
				0xFFFF, 16,
				0x10000, 1,
				0x20000, 1,
				0x50000, 2,
				0xF0000, 4,
				0xF50000, 6,
				0x5F50000, 8,
				0xFFF0000, 12,
				(int)(0x7FFF0000), 15,
				(int)0x7FFF5555, 23,
				(int)0x7FFFFFFF, 31,
				int.MinValue, 1
			};

			for (int i = 0; i < tests.Length; i+=2)
			{
				var test = tests[i];
				var expected = tests[i + 1];
				var actual = test.CountBits();

				Assert.AreEqual(expected, actual, "Int: " + test.ToString("x"));
			}
		}

		[TestMethod()]
		public void PluralizeTest()
		{
			Tuple<int, string>[] tests = new Tuple<int, string>[]
			{
				Tuple.Create(0, "0 widgets"),
				Tuple.Create(1, "1 widget"),
				Tuple.Create(2, "2 widgets"),
				Tuple.Create(-10, "-10 widgets"),
				Tuple.Create(-1, "-1 widget"),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.Pluralize("widget", "widgets");
				Assert.AreEqual(expected, actual);
			}

		}

		[TestMethod()]
		public void ToAndFromHexTest()
		{
			for (int i = -100000; i <= 1000000; i += 37)
			{
				var hex = i.ToHex();
				var i2 = IntExtensions.FromHex(hex);
				Assert.AreEqual(i, i2, hex);
			}

			Assert.AreEqual(int.MinValue, IntExtensions.FromHex(int.MinValue.ToHex()));
			Assert.AreEqual(int.MaxValue, IntExtensions.FromHex(int.MaxValue.ToHex()));
			
		}

		[TestMethod()]
		public void ToRoundedMemorySizeTest()
		{
			List<Tuple<int, string>> tests = new List<Tuple<int, string>>
			{
				new Tuple<int, string>(0, "0"),
				new Tuple<int, string>(27, "27"),
				new Tuple<int, string>(854, "854"),
				new Tuple<int, string>(1023, "1023"),
				new Tuple<int, string>(1024, "1k"),
				new Tuple<int, string>(1500, "1.46k"),
				new Tuple<int, string>(1536, "1.5k"),
				new Tuple<int, string>(2048, "2k"),
				new Tuple<int, string>(5569, "5.44k"),
				new Tuple<int, string>(5576, "5.45k"),
				new Tuple<int, string>(11434, "11.2k"),
				new Tuple<int, string>(3 * 1024 * 1024, "3M"),
				new Tuple<int, string>(18025792, "17.2M"),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.ToRoundedMemorySize();

				Assert.AreEqual(expected, actual, "Test: " + test.Item1);
			}

		}

		[TestMethod()]
		public void ToLiteralTest()
		{
			List<Tuple<int, string>> tests = new List<Tuple<int, string>>
			{
				Tuple.Create(20, "twenty"),
				Tuple.Create(0, "zero"),
				Tuple.Create(1, "one"),
				Tuple.Create(10, "ten"),
				Tuple.Create(11, "eleven"),
				Tuple.Create(27, "twenty-seven"),
				Tuple.Create(99, "ninety-nine"),
				Tuple.Create(100, "one hundred"),
				Tuple.Create(200, "two hundred"),
				Tuple.Create(201, "two hundred one"),
				Tuple.Create(210, "two hundred ten"),
				Tuple.Create(225, "two hundred twenty-five"),
				Tuple.Create(999, "nine hundred ninety-nine"),
				Tuple.Create(1000, "one thousand"),
				Tuple.Create(3357, "three thousand three hundred fifty-seven"),
				Tuple.Create(27853, "twenty-seven thousand eight hundred fifty-three"),
				Tuple.Create(555444, "five hundred fifty-five thousand four hundred forty-four"),
				Tuple.Create(4234876, "four million two hundred thirty-four thousand eight hundred seventy-six"),

				// 2,147,483,647
				Tuple.Create(int.MaxValue, "two billion one hundred forty-seven million four hundred eighty-three thousand six hundred forty-seven"), 
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.ToLiteral();

				Assert.AreEqual(expected, actual, test.Item1.ToString());
			}
		}
	}
}
