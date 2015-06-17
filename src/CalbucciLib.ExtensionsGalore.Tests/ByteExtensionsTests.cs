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
	public class ByteExtensionsTests
	{
		[TestMethod()]
		public void CountBitsTest()
		{
			int[] bitCounts = new[]
			{
				0, 0,
				1, 1,
				2, 1,
				3, 2,
				7, 3,
				8, 1,
				255, 8
			};

			for (int i = 0; i < bitCounts.Length; i += 2)
			{
				byte b = (byte) bitCounts[i];
				int bits = b.CountBits();

				Assert.AreEqual(bits, bitCounts[i+1], "Failed: " + b);
			}
		}

		[TestMethod()]
		public void FromHexTest()
		{
			for (int i = 0; i <= byte.MaxValue; i++)
			{
				byte b = (byte) i;
				string hexByte = b.ToHex();
				byte actual = ByteExtensions.FromHex(hexByte);

				Assert.AreEqual(b, actual, b.ToString());

			}

			Assert.AreEqual(ByteExtensions.FromHex(null), 0);
			Assert.AreEqual(ByteExtensions.FromHex(""), 0);
			Assert.AreEqual(ByteExtensions.FromHex("abc"), 0);
			Assert.AreEqual(ByteExtensions.FromHex("ge"), 0);
			Assert.AreEqual(ByteExtensions.FromHex("çá"), 0);
			Assert.AreEqual(ByteExtensions.FromHex("=="), 0);
			Assert.AreEqual(ByteExtensions.FromHex("="), 0);
			Assert.AreEqual(ByteExtensions.FromHex("m"), 0);

			Assert.AreEqual(ByteExtensions.FromHex("a"), 10);


		}
	}
}
