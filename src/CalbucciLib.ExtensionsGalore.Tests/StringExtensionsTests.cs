using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using CalbucciLib.ExtensionsGalore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalbucciLib.ExtensionsGalore.Tests
{
	[TestClass()]
	public class StringExtensionsTests
	{
		[TestMethod()]
		public void EscapeCStringTest()
		{
			string[] testStrings = new[]
			{
				null, null,
				"", "",
				" ", " ",
				"\t", "\\t",
				" \t\r\n ", " \\t\\r\\n ",
				"abc", "abc",
				"\rabc", "\\rabc",
				"abc\r", "abc\\r",
				"\a\b\t\n\v\f\r'\"\\", "\\a\\b\\t\\n\\v\\f\\r'\\\"\\\\",
			};

			for (int i = 0; i < testStrings.Length; i += 2)
			{
				string original = testStrings[i];
				string expected = testStrings[i + 1];
				string actual = original.EscapeCString();

				Assert.AreEqual(expected, actual, "Failed for expected: " + expected);
			}

		}

		[TestMethod()]
		public void TrimInBetweenTest()
		{
			string[] testStrings = new[]
			{
				" a", "a",
				null, null,
				"", "",
				" ", "",
				"a", "a",
				"a ", "a",
				"a b", "a b",
				"a  b", "a b",
				" a  b", "a b",
				"\r\n a \t\r\n b\r\n\t", "a b"
			};

			for (int i = 0; i < testStrings.Length; i += 2)
			{
				string original = testStrings[i];
				string expected = testStrings[i + 1];
				string actual = original.TrimInBetween();

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void TrimInBetween_Reference_Test()
		{
			string str = "abc def ghi";
			string str2 = str.TrimInBetween();

			Assert.IsTrue(object.ReferenceEquals(str, str2));
		}

		[TestMethod()]
		public void ToListFromCsvLineTest()
		{
			string[][] tests = new[]
			{
				new[] {"\\\",b", "\"", "b"},
				new string[] {null, null},
				new[] {"", null},
				new[] {",", "", ""},
				new[] {",,", "", "", ""},
				new[] {" ,  ,\t ", "", "", ""},

				new[] {"a", "a"},
				new[] {"a,b", "a", "b"},
				new[] {" a , b ", "a", "b"},
				new[] {"\ta,\tb", "a", "b"},
				new[] {"a,", "a", ""},
				new[] {",a", "", "a"},
				new[] {"\"a,b\",c", "a,b", "c"},

				new[] {"\"ab\",c", "ab", "c"},
				new[] {"a\\\"b,c", "a\"b", "c"},
			};

			foreach (var test in tests)
			{
				var row = test[0].ToListFromCsvLine();

				if (row == null)
				{
					Assert.IsNull(test[1], "test: " + test[0]);
				}
				else
				{
					Assert.AreEqual(row.Count, test.Length - 1, "test: " + test[0]);
					for (int i = 0; i < row.Count; i++)
					{
						Assert.AreEqual(row[i], test[i + 1], "test: " + test[0] + " - Item " + i);
					}
				}
			}


		}

		[TestMethod()]
		public void UnescapeCSVTest()
		{
			string[] tests = new string[]
			{
				"a", "a",
				null, null,
				"", "",
				" a ", "a",
				"\"a\"", "a",
				"  \"a\" ", "a",
				"\"\\n\"", "\n",
				"\" a \"", " a ",

				"\"a,b\\\",c", "a,b\",c", // malformed
			};

			for(int i = 0; i < tests.Length; i+=2)
			{
				string test = tests[i];
				string expected = tests[i + 1];
				string actual = tests[i].UnescapeCSVField();

				Assert.AreEqual(expected, actual, "Test: " + test);
			}
		}

		[TestMethod()]
		public void TransliterateTest()
		{
			string[] tests = new[]
			{
				null, null,
				"", "",
				"abc", "abc",
				"São Paulo é ótima", "São Paulo é ótima",
				"שָׁלוֹם", "shalvom",
				//"Ελλάς", "Greek",
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string test = tests[i];
				string expected = tests[i + 1];

				string actual = test.Transliterate();

				Assert.AreEqual(expected, actual, "String: " + test);
			}
		}

		[TestMethod()]
		public void GenerateLoremIpsumTest()
		{
			Assert.AreEqual(StringExtensions.GenerateLoremIpsum(0), "");
			Assert.AreEqual(StringExtensions.GenerateLoremIpsum(1), "Lorem");
			Assert.AreEqual(StringExtensions.GenerateLoremIpsum(5), "Lorem ipsum dolor sit amet");
			Assert.AreEqual(StringExtensions.GenerateLoremIpsum(6), "Lorem ipsum dolor sit amet, consectetur");
		}

		[TestMethod()]
		public void CreateTRTDTest()
		{
			Assert.AreEqual(StringExtensions.CreateTRTD(null), "");
			string[] empty = new string[0];
			Assert.AreEqual(StringExtensions.CreateTRTD(empty), "");
			Assert.AreEqual(StringExtensions.CreateTRTD(""), "<tr><td></td></tr>");
			Assert.AreEqual(StringExtensions.CreateTRTD("abc"), "<tr><td>abc</td></tr>");
			Assert.AreEqual(StringExtensions.CreateTRTD("abc", ""), "<tr><td>abc</td><td></td></tr>");
			Assert.AreEqual(StringExtensions.CreateTRTD("a", "b", "c"), "<tr><td>a</td><td>b</td><td>c</td></tr>");
			
		}

		[TestMethod()]
		public void UnescapeCStringTest()
		{
			string[] tests = new[]
			{
				null,
				"",
				"abc",
				"\t\r\n",
				"\\t\\r\\n",
				"\\\t"
			};

			foreach (var test in tests)
			{
				var expected = test;
				var escaped = expected.EscapeCString();
				var actual = escaped.UnescapeCString();

				Assert.AreEqual(expected, actual);
			}

		}

		[TestMethod()]
		public void EscapeJsonTest()
		{
			string[] tests = new[]
			{
				null, "null",
				"", "''",
				"abc", "'abc'",
				"a\"bc", "'a\"bc'",
				"a\'bc", "'a\\'bc'",
				"a\r\nb\tc", "'a\\r\\nb\\tc'"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].EscapeJson('\'');
				Assert.AreEqual(expected, actual);

			}
		}

		[TestMethod()]
		public void EscapeCSVTest()
		{
			string[] tests = new[]
			{
				null, null,
				"", "",
				" ", " ",
				"abc", "abc",
				" abc ", " abc ",
				"abc,def", "\"abc,def\"",
				"abc\\def", "\"abc\\\\def\"",
				"Mark \"One\"", "\"Mark \\\"One\\\"\""
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var test = tests[i];
				var expected = tests[i + 1];
				var actual = test.EscapeCSV();

				Assert.AreEqual(expected, actual, test);
			}

			Assert.AreEqual("\"\\r\\n\"", "\r\n".EscapeCSV(true));

			string[] tests2 = new[]
			{
				null,
				"",
				"abc",
				"abc,def",
				"abc\"def",
				"abc\'def"
			};

			foreach (var test in tests2)
			{
				var expected = test;
				var escaped = test.EscapeCSV();
				var actual = escaped.UnescapeCSVField();
				Assert.AreEqual(expected, actual);
			}

			Assert.AreEqual(" abc ", " abc ".UnescapeCSVField(false));
			Assert.AreEqual(" abc ", " \" abc ".UnescapeCSVField(false));

		}

		[TestMethod()]
		public void ContainsAnyTest()
		{
			string[][] tests = new[]
			{
				new[] {"Marcelo", "mar", "123", "rc1"},
				new[] {"Marcelo", "abc", "123", "elo"},
				new[] {"Marcelo", "abc", "rc", "7"},
				new[] {"Marcelo", "marcELO", "racelo", "7"},
				new[] {"Marcelo", "marcELO", "23", "7"},
				new[] {"Marcelo", "marcELO", "", null},
			};

			foreach (var testSet in tests)
			{
				string str = testSet[0];
				var matches = testSet.Skip(1).ToList();

				Assert.IsTrue(str.ContainsAny(matches));
			}


			string[][] negativeTests = new[]
			{
				new[] {"Marcelo", "mr", "123", "rc1"},
				new[] {"Marcelo", "abc", "123", "eo"},
				new[] {"Marcelo", "abc", "rEc", "7"},
				new[] {"Marcelo", "amarc", "", null},
			};

			foreach (var testSet in negativeTests)
			{
				string str = testSet[0];
				var matches = testSet.Skip(1).ToList();

				Assert.IsFalse(str.ContainsAny(matches));
			}

			string[] charTests = new[]
			{
				"a", "a",
				"abcd", "abcd",
				"abcd", "defg",
				"abcd", "efgc"
			};

			for (int i = 0; i < charTests.Length; i += 2)
			{
				var chars = charTests[i + 1].ToCharArray();
				Assert.IsTrue(charTests[i].ContainsAny(chars));
			}


		}

		[TestMethod()]
		public void StartsWithAnyTest()
		{
			string[][] tests = new[]
			{
				new[] {"Marcelo", "mar", "123", "rc1"},
				new[] {"Marcelo", "m", "123", "eo"},
				new[] {"Marcelo", "abc", "marcelo", "7"},
				new[] {"Marcelo", "cELO", "racelo", "MARCELO"},
			};

			foreach (var testSet in tests)
			{
				string str = testSet[0];
				var matches = testSet.Skip(1).ToList();

				Assert.IsTrue(str.StartsWithAny(matches));
			}


			string[][] negativeTests = new[]
			{
				new[] {"Marcelo", "mr", "123", "arcelo"},
				new[] {"Marcelo", "abc", "123", "lo"},
				new[] {"Marcelo", "abc", "rEc", "7"},
				new[] {"Marcelo", "amarc", "", null},
			};

			foreach (var testSet in negativeTests)
			{
				string str = testSet[0];
				var matches = testSet.Skip(1).ToList();

				Assert.IsFalse(str.StartsWithAny(matches));
			}
		}

		[TestMethod()]
		public void EndsWithAnyTest()
		{
			string[][] tests = new[]
			{
				new[] {"Marcelo", "mr", "123", "lo"},
				new[] {"Marcelo", "n", "elo", "eo"},
				new[] {"Marcelo", "rcelo", "marlo", "7"},
				new[] {"Marcelo", "cELO", "racelo", "MARCELO"},
			};

			foreach (var testSet in tests)
			{
				string str = testSet[0];
				var matches = testSet.Skip(1).ToList();

				Assert.IsTrue(str.EndsWithAny(matches));
			}


			string[][] negativeTests = new[]
			{
				new[] {"Marcelo", "mr", "123", "marcel"},
				new[] {"Marcelo", "abc", "123", "arce"},
				new[] {"Marcelo", "abc", "rEc", "7"},
				new[] {"Marcelo", "amarc", "", null},
			};

			foreach (var testSet in negativeTests)
			{
				string str = testSet[0];
				var matches = testSet.Skip(1).ToList();

				Assert.IsFalse(str.EndsWithAny(matches));
			}
		}

		[TestMethod()]
		public void CompareNonWhitespaceTest()
		{
			string[] tests = new string[]
			{
				null, null,
				"", "",
				"  ", "  ",
				"\t\r\n", "",
				"a", "a",
				" a", "a",
				"a ", " a ",
				"\ta\nb\rc", "abc",
				" A\t b \r C ", "\nab c"
			};

			string[] negativeTests = new string[]
			{
				"", "a",
				"A", "á",
				"a\ra", "a\rã"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var var1 = tests[i];
				var var2 = tests[i + 1];
				Assert.IsTrue(var1.CompareNonWhitespace(var2));
			}

			for (int i = 0; i < negativeTests.Length; i += 2)
			{
				var var1 = negativeTests[i];
				var var2 = negativeTests[i + 1];
				Assert.IsFalse(var1.CompareNonWhitespace(var2), "String: " + var1);
				
			}
		}

		[TestMethod()]
		public void LastIndexOfTest()
		{
			List<Tuple<string, Func<char, bool>, int>> tests = new List<Tuple<string, Func<char, bool>, int>>
			{
				Tuple.Create<string, Func<char, bool>, int>("efgb", c => "abc".IndexOf(c) >= 0, 3),
				Tuple.Create<string, Func<char, bool>, int>((string)null, c => "abc".IndexOf(c) >= 0, -1),
				Tuple.Create<string, Func<char, bool>, int>("", c => "abc".IndexOf(c) >= 0, -1),
				Tuple.Create<string, Func<char, bool>, int>("AAA", c => "abc".IndexOf(c) >= 0, -1),
				Tuple.Create<string, Func<char, bool>, int>("aaa", c => "abc".IndexOf(c) >= 0, 2),
				Tuple.Create<string, Func<char, bool>, int>("cde", c => "abc".IndexOf(c) >= 0, 0),
				Tuple.Create<string, Func<char, bool>, int>("cda", c => "abc".IndexOf(c) >= 0, 2),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var func = test.Item2;
				var expected = test.Item3;
				var actual = str.LastIndexOf(func);

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void TruncatePhraseTest()
		{
			List<Tuple<string, int, string>> tests = new List<Tuple<string, int, string>>
			{
				Tuple.Create((string)null, 0, (string)null),
				Tuple.Create((string)null, 10, (string)null),
				Tuple.Create((string)"", 10, (string)""),
				Tuple.Create((string)"a", 1, (string)"a"),
				Tuple.Create((string)"a", 10, (string)"a"),

				Tuple.Create((string)"This is a test", 1, (string)"T..."),
				Tuple.Create((string)"This is a test", 7, (string)"This..."),
				Tuple.Create((string)"This is a test", 8, (string)"This..."),
				Tuple.Create((string)"This is a test", 10, (string)"This is..."),
				Tuple.Create((string)"This is a test", 14, (string)"This is a test"),

				Tuple.Create((string)"This is a test ThisLastWordIsTooBigToBeTruncatedNeatlyAndWeJustBreakIt", 40, (string)"This is a test ThisLastWordIsTooBigTo..."),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var maxLength = test.Item2;
				var expected = test.Item3;
				var actual = test.Item1.TruncatePhrase(maxLength);

				Assert.AreEqual(expected, actual, "Test: " + str + "/" + maxLength);
			}

		}

		[TestMethod()]
		public void TruncateEllipsisTest()
		{
			List<Tuple<string, int, string>> tests = new List<Tuple<string, int, string>>
			{
				Tuple.Create((string)null, 0, (string)null),
				Tuple.Create((string)null, 10, (string)null),
				Tuple.Create((string)"", 10, (string)""),
				Tuple.Create((string)"a", 1, (string)"a"),
				Tuple.Create((string)"a", 10, (string)"a"),

				Tuple.Create((string)"This is a test", 1, (string)"T..."),
				Tuple.Create((string)"This is a test", 4, (string)"T..."),
				Tuple.Create((string)"This is a test", 5, (string)"Th..."),
				Tuple.Create((string)"This is a test", 8, (string)"This..."),
				Tuple.Create((string)"This is a test", 15, (string)"This is a test"),

			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var maxLength = test.Item2;
				var expected = test.Item3;
				var actual = test.Item1.TruncateEllipsis(maxLength);

				Assert.AreEqual(expected, actual, "Test: " + str + "/" + maxLength);
			}

		}

		[TestMethod()]
		public void TruncateTrimLinkTest()
		{
			List<Tuple<string, int, string>> tests = new List<Tuple<string, int, string>>
			{

				Tuple.Create((string)null, 0, (string)null),
				Tuple.Create((string)null, 10, (string)null),
				Tuple.Create((string)"", 10, (string)""),
				Tuple.Create((string)"a", 1, (string)"a"),
				Tuple.Create((string)"abcd", 1, (string)"a..."),

				Tuple.Create((string)"google.com", 1, (string)"g..."),

				Tuple.Create((string)"google.com/", 10, (string)"google.com"),
				Tuple.Create((string)"www.google.com", 10, (string)"google.com"),
				Tuple.Create((string)"www.google.com/search", 10, (string)"google.com"),

				Tuple.Create((string)"www.google.com/search", 17, (string)"google.com/search"),
				Tuple.Create((string)"www.google.com/search?hello=abc", 17, (string)"google.com/search"),
				Tuple.Create((string)"www.google.com/search?hello=abc", 26, (string)"google.com/search?hello..."),
				Tuple.Create((string)"www.google.com/search?hello=abc", 27, (string)"google.com/search?hello=abc"),
				Tuple.Create((string)"www.google.com/search?hello=abc", 40, (string)"google.com/search?hello=abc"),

				Tuple.Create((string)"www.google.com/search?hello=abc&", 40, (string)"google.com/search?hello=abc"),
				Tuple.Create((string)"www.google.com/search?hello=abc&&", 40, (string)"google.com/search?hello=abc"),
				Tuple.Create((string)"www.google.com/search?hello=abc&&delta", 40, (string)"google.com/search?hello=abc"),
				Tuple.Create((string)"www.google.com/search?hello=abc&&delta=", 40, (string)"google.com/search?hello=abc"),
				Tuple.Create((string)"www.google.com/search?hello=abc&&delta=def", 40, (string)"google.com/search?hello=abc&delta=def"),

				Tuple.Create((string)"https://www.google.com/search?hello=abc#32", 27, (string)"google.com/search?hello=abc"),

			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var maxLength = test.Item2;
				var expected = test.Item3;
				var actual = str.TruncateTrimLink(maxLength);

				Assert.AreEqual(expected, actual, "Test: " + str + " /" + maxLength);
			}
		}

		[TestMethod()]
		public void RemoveAccentsTest()
		{
			string[] tests = new[]
			{
				null, null,
				"", "",
				"  \t", "  \t",
				"Bob", "Bob",
				"Açucar", "Acucar",
				"Sãüñôr", "Saunor"
			};

			for (int i = 0; i < tests.Length; i+=2)
			{
				string test = tests[i];
				string expected = tests[i + 1];
				string actual = test.RemoveAccents();

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void CapitalizeFirstWordTest()
		{
			string[] tests = new[]
			{
				(string) null, (string) null,
				"", "",
				"a", "A",
				"A", "A",
				"álô", "Álô",
				"this is a test", "This is a test",
				"this\ris\na\ttest", "This\ris\na\ttest",
				"this,is.a-test", "This,is.a-test",
				"ALL CAPS", "ALL CAPS",
				"123hello", "123hello"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string test = tests[i];
				string expected = tests[i + 1];
				string actual = tests[i].CapitalizeFirstWord();

				Assert.AreEqual(expected, actual);
			}

		}

		[TestMethod()]
		public void CapitalizeAllWordsTest()
		{
			string[] tests = new[]
			{
				(string) null, (string) null,
				"", "",
				"a", "A",
				"A", "A",
				"álô", "Álô",
				"this is a test", "This Is A Test",
				"this\ris\na\ttest", "This\rIs\nA\tTest",
				"this,is.a-test", "This,Is.A-Test",
				"ALL CAPS", "ALL CAPS",
				"123hello", "123hello"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string test = tests[i];
				string expected = tests[i + 1];
				string actual = tests[i].CapitalizeAllWords();

				Assert.AreEqual(expected, actual);
			}
		}


		[TestMethod()]
		public void GetFirstWordTest()
		{
			string[] tests = new[]
			{
				(string) null, (string) null,
				"", "",
				"abc", "abc",
				"abc def", "abc",
				"abc,def", "abc",
				"abc123-def", "abc123",
				"This-is", "This",
				"Don't", "Don't",
				"Don't run", "Don't",
				"Donʼt run", "Donʼt",
				"  Don't run", "Don't",
				"--  Don't run", "Don't",
				"'car", "car",
				"car'", "car",
				"'can we'", "can",
				" 'can we' ", "can"

			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string test = tests[i];
				string expected = tests[i + 1];
				string actual = tests[i].GetFirstWord();

				Assert.AreEqual(expected, actual);
			}
			
		}

		[TestMethod()]
		public void GetLastWordTest()
		{
			string[] tests = new[]
			{
				(string) null, (string) null,
				"", "",
				"abc", "abc",
				"abc def", "def",
				"abc,def", "def",
				"abc123-def123", "def123",
				"This-is", "is",
				"Don't", "Don't",
				"Don't run", "run",
				"run donʼt", "donʼt",
				" run  Don't ", "Don't",
				"  run don't --", "don't",
				"'car", "car",
				"car'", "car",
				"'can we'", "we",
				" 'can we' ", "we"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string test = tests[i];
				string expected = tests[i + 1];
				string actual = tests[i].GetLastWord();

				Assert.AreEqual(expected, actual, test);
			}
		}

		[TestMethod()]
		public void ToBoolTest()
		{
			List<Tuple<string, bool>> tests = new List<Tuple<string, bool>>
			{
				Tuple.Create((string)null, false),
				Tuple.Create("", false),
				Tuple.Create("banana", false),
				Tuple.Create("false", false),

				Tuple.Create("1", true),
				Tuple.Create("yes", true),
				Tuple.Create("true", true),
				Tuple.Create("TRue", true),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expected = test.Item2;
				var actual = str.ToBool();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void ToLongTest()
		{
			List<Tuple<string, long>> tests = new List<Tuple<string, long>>
			{

				Tuple.Create<string,long>(null, 0),
				Tuple.Create<string,long>("", 0),
				Tuple.Create<string,long>("a123", 0),
				Tuple.Create<string,long>(",123", 0),
				Tuple.Create<string,long>("x", 0),
				Tuple.Create<string,long>("abc", 0),

				Tuple.Create<string,long>("127", 127),
				Tuple.Create<string,long>("  127", 127),
				Tuple.Create<string,long>("  127  ", 127),
				Tuple.Create<string,long>(" 127abc", 127),
				Tuple.Create<string,long>(" 127 123", 127),
				Tuple.Create<string,long>("127,", 127),

				Tuple.Create<string,long>("0X7f", 127),
				Tuple.Create<string,long>(" 0x7F ", 127),
				Tuple.Create<string,long>(" 0x7Fz ", 127),
				Tuple.Create<string,long>("0x7F", 127),


				Tuple.Create<string,long>("127,123", 127123),
				Tuple.Create<string,long>("987654321987654321", 987654321987654321),

				Tuple.Create<string,long>("-127", -127),
				Tuple.Create<string,long>(" -127", -127),
				Tuple.Create<string,long>(" - 127", -127),
				Tuple.Create<string,long>(" -127 ", -127),
				Tuple.Create<string,long>("127-", -127),
				Tuple.Create<string,long>("- 127.23", -127),

				Tuple.Create<string,long>(long.MaxValue.ToString(), long.MaxValue),
				Tuple.Create<string,long>(long.MinValue.ToString(), long.MinValue),
				Tuple.Create<string,long>("0x" + long.MaxValue.ToString("x"), long.MaxValue),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expected = test.Item2;
				var actual = test.Item1.ToLong();

				Assert.AreEqual(expected, actual, str);
			}
			
		}

		[TestMethod()]
		public void ToDoubleTest()
		{
			List<Tuple<string, double>> tests = new List<Tuple<string, double>>
			{
				Tuple.Create<string,double>(null, 0),
				Tuple.Create<string,double>("", 0),
				Tuple.Create<string,double>("a123", 0),
				Tuple.Create<string,double>(",123", 0),
				Tuple.Create<string,double>("x", 0),
				Tuple.Create<string,double>("abc", 0),

				Tuple.Create<string,double>("127", 127),
				Tuple.Create<string,double>("  127", 127),
				Tuple.Create<string,double>("  127  ", 127),
				Tuple.Create<string,double>(" 127abc", 127),
				Tuple.Create<string,double>(" 127 123", 127),
				Tuple.Create<string,double>("127,", 127),
				Tuple.Create<string,double>("127.00", 127),

				Tuple.Create<string,double>("127%", 1.27),
				Tuple.Create<string,double>("-127%", -1.27),

				Tuple.Create<string,double>("127,123", 127123),
				Tuple.Create<string,double>("987654321", 987654321),

				Tuple.Create<string,double>("-127", -127),
				Tuple.Create<string,double>(" -127", -127),
				Tuple.Create<string,double>(" - 127", -127),
				Tuple.Create<string,double>(" -127 ", -127),
				Tuple.Create<string,double>("127-", -127),

				Tuple.Create<string,double>("127.123", 127.123),
				Tuple.Create<string,double>("- 127.123", -127.123),
				Tuple.Create<string,double>("$-127.123", -127.123),
				Tuple.Create<string,double>("€ - 127.123", -127.123),
				Tuple.Create<string,double>("987654321.987654321", 987654321.987654321),

				Tuple.Create<string,double>("1.23e+1", 12.3),
				Tuple.Create<string,double>("-1.23e-1", -0.123),
				Tuple.Create<string,double>(" - 1.23e-1", -0.123),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expected = test.Item2;
				var actual = test.Item1.ToDouble();

				Assert.AreEqual(expected, actual, str);
			}
			
		}

		[TestMethod()]
		public void ToFloatTest()
		{
			List<Tuple<string, float>> tests = new List<Tuple<string, float>>
			{
				Tuple.Create<string,float>(null, 0),
				Tuple.Create<string,float>("", 0),
				Tuple.Create<string,float>("a123", 0),
				Tuple.Create<string,float>(",123", 0),
				Tuple.Create<string,float>("x", 0),
				Tuple.Create<string,float>("abc", 0),

				Tuple.Create<string,float>("127", 127),
				Tuple.Create<string,float>("  127", 127),
				Tuple.Create<string,float>("  127  ", 127),
				Tuple.Create<string,float>(" 127abc", 127),
				Tuple.Create<string,float>(" 127 123", 127),
				Tuple.Create<string,float>("127,", 127),
				Tuple.Create<string,float>("127.00", 127),

				Tuple.Create<string,float>("127%", (float)1.27),
				Tuple.Create<string,float>("-127%", (float)-1.27),

				Tuple.Create<string,float>("127,123", 127123),
				Tuple.Create<string,float>("1234567", 1234567),

				Tuple.Create<string,float>("-127", -127),
				Tuple.Create<string,float>(" -127", -127),
				Tuple.Create<string,float>(" - 127", -127),
				Tuple.Create<string,float>(" -127 ", -127),
				Tuple.Create<string,float>("127-", -127),

				Tuple.Create<string,float>("127.1", (float)127.1),
				Tuple.Create<string,float>("- 127.123", (float)-127.123),
				Tuple.Create<string,float>("$-127.123", (float)-127.123),
				Tuple.Create<string,float>("€ - 127.123", (float)-127.123),
				Tuple.Create<string,float>("0.1234567", (float)0.1234567),

				Tuple.Create<string,float>("1.23e+1", (float)12.3),
				Tuple.Create<string,float>("-1.23e-1", (float)-0.123),
				Tuple.Create<string,float>(" - 1.23e-1", (float)-0.123),
			};

			var floatPrecision = 0.00001;
			foreach (var test in tests)
			{
				var str = test.Item1;
				var expected = test.Item2;
				var actual = test.Item1.ToDouble();

				var delta = Math.Abs(expected - actual);
				
				Assert.IsTrue(delta <= floatPrecision, str);
			}
		}

		[TestMethod()]
		public void ToDecimalTest()
		{
			List<Tuple<string, decimal>> tests = new List<Tuple<string, decimal>>
			{
				Tuple.Create<string,decimal>(null, 0),
				Tuple.Create<string,decimal>("", 0),
				Tuple.Create<string,decimal>("a123", 0),
				Tuple.Create<string,decimal>(",123", 0),
				Tuple.Create<string,decimal>("x", 0),
				Tuple.Create<string,decimal>("abc", 0),

				Tuple.Create<string,decimal>("127", 127),
				Tuple.Create<string,decimal>("  127", 127),
				Tuple.Create<string,decimal>("  127  ", 127),
				Tuple.Create<string,decimal>(" 127abc", 127),
				Tuple.Create<string,decimal>(" 127 123", 127),
				Tuple.Create<string,decimal>("127,", 127),
				Tuple.Create<string,decimal>("127.00", 127),

				Tuple.Create<string,decimal>("127%", (decimal)1.27),
				Tuple.Create<string,decimal>("-127%", (decimal)-1.27),


				Tuple.Create<string,decimal>("127,123", 127123),
				Tuple.Create<string,decimal>("987654321987654321", 987654321987654321),

				Tuple.Create<string,decimal>("-127", -127),
				Tuple.Create<string,decimal>(" -127", -127),
				Tuple.Create<string,decimal>(" - 127", -127),
				Tuple.Create<string,decimal>(" -127 ", -127),
				Tuple.Create<string,decimal>("127-", -127),

				Tuple.Create<string,decimal>("127.123", (decimal)127.123),
				Tuple.Create<string,decimal>("- 127.123", (decimal)-127.123),
				Tuple.Create<string,decimal>("$-127.123", (decimal)-127.123),
				Tuple.Create<string,decimal>("€ - 127.123", (decimal)-127.123),
				Tuple.Create<string,decimal>("987654321.987654", (decimal)987654321.987654),

				Tuple.Create<string,decimal>("1.23e+1", (decimal)12.3),
				Tuple.Create<string,decimal>("-1.23e-1", (decimal)-0.123),
				Tuple.Create<string,decimal>(" - 1.23e-1", (decimal)-0.123),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expected = test.Item2;
				var actual = test.Item1.ToDecimal();

				Assert.AreEqual(expected, actual, str);
			}
		}

		[TestMethod()]
		public void TrimStartTest()
		{
			List<Tuple<string, Func<char, bool>, string>> tests = new List<Tuple<string, Func<char, bool>, string>>
			{
				Tuple.Create<string, Func<char, bool>, string>((string)"a b c", char.IsLetterOrDigit, (string)" b c"),
				Tuple.Create<string, Func<char, bool>, string>((string)null, char.IsLetterOrDigit, (string)null),
				Tuple.Create<string, Func<char, bool>, string>((string)"", char.IsLetterOrDigit, (string)""),
				Tuple.Create<string, Func<char, bool>, string>((string)" \t", char.IsLetterOrDigit, (string)" \t"),
				Tuple.Create<string, Func<char, bool>, string>((string)"abc", char.IsLetterOrDigit, (string)""),
				Tuple.Create<string, Func<char, bool>, string>((string)" a b c ", char.IsLetterOrDigit, (string)" a b c "),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expression = test.Item2;
				var expected = test.Item3;
				var actual = str.TrimStart(expression);

				Assert.AreEqual(expected, actual, str);
			}
		}

		[TestMethod()]
		public void TrimEndTest()
		{
			List<Tuple<string, Func<char, bool>, string>> tests = new List<Tuple<string, Func<char, bool>, string>>
			{
				Tuple.Create<string, Func<char, bool>, string>((string)null, char.IsLetterOrDigit, (string)null),
				Tuple.Create<string, Func<char, bool>, string>((string)"", char.IsLetterOrDigit, (string)""),
				Tuple.Create<string, Func<char, bool>, string>((string)" \t", char.IsLetterOrDigit, (string)" \t"),
				Tuple.Create<string, Func<char, bool>, string>((string)"abc", char.IsLetterOrDigit, (string)""),
				Tuple.Create<string, Func<char, bool>, string>((string)"a b c", char.IsLetterOrDigit, (string)"a b "),
				Tuple.Create<string, Func<char, bool>, string>((string)" a b c ", char.IsLetterOrDigit, (string)" a b c "),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expression = test.Item2;
				var expected = test.Item3;
				var actual = str.TrimEnd(expression);

				Assert.AreEqual(expected, actual, str);
			}
		}

		[TestMethod()]
		public void TrimTest()
		{
			List<Tuple<string, Func<char, bool>, string>> tests = new List<Tuple<string, Func<char, bool>, string>>
			{
				Tuple.Create<string, Func<char, bool>, string>((string)null, char.IsLetterOrDigit, (string)null),
				Tuple.Create<string, Func<char, bool>, string>((string)"", char.IsLetterOrDigit, (string)""),
				Tuple.Create<string, Func<char, bool>, string>((string)" \t", char.IsLetterOrDigit, (string)" \t"),
				Tuple.Create<string, Func<char, bool>, string>((string)"abc", char.IsLetterOrDigit, (string)""),
				Tuple.Create<string, Func<char, bool>, string>((string)"a b c", char.IsLetterOrDigit, (string)" b "),
				Tuple.Create<string, Func<char, bool>, string>((string)" a b c ", char.IsLetterOrDigit, (string)" a b c "),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expression = test.Item2;
				var expected = test.Item3;
				var actual = str.Trim(expression);

				Assert.AreEqual(expected, actual, str);
			}

		}

		[TestMethod()]
		public void ToIntTest()
		{
			List<Tuple<string, int>> tests = new List<Tuple<string, int>>
			{

				Tuple.Create<string,int>(null, 0),
				Tuple.Create<string,int>("", 0),
				Tuple.Create<string,int>("a123", 0),
				Tuple.Create<string,int>(",123", 0),
				Tuple.Create<string,int>("x", 0),
				Tuple.Create<string,int>("abc", 0),

				Tuple.Create<string,int>("127", 127),
				Tuple.Create<string,int>("  127", 127),
				Tuple.Create<string,int>("  127  ", 127),
				Tuple.Create<string,int>(" 127abc", 127),
				Tuple.Create<string,int>(" 127 123", 127),
				Tuple.Create<string,int>("127,", 127),

				Tuple.Create<string,int>("0X7f", 127),
				Tuple.Create<string,int>(" 0x7F ", 127),
				Tuple.Create<string,int>(" 0x7Fz ", 127),
				Tuple.Create<string,int>("0x7F", 127),


				Tuple.Create<string,int>("127,123", 127123),
				Tuple.Create<string,int>("987654321", 987654321),

				Tuple.Create<string,int>("-127", -127),
				Tuple.Create<string,int>(" -127", -127),
				Tuple.Create<string,int>(" - 127", -127),
				Tuple.Create<string,int>(" -127 ", -127),
				Tuple.Create<string,int>("127-", -127),
				Tuple.Create<string,int>("- 127.23", -127),

				Tuple.Create<string,int>(int.MaxValue.ToString(), int.MaxValue),
				Tuple.Create<string,int>(int.MinValue.ToString(), int.MinValue),
				Tuple.Create<string,int>("0x" + int.MaxValue.ToString("x"), int.MaxValue),
			};

			foreach (var test in tests)
			{
				var str = test.Item1;
				var expected = test.Item2;
				var actual = test.Item1.ToLong();

				Assert.AreEqual(expected, actual, str);
			}

		}

		[TestMethod()]
		public void HtmlEncodeTextareaTest()
		{
			string[] tests = new[]
			{
				"\r\n", "\r\n",
				null, "",
				"", "",
				"abc", "abc",
				"<abc>","&lt;abc&gt;",
			};

			for (int i = 0; i < tests.Length; i+=2)
			{
				string expected = tests[i + 1];
				string actual = tests[i].HtmlEncodeTextarea();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void HtmlDecodeTest()
		{
			string[] tests = new string[]
			{
				null, "",
				"", "",
				" abc\t", " abc\t",
				"&amp;", "&",
				"&lt", "&lt",
				"&lt;b&gt;", "<b>",
				"<b>", "<b>"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i+1];
				var actual = tests[i].HtmlDecode();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void UrlEncodeTest()
		{
			string[] tests = new string[]
			{
				null, "",
				"", "",
				" ", "+",
				"abc", "abc",
				"a=b", "a%3db",
				"a&b", "a%26b"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].UrlEncode();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void UrlDecodeTest()
		{
			string[] tests = new string[]
			{
				null, "",
				"", "",
				"+", " ",
				"abc", "abc",
				"a%3db", "a=b",
				"a%26b", "a&b"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].UrlDecode();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void EscapeStringFormatTest()
		{
			string[] tests = new string[]
			{
				null, null,
				"", "",
				" ", " ",
				"abc", "abc",
				"\r\nabc\t", "\r\nabc\t",
				"{0}", "{{0}",
				"hello{0:d}world{1:N}", "hello{{0:d}world{{1:N}"
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].EscapeStringFormat();
				Assert.AreEqual(expected, actual);
			}
			
		}

		[TestMethod()]
		public void EscapeCDATATest()
		{
			string[] tests = new[]
			{
				null, null,
				"", "",
				"abc", "abc",
				"<![that's cool","<![that's cool",
				"must escape]]>", "<![CDATA[must escape]]]]><![CDATA[>]]>",
				"must ]]> escape", "<![CDATA[must ]]]]><![CDATA[> escape]]>",
				"<![CDATA[double encoded]]>", "<![CDATA[<![CDATA[double encoded]]]]><![CDATA[>]]>"
			};
			for (int i = 0; i < tests.Length; i+=2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].EscapeCDATA();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void HasLowerCaseTest()
		{
			string[] tests = new[]
			{
				"a",
				"ç",
				"ABC123d",
				"44ñ88"
			};

			foreach (var test in tests)
			{
				Assert.IsTrue(test.HasLowerCase(), test);
			}


			string[] negativeTests = new[]
			{
				"A",
				"Ç",
				"ABC123D",
				"44Ñ88"
			};

			foreach (var test in negativeTests)
			{
				Assert.IsFalse(test.HasLowerCase(), test);
			}

		}

		[TestMethod()]
		public void HasUpperCaseTest()
		{
			string[] tests = new[]
			{
				"A",
				"Ç",
				"abc123D",
				"44Ñ88"
			};

			foreach (var test in tests)
			{
				Assert.IsTrue(test.HasUpperCase(), test);
			}


			string[] negativeTests = new[]
			{
				"a",
				"ç",
				"abc123d",
				"44ñ88"
			};

			foreach (var test in negativeTests)
			{
				Assert.IsFalse(test.HasUpperCase(), test);
			}
		}

		[TestMethod()]
		public void StartsWithAnyTest1()
		{
			Assert.IsTrue("Marcelo".StartsWithAnyCI("MARC"));
			Assert.IsTrue("Marcelo".StartsWithAnyCI("marc"));
			Assert.IsTrue("Marcelo".StartsWithAnyCI("abc", "marc"));
			Assert.IsTrue("Marcelo".StartsWithAnyCI("abc", "marc", "efg"));
		}

		[TestMethod()]
		public void IndexOfWhitespaceTest()
		{
			List<Tuple<string, int>> tests = new List<Tuple<string, int>>
			{
				Tuple.Create((string)null, -1),
				Tuple.Create("", -1),
				Tuple.Create(" ", 0),
				Tuple.Create("\t", 0),
				Tuple.Create("abc", -1),
				Tuple.Create("abc ", 3),
				Tuple.Create("abc \t", 3),
				Tuple.Create("abc \tabc ", 3),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.IndexOfWhitespace();
				Assert.AreEqual(expected, actual, test.Item1);
			}
		}

		[TestMethod()]
		public void LastIndexOfNonWhitespaceTest()
		{
			List<Tuple<string, int>> tests = new List<Tuple<string, int>>
			{
				Tuple.Create((string)null, -1),
				Tuple.Create("", -1),
				Tuple.Create(" ", -1),
				Tuple.Create("\t", -1),
				Tuple.Create("abc", 2),
				Tuple.Create("abc ", 2),
				Tuple.Create("abc \t", 2),
				Tuple.Create("abc \tabc ", 7),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.LastIndexOfNonWhitespace();
				Assert.AreEqual(expected, actual, test.Item1);
			}
		}

		[TestMethod()]
		public void TruncateTest()
		{
			List<Tuple<string, string>> tests = new List<Tuple<string, string>>
			{
				Tuple.Create((string)null, (string)null),
				Tuple.Create("", ""),
				Tuple.Create("MyNameIsBlaBla", "MyNameIsBl"),
				Tuple.Create("My Name Is Bla", "My Name Is"),
				Tuple.Create("MyNameIs    Bla", "MyNameIs"),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.Truncate(10);
				Assert.AreEqual(expected, actual);
			}
		}



		[TestMethod()]
		public void ToListFromTabDelimitedLineTest()
		{
			string[][] tests = new[]
			{
				new string[] {null, null},
				new[] {"", null},
				new[] {"\t", "", ""},
				new[] {"\t\t", "", "", ""},
				new[] {" \t  \t\\t ", " ", "  ", "\t "},
				new[] {"a\\tb", "a\tb"},

				new[] {"a", "a"},
				new[] {"\ta", "", "a"},
				new[] {"a\t", "a", ""},
				new[] {"a\tb", "a", "b"},
				new[] {" a \t b ", " a ", " b "},
				new[] {"\\ta\t\\tb", "\ta", "\tb"},
				new[] {"\"a\tb\"\tc", "a\tb", "c"},

				new[] {"\\\"\tb", "\"", "b"},
				new[] {"\"ab\"\tc", "ab", "c"},
				new[] {"a\\\"b\tc", "a\"b", "c"},
			};

			foreach (var test in tests)
			{
				var row = test[0].ToListFromTabDelimitedLine();

				if (row == null)
				{
					Assert.IsNull(test[1], "test: " + test[0]);
				}
				else
				{
					Assert.AreEqual(row.Count, test.Length - 1, "test: " + test[0]);
					for (int i = 0; i < row.Count; i++)
					{
						Assert.AreEqual(row[i], test[i + 1], "test: " + test[0] + " - Item " + i);
					}
				}
			}

		}
	}

}