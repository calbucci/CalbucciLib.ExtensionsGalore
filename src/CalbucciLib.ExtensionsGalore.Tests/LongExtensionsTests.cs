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
	public class LongExtensionsTests
	{
		[TestMethod()]
		public void ToRoundedMemorySizeTest()
		{
			List<Tuple<long, string>> tests = new List<Tuple<long, string>>
			{
				new Tuple<long, string>(0, "0"),
				new Tuple<long, string>(27, "27"),
				new Tuple<long, string>(854, "854"),
				new Tuple<long, string>(1023, "1023"),
				new Tuple<long, string>(1024, "1k"),
				new Tuple<long, string>(1500, "1.46k"),
				new Tuple<long, string>(1536, "1.5k"),
				new Tuple<long, string>(2048, "2k"),
				new Tuple<long, string>(5569, "5.44k"),
				new Tuple<long, string>(5576, "5.45k"),
				new Tuple<long, string>(11434, "11.2k"),
				new Tuple<long, string>(3 * 1024 * 1024, "3M"),
				new Tuple<long, string>(18025792, "17.2M"),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.ToRoundedMemorySize();

				Assert.AreEqual(expected, actual, "Test: " + test.Item1);
			}

		}

		[TestMethod()]
		public void CountBitsTest()
		{
			long[] tests = new long[]
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
				0xFFFFFFFF, 32,
				0x5555FFFFFFFF, 40,
				0x7FFFFFFFFFFFFFFF, 63,
				long.MinValue, 1,
				long.MaxValue, 63
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var test = tests[i];
				var expected = tests[i + 1];
				var actual = test.CountBits();

				Assert.AreEqual(expected, actual, "Long: " + test.ToString("x"));
			}


		}

		[TestMethod()]
		public void ToBase62Test()
		{
			Assert.AreEqual(LongExtensions.FromBase62(""), (long)0);

			Random r = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				long l = (((long) r.Next()) << 32) + i;
				string b62 = l.ToBase62();
				long l2 = LongExtensions.FromBase62(b62);
				Assert.AreEqual(l, l2, "Long: " + l);
			}

		}

		[TestMethod()]
		public void LiteralTest()
		{
			List<Tuple<long, string>> tests = new List<Tuple<long, string>>
			{
				Tuple.Create((long)20, "twenty"),
				Tuple.Create((long)0, "zero"),
				Tuple.Create((long)1, "one"),
				Tuple.Create((long)10, "ten"),
				Tuple.Create((long)11, "eleven"),
				Tuple.Create((long)27, "twenty-seven"),
				Tuple.Create((long)99, "ninety-nine"),
				Tuple.Create((long)100, "one hundred"),
				Tuple.Create((long)200, "two hundred"),
				Tuple.Create((long)201, "two hundred one"),
				Tuple.Create((long)210, "two hundred ten"),
				Tuple.Create((long)225, "two hundred twenty-five"),
				Tuple.Create((long)999, "nine hundred ninety-nine"),
				Tuple.Create((long)1000, "one thousand"),
				Tuple.Create((long)3357, "three thousand three hundred fifty-seven"),
				Tuple.Create((long)27853, "twenty-seven thousand eight hundred fifty-three"),
				Tuple.Create((long)555444, "five hundred fifty-five thousand four hundred forty-four"),
				Tuple.Create((long)4234876, "four million two hundred thirty-four thousand eight hundred seventy-six"),

				Tuple.Create((long)5000000000, "five billion"),
				Tuple.Create((long)-1, "minus one"),
				Tuple.Create((long)-27, "minus twenty-seven"),

				Tuple.Create((long)7000000000100, "seven trillion one hundred"),

				// 9,223,372,036,854,775,807
				Tuple.Create(long.MaxValue, "nine quintillion two hundred twenty-three quadrillion three hundred seventy-two trillion thirty-six billion eight hundred fifty-four million seven hundred seventy-five thousand eight hundred seven"), 
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
