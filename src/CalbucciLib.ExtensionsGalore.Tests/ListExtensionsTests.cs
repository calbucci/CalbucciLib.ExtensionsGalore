using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalbucciLib.ExtensionsGalore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CalbucciLib.ExtensionsGalore.Tests
{
	[TestClass()]
	public class ListExtensionsTests
	{
		[TestMethod()]
		public void ToCSVLineTest()
		{
			List<string> test1 = new List<string>
			{
				"abc",
				null,
				"def",
				"",
				"z",
				" "
			};

			Assert.AreEqual(test1.ToCSVLine(), "abc,,def,,z, ");

			Assert.AreEqual((new List<int>()).ToCSVLine(), "");

			List<object> test2 = new List<object>
			{
				1,
				"abc",
				null,
				Color.White,
				27.3,
				"d\te",
				"M,C",
				"\r\n"
			};

			Assert.AreEqual(test2.ToCSVLine(), "1,abc,,Color [White],27.3,\"d\\te\",\"M,C\",\"\\r\\n\"");
		}

		[TestMethod()]
		public void ToTabSeparatedLineTest()
		{
			List<string> test1 = new List<string>
			{
				"abc",
				null,
				"def",
				"",
				"z",
				" "
			};

			Assert.AreEqual(test1.ToTabSeparatedLine(), "abc\t\tdef\t\tz\t ");

			Assert.AreEqual((new List<int>()).ToTabSeparatedLine(), "" );

			List<object> test2 = new List<object>
			{
				1,
				"abc",
				null,
				Color.White,
				27.3,
				"d\te",
				"\r\n"
			};

			Assert.AreEqual(test2.ToTabSeparatedLine(), "1\tabc\t\tColor [White]\t27.3\td\\te\t\\r\\n");

			
		}

		[TestMethod()]
		public void RandomizeTest()
		{
			List<int> original = new List<int>();
			for (int i = 0; i < 1000; i++)
			{
				original.Add(i);
			}

			var randomized = original.Randomize(486);

			Assert.AreNotSame(original, randomized);
			Assert.IsTrue(original.IsEqualUnordered(randomized));
			Assert.AreNotEqual(randomized[0], original[0]);
			Assert.AreNotEqual(randomized[999], original[999]);

			int consecutive = 0;
			for (int i = 0; i < 999; i++)
			{
				if (randomized[i] == randomized[i + 1])
				{
					consecutive++;
					// No more than 3 consecutive numbers
					Assert.IsTrue(consecutive < 3);
				}
				else
				{
					consecutive = 0;
				}
			}

			int samePos = 0;
			for (int i = 0; i < 999; i++)
			{
				if (randomized[i] == i)
				{
					samePos++;
				}
			}
			Assert.IsTrue(samePos < 10, samePos.ToString());

		}

		[TestMethod()]
		public void DeepCopyTest()
		{
			List<object> original = new List<object>();

			object number = 1;
			original.Add(number);
			original.Add("hello world");
			original.Add(new List<string>
			{
				"abc",
				"def"
			});

			var copy = original.DeepCopy();

			Assert.AreEqual(original.Count, copy.Count);
			for (int i = 0; i < original.Count; i++)
			{
				var objOriginal = original[i];
				var objCopy = copy[i];

				if (objOriginal is IList)
				{
					var listOriginal = objOriginal as IList;
					var listCopy = objCopy as IList;

					Assert.IsNotNull(listCopy);
					Assert.AreEqual(listOriginal.Count, listCopy.Count);

					Assert.IsTrue(listOriginal.IsEqualOrdered(listCopy));
				}
				else
				{
					Assert.AreEqual(objOriginal, objCopy, objOriginal.ToString());
					Assert.AreNotSame(objOriginal, objCopy, objOriginal != null ? objOriginal.ToString() : "null");
				}
			}

		}

		[TestMethod()]
		public void GetPaginationTest()
		{
			for (int i = 0; i < 20; i++)
			{
				List<int> items = new List<int>();
				for (int j = 0; j < i; j++)
				{
					items.Add(j);
				}

				for (int resultsPerPage = 1; resultsPerPage < 21; resultsPerPage++)
				{
					var pageNumber = 0;
					var page = items.GetPagination(resultsPerPage, ref pageNumber);
					Assert.IsTrue(page.Count <= resultsPerPage);
					Assert.IsTrue(page.IsEqualOrdered(items.Take(resultsPerPage).ToList()));

					pageNumber = 99; // last page
					page = items.GetPagination(resultsPerPage, ref pageNumber);
					Assert.AreNotEqual(pageNumber, 99, i + " / " + resultsPerPage);
					Assert.IsTrue(page.Count <= resultsPerPage);
					if (items.Count > 0)
					{
						Assert.IsTrue(page.Count > 0, i + " / " + resultsPerPage);
						Assert.IsTrue(page.IsEqualOrdered(items.Skip(items.Count - page.Count).Take(page.Count).ToList()));
					}
				}
			}
		}

		[TestMethod()]
		public void GetRandomItemTest()
		{
			Random r = new Random(987);
			var items = new List<int>();
			for (int i = 0; i < 1000; i++)
			{
				items.Add(i);
			}

			int v1, v2;
			v1 = v2 = -1;
			for (int i = 0; i < 20; i++)
			{
				v1 = items.GetRandomItem(r);
				Assert.AreNotEqual(v1, v2);
				v2 = v1;
			}


			// Test distribution: We expect a more or less uniform distribution
			int distributionItemToTest = 10;
			int totalTests = 1000;
			items = items.GetRange(0, distributionItemToTest);
			var distribution = new List<int>();
			for (int i = 0; i < distributionItemToTest; i++)
			{
				distribution.Add(0);
			}

			for (int i = 0; i < totalTests; i++)
			{
				var item = items.GetRandomItem(r);
				distribution[item] = distribution[item] + 1;
			}

			int minimumCount = (totalTests/distributionItemToTest ) / 2;
			for (int i = 0; i < 10; i++)
			{
				Assert.IsTrue(distribution[i] >= minimumCount, i.ToString());
			}

		}

		[TestMethod()]
		public void IsEqualUnorderedTest()
		{
			List<List<int>> testsEqual = new List<List<int>>
			{
				new List<int>(), new List<int>(),
				new List<int>{1}, new List<int>{1},
				new List<int>{1,2,3}, new List<int>{1,2,3},
				new List<int>{1,2,3}, new List<int>{1,3,2},
				new List<int>{1,2,3}, new List<int>{3,1,2},
				new List<int>{1,2,3}, new List<int>{3,2,1},
			};

			for (int i = 0; i < testsEqual.Count; i += 2)
			{
				var list1 = testsEqual[i];
				var list2 = testsEqual[i + 1];
				Assert.IsTrue(list1.IsEqualUnordered(list2));
				Assert.IsTrue(list2.IsEqualUnordered(list1));
				Assert.IsTrue(list1.IsEqualUnordered(list1));
			}

			List<List<int>> testsNotEqual = new List<List<int>>
			{
				new List<int>{1}, new List<int>(),
				new List<int>{1}, new List<int>{2},
				new List<int>{1,2,3}, new List<int>{1,2},
				new List<int>{1,2,3}, new List<int>{1,3},
				new List<int>{1,2,3}, new List<int>{3,2},
			};

			for (int i = 0; i < testsNotEqual.Count; i += 2)
			{
				var list1 = testsNotEqual[i];
				var list2 = testsNotEqual[i + 1];
				Assert.IsFalse(list1.IsEqualUnordered(list2));
				Assert.IsFalse(list2.IsEqualUnordered(list1));
			}


		}

		[TestMethod()]
		public void IsEqualSameOrderTest()
		{
			List<List<int>> testsEqual = new List<List<int>>
			{
				new List<int>(), new List<int>(),
				new List<int>{1}, new List<int>{1},
				new List<int>{1,2,3}, new List<int>{1,2,3},
			};

			for (int i = 0; i < testsEqual.Count; i += 2)
			{
				var list1 = testsEqual[i];
				var list2 = testsEqual[i + 1];
				Assert.IsTrue(list1.IsEqualOrdered(list2));
				Assert.IsTrue(list2.IsEqualOrdered(list1));
				Assert.IsTrue(list1.IsEqualOrdered(list1));
			}

			List<List<int>> testsNotEqual = new List<List<int>>
			{
				new List<int>{1}, new List<int>(),
				new List<int>{1}, new List<int>{2},
				new List<int>{1,2,3}, new List<int>{1,3,2},
				new List<int>{1,2,3}, new List<int>{2,1,3},
				new List<int>{1,2,3}, new List<int>{3,2,1},
			};

			for (int i = 0; i < testsNotEqual.Count; i += 2)
			{
				var list1 = testsNotEqual[i];
				var list2 = testsNotEqual[i + 1];
				Assert.IsFalse(list1.IsEqualOrdered(list2));
				Assert.IsFalse(list2.IsEqualOrdered(list1));
			}

			
		}
	}
}
