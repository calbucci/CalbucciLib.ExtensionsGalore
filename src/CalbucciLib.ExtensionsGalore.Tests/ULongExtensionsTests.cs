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
	public class ULongExtensionsTests
	{

		[TestMethod()]
		public void CountBitsTest()
		{
			ulong[] tests = new ulong[]
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
				ulong.MinValue, 0,
				ulong.MaxValue, 64
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var test = tests[i];
				var expected = (int)tests[i + 1];
				var actual = test.CountBits();

				Assert.AreEqual(expected, actual, "ULong: " + test.ToString("x"));
			}
		}
	}
}
