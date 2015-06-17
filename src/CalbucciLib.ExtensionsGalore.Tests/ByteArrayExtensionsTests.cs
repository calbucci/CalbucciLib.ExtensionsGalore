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
	public class ByteArrayExtensionsTests
	{
		[TestMethod()]
		public void ToBase64Test()
		{
			Random r = new Random(1);
			for (int i = 1; i < 64; i++)
			{
				byte[] bytes = new byte[i]; 
				r.NextBytes(bytes);

				string base64 = bytes.ToBase64();
				byte[] bytes2 = base64.ToBytesFromBase64();

				Assert.IsTrue(bytes.IsEqual(bytes2));
			}


		}

		[TestMethod()]
		public void ToBase64_Empty_Test()
		{
			byte[] bytes = new byte[0];

			string base64 = bytes.ToBase64();
			Assert.IsTrue(bytes.IsEqual(base64.ToBytesFromBase64()));
		}

		[TestMethod()]
		public void ToBase62Test()
		{
			Random r = new Random(1);
			for (int i = 1; i < 64; i++)
			{
				byte[] bytes = new byte[i];
				r.NextBytes(bytes);

				string base62 = bytes.ToBase62();
				byte[] bytes2 = base62.ToBytesFromBase62();

				Assert.IsTrue(bytes.IsEqual(bytes2));
			}



		}

		[TestMethod()]
		public void ToBase62_Empty_Test()
		{
			byte[] bytes = new byte[0];

			string base62 = bytes.ToBase62();
			Assert.IsTrue(bytes.IsEqual(base62.ToBytesFromBase64()));
		}

		[TestMethod()]
		public void ToHexEncodingTest()
		{
			Random r = new Random(1);
			for (int i = 1; i < 64; i++)
			{
				byte[] bytes = new byte[i];
				r.NextBytes(bytes);

				string hex = bytes.ToHexEncoding();
				byte[] bytes2 = hex.ToBytesFromHex();

				Assert.IsTrue(bytes.IsEqual(bytes2), "Size: " + i);
			}
		}

		[TestMethod()]
		public void ToHexEncodingUpper_Test()
		{
			Random r = new Random(1);
			for (int i = 1; i < 64; i++)
			{
				byte[] bytes = new byte[i];
				r.NextBytes(bytes);

				string hex = bytes.ToHexEncoding().ToUpper();
				byte[] bytes2 = hex.ToBytesFromHex();

				Assert.IsTrue(bytes.IsEqual(bytes2), "Size: " + i);
			}
		}

		[TestMethod()]
		public void ToHexEncoding_Empty_Test()
		{
			byte[] bytes = new byte[0];

			string hex = bytes.ToHexEncoding();
			Assert.IsTrue(bytes.IsEqual(hex.ToBytesFromHex()));
		}


		[TestMethod()]
		public void ToStringFromUTF8Test()
		{
			string[] tests = new[]
			{
				null,
				"",
				"a",
				"ação",
				"\u1234\u4567"
			};

			foreach (var test in tests)
			{
				var utf8Bytes = ByteArrayExtensions.ToUTF8(test);
				var actual = utf8Bytes.ToStringFromUTF8();

				Assert.AreEqual(test, actual);
			}
		}

	}
}
