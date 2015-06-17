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
	public class ShortExtensionsTests
	{
		[TestMethod()]
		public void CountBitsTest()
		{
			short[] tests = new short[]
			{
				0x100, 1,
				0, 0,
				1, 1,
				2, 1,
				3, 2,
				5, 2,
				0x55, 4,
				0xFF, 8,
				0xFFF, 12,
				0x1000, 1,
				0x7FFF, 15,
				short.MaxValue, 15,
				short.MinValue, 1
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var test = tests[i];
				var expected = tests[i + 1];
				var actual = test.CountBits();

				Assert.AreEqual(expected, actual, "Short: " + test.ToString("x"));
			}
			
		}
	}
}
