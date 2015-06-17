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
	public class ValidateTests
	{
		[TestMethod()]
		public void IsValidEmailTest()
		{
			// http://blogs.msdn.com/b/testing123/archive/2009/02/05/email-address-test-cases.aspx

			string[] validEmails = new[]
			{
				"a@a.co",
				"test+123@gmail.com",
				"marcelo%calbucci@ww2.blogspot.careers.com",
				"abc.def%ghi+nth@gmail.com",
				"email@123.123.123.123",
				"\"email\"@domain.com",
				"eMail@domain-one.com",
				"a_______@domain.com",
			};

			foreach (string email in validEmails)
			{
				Assert.IsTrue(Validate.IsValidEmail(email), email);
			}

			string[] invalidEmails = new[]
			{
				null,
				"",
				" ",
				"mark",
				"john.com",
				"@abc.com",
				"jane@emily",
				"jane@emily.",
				"j@e.c",
				"J@.com",
				"@j.com",
				"#@%^%#$@#$@abc.com",
				"j@n@abc.com",
				"email.domain.com",
				"email.@domain.com",
				"あいうえお@domain.com",
				"email@domain.com (Joe Smith)",
				"email@-domain.com",
				"email@111.222.333.44444",
				"email@domain..com",
				"em\rail@domain.com",
				"\remail@domain.com",
				"email\r@domain.com",
				"email@do\rmain.com"
			};

			foreach (string nonEmail in invalidEmails)
			{
				Assert.IsFalse(Validate.IsValidEmail(nonEmail), nonEmail);
			}
		}

		[TestMethod()]
		public void IsValidLinkTest()
		{
			string[] validLinks = new[]
			{
				"mailto:mark@domain.com?subject=test",
				"http://a.com",
				"https://b.com",
				"http://a.com/",
				"http://a.com/search",
				"http://a.com/search?",
				"http://a.com/search?q",
				"http://a.com/search?q=",
				"http://a.com/search?q=1",
				"http://a.com/search?q=1&b=2",
				"http://a.com/search?q=1&b=2#",
				"http://a.com/search?q=1&b=2#a",
				"http://a.com/search?q=1&b=2#a=",
				"http://a.com/search?q=1&b=2#a=b",
				"http://a.com/search?q=1&b=2#!a=b&c=d",
				"http://a.com/search#abc",
				"https://a.com:1234/search?q=1&b=2#!a=b&c=d",
				"https://user:password@a.com:1234/search?q=1&b=2#!a=b&c=d",

				"mailto:mark@domain.com",
				"mailto:mark@domain.com?subject=test",
			};

			foreach (string validLink in validLinks)
			{
				Assert.IsTrue(Validate.IsValidLink(validLink), validLink);
			}

			string[] invalidLinks = new[]
			{
				null,
				"",
				"://",
				"abc.com",
				"abc.com/search",
				"http:abc.com",
				":abc.com",
				"://abc.com",
				"htp://abc.com",
				"http://abc.",
				"http://abc.c",
				"http://abc.com=abc",
				"mailto:"
			};

			foreach (string invalidLink in invalidLinks)
			{
				Assert.IsFalse(Validate.IsValidLink(invalidLink), invalidLink);
			}
		}

		[TestMethod()]
		public void IsValidDomainTest()
		{
			string[] validDomains = new[]
			{
				"a.co",
				"abc.com",
				"info.info",
				"1-800-flowers.com",
				"www.w2.web.biz"
			};

			foreach (var domain in validDomains)
			{
				Assert.IsTrue(Validate.IsValidDomain(domain), domain);
			}

			string[] invalidDomains = new[]
			{
				null,
				"",
				" ",
				" a.co",
				"abc",
				".com",
				"abc.",
				"abc.c",
				"-.com",
				"_.com",
				"google.invalidtld"
			};

			foreach (var domain in invalidDomains)
			{
				Assert.IsFalse(Validate.IsValidDomain(domain), domain);
			}
		}

		[TestMethod()]
		public void IsValidUSPhoneNumberTest()
		{
			string[] validPhones = new[]
			{
				"+1(206)831-3131",
				"2068313131",
				"206-8313131",
				"206-831-3131",
				"(206) 831-3131",
				"+1 (206) 831-3131",
				"+12068313131"
			};

			foreach (var phone in validPhones)
			{
				Assert.IsTrue(Validate.IsValidUSPhoneNumber(phone), phone);
			}

			Assert.IsTrue(Validate.IsValidUSPhoneNumber("8313131", false));
			Assert.IsTrue(Validate.IsValidUSPhoneNumber("831-3131", false));
			Assert.IsTrue(Validate.IsValidUSPhoneNumber("831-31-31", false));

			string[] invalidPhones = new[]
			{
				null,
				"",
				"abc",
				"123",
				"911",
				"123456",
				"123-456",
				"42583199",
				"425831999",
				"425-831-999",
				"999-831-9876",
				"+2(206)831-3131",
				"+11(206)831-3112"
			};

			foreach (var phone in invalidPhones)
			{
				Assert.IsFalse(Validate.IsValidUSPhoneNumber(phone), phone);
			}
		}

		[TestMethod()]
		public void IsValidPhoneNumberTest()
		{
			string[] validPhones = new[]
			{

				"+1(206)831-3131",
				"2068313131",
				"206-8313131",
				"206-831-3131",
				"(206) 831-3131",
				"+1 (206) 831-3131",
				"+12068313131",
				"123456", // 6-digit
				"+55151155738888" // 14-digit
			};

			foreach (var phone in validPhones)
			{
				Assert.IsTrue(Validate.IsValidPhoneNumber(phone), phone);
			}

			string[] invalidPhones = new[]
			{
				null,
				"",
				"abc",
				"123",
				"911",
				"+11(173206)831-3112"
			};

			foreach (var phone in invalidPhones)
			{
				Assert.IsFalse(Validate.IsValidPhoneNumber(phone), phone);
			}
			
		}

		[TestMethod()]
		public void IsValidBase64StringTest()
		{
			for (int i = 0; i < 256; i++)
			{
				byte[] ba = new[] {(byte)i};
				var b64 = ba.ToBase64();
				Assert.IsTrue(Validate.IsValidBase64String(b64));
			}

			Random r = new Random(123);
			for (int i = 0; i < 10; i++)
			{
				byte[] ba = new byte[32 + i];
				r.NextBytes(ba);
				var b64 = ba.ToBase64();
				Assert.IsTrue(Validate.IsValidBase64String(b64));
			}

			Assert.IsTrue(Validate.IsValidBase64String(null));
			Assert.IsTrue(Validate.IsValidBase64String(""));

			Assert.IsFalse(Validate.IsValidBase64String("$"));
			Assert.IsFalse(Validate.IsValidBase64String("abc78#"));
			Assert.IsFalse(Validate.IsValidBase64String("ábc"));

		}

		[TestMethod()]
		public void IsValidHtmlColorTest()
		{
			string[] testsValid = new[]
			{
				"White",
				"#fff",
				"#ffffff",
				"fff",
				"rgb(1,2,3)",
				"rgba(1,2,3,1.0)",
				"hsl(120, 100%, 50%)",
				"hsla( 30, 10%, .25, 0.5)"
			};

			foreach (var test in testsValid)
			{
				Assert.IsTrue(Validate.IsValidHtmlColor(test), test);
			}

			string[] testsInvalid = new[]
			{
				"Whity",
				"#ff",
				"#fffff",
				"#fgh",
				"klm",
				"rgb[1,2,3]",
				"rgb(1,2,3,4)",
				"rgb(1,2)",
				"rgb(1,2,3,1.0)",
				"rgba(1,2,3)",
				"hsl(120, 200%, 50%)",
				"hsla( 10%, .25, 0.5)",
				"hsla( 30, 10%, .25, 0.5, 3)"

			};

			foreach (var test in testsInvalid)
			{
				Assert.IsFalse(Validate.IsValidHtmlColor(test), test);
			}

		}

		[TestMethod()]
		public void IsValidTwitterUsernameTest()
		{
			string[] testsValid = new[]
			{
				"a",
				"1",
				"_a",
				"___",
				"123456789012345",
				"ABCDEfghijKLMNO"
			};

			foreach (var test in testsValid)
			{
				Assert.IsTrue(Validate.IsValidTwitterUsername(test), test);
			}


			string[] testsInvalid = new[]
			{
				null,
				"",
				" ",
				" abc ",
				"a\tc",
				"ábc",
				"abc-d",
				"1234567890123456",
				"ABCDEFGHIJKLMNOP"
			};

			foreach (var test in testsInvalid)
			{
				Assert.IsFalse(Validate.IsValidTwitterUsername(test), test);
			}


		}

		[TestMethod()]
		public void IsValidMailToAddressTest()
		{
			string[] testsValid = new[]
			{
				"me@abc.com",
				"me%test.local@w2.internet.local.google.com",
				"Marcelo <marcelo@calbucci.com>",
				"\"Calbucci, Marcelo\" <marcelo@calbucci.com>",
			};

			foreach (var test in testsValid)
			{
				Assert.IsTrue(Validate.IsValidMailToAddress(test), test);
			}

			string[] testsInvalid = new[]
			{
				"@abc.com",
				"Marcelo",
				"Marcelo <>",
				"Marcelo <abc@>",
				"Marcelo <abc@banana.com",
				"Marcelo marcelo@calbucci.com"
			};

			foreach (var test in testsInvalid)
			{
				Assert.IsFalse(Validate.IsValidMailToAddress(test), test);
			}
		}

		[TestMethod()]
		public void IsValidDomainTLDTest()
		{
			string[] testsValid = new[]
			{
				"com",
				"co",
				".com",
				".info",
				".biz",
				".party",
				"travel"
			};

			foreach (var test in testsValid)
			{
				Assert.IsTrue(Validate.IsValidDomainTLD(test), test);
			}

			string[] testsInvalid = new[]
			{
				"comb",
				"c",
				".com.br",
				".partee",
				"google.com"
			};

			foreach (var test in testsInvalid)
			{
				Assert.IsFalse(Validate.IsValidDomainTLD(test), test);
			}
		}

		[TestMethod()]
		public void IsValidGuidTest()
		{
			string[] testValids = new[]
			{
				"00000000000000000000000000000000",
				"00000000-0000-0000-0000-000000000000",
				"{ffffffff-ffff-ffff-ffff-ffffffffffff}",
				"{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}",
				"{704D7E33-360A-45F3-B0BC-6AC7ADE8B32E}",
				"(704D7E33-360A-45F3-B0BC-6AC7ADE8B32E)",
				"704D7E33-360A-45F3-B0BC-6AC7ADE8B32E",
				"704D7E33360A45F3B0BC6AC7ADE8B32E",
				"{0x704D7E33,0x360A,0x45F3,{0xB0,0xBC,0x6A,0xC7,0xAD,0xE8,0xB3,0x2E}}",
			};

			foreach (var test in testValids)
			{
				Assert.IsTrue(Validate.IsValidGuid(test), test);
			}

			string[] testInvalids = new[]
			{
				null,
				"",
				" ",
				"0000000000000000000000000000000", // 31 zeroes
				"{gfffffff-ffff-ffff-ffff-ffffffffffff}", // invalid character
				"{FFFFFFFF-FFFF-FFFF-FFFFFFFFFFFF}", // missing block
			};

			foreach (var test in testInvalids)
			{
				Assert.IsFalse(Validate.IsValidGuid(test), test);
			}

		}

		[TestMethod()]
		public void IsValidIPv4Test()
		{
			string[] testValids = new[]
			{
				"0.0.0.0",
				"1.1.1.1",
				"255.255.255.255",
				"192.168.1.1",
			};

			foreach (var test in testValids)
			{
				Assert.IsTrue(Validate.IsValidIPv4(test), test);
			}

			string[] testInvalids = new[]
			{
				null,
				"",
				"1.2",
				".1",
				"1.2.3",
				"1.2.3.",
				"256.1.1.1",
				"8f.1.1.1"
			};

			foreach (var test in testInvalids)
			{
				Assert.IsFalse(Validate.IsValidIPv4(test), test);
			}
			
		}

		[TestMethod()]
		public void IsValidIPv6Test()
		{
			string[] testValids = new[]
			{
				"2001:0db8:85a3:0000:0000:8a2e:0370:7334",
				"2001:db8:85a3:0:0:8a2e:370:7334",
				"2001:db8:85a3::8a2e:370:7334",
				"::ffff:192.0.2.12",
				"::",
				"::1",
				"fe80::"
			};

			foreach (var test in testValids)
			{
				Assert.IsTrue(Validate.IsValidIPv6(test), test);
			}

			string[] testInvalids = new[]
			{
				null,
				"",
				"1.2.3.4", // IP v4
				"f2001:db8:85a3::8a2e:370:7334", // extra digit
				"2001db885a38a2e3707334", // no separators
				"::g" // invalid character
			};

			foreach (var test in testInvalids)
			{
				Assert.IsFalse(Validate.IsValidIPv6(test), test);
			}
		}


		[TestMethod()]
		public void IsValidTimeTest()
		{
			string[] testValids = new[]
			{
				"1",
				"1p",
				"3:43",
				"3:43a",
				"19:37"
			};

			foreach (var test in testValids)
			{
				Assert.IsTrue(Validate.IsValidTime(test), test);
			}


			string[] testInvalids = new[]
			{
				null,
				"",
				"abc",
				"p",
				"am",
				"25",
				"3:67",
				"3:",
				":37"
			};

			foreach (var test in testInvalids)
			{
				Assert.IsFalse(Validate.IsValidTime(test), test);
			}

		}

		[TestMethod()]
		public void IsValidDateTest()
		{
			string[] testsValid = new []
			{
				"1/1/1",
				"12/31/2035",
				"1/31/2035",
				"2/29/2016"
			};

			foreach (var test in testsValid)
			{
				Assert.IsTrue(Validate.IsValidDate(test), test);
			}

			string[] testsInvalid = new[]
			{
				null,
				"",
				"1",
				"1/",
				"1/1",
				"1/1/",
				"-1/1/1",
				"13/13/2015",
				"12/31/-1",
				"2/29/2015",
				"7/31/5837"
			};

			foreach (var test in testsInvalid)
			{
				Assert.IsFalse(Validate.IsValidDate(test), test);
			}

		}
	}
}

