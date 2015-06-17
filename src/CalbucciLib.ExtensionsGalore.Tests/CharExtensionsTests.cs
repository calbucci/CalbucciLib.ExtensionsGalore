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
	public class CharExtensionsTests
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			// Init the extension
			CharExtensions.IsVowel('a');
		}

		[TestMethod()]
		public void IsConsonantTest()
		{
			string consonants = "bcdywxz";
			foreach (char c in consonants)
			{
				Assert.IsTrue(c.IsConsonant(), "Char: " + c);
			}

			string notConsonants = "aeiouáéíõü12 \t";
			foreach (char c in notConsonants)
			{
				Assert.IsFalse(c.IsConsonant(), "Char: " + c);
			}
		}

		[TestMethod()]
		public void IsVowelTest()
		{
			string vowels = "aeiouáéíõü";
			foreach (char c in vowels)
			{
				Assert.IsTrue(c.IsVowel(), "Char: " + c);
			}

			string notVowels = "bcdywxz1 ";
			foreach (char c in notVowels)
			{
				Assert.IsFalse(c.IsVowel(), "Char: " + c);
			}
		}


		[TestMethod()]
		public void RemoveAccentTest()
		{
			string original = "áaàãüúö";
			string noAccent = "aaaauuo";

			for (int i = 0; i < original.Length; i++)
			{
				char c1 = original[i];
				char expected = noAccent[i];

				char actual = c1.RemoveAccent();

				Assert.AreEqual(expected, actual, "Char: " + c1);
			}

		}


	}
}
