using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CalbucciLib.ExtensionsGalore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CalbucciLib.ExtensionsGalore.Tests
{
	[TestClass()]
	public class ObjectExtensionsTests
	{
		private void TestSet(object[] tests)
		{
			foreach (var test in tests)
			{
				var copy = test.DeepCopy();
				Assert.AreEqual(test, copy, test != null ? test.ToString() : "null");

				if (test != null)
				{
					Assert.AreNotSame(test, copy, test.ToString());
					Assert.AreEqual(test.GetType(), copy.GetType());
				}
			}
		}

		private void TestIDictionary(IDictionary dictionary)
		{
			var copy = (IDictionary)((object)dictionary).DeepCopy() as IDictionary;

			Assert.AreNotSame(dictionary, copy);
			Assert.AreEqual(dictionary.GetType(), copy.GetType());
			Assert.AreEqual(dictionary.Count, copy.Count);

			foreach (DictionaryEntry item in dictionary)
			{
				bool found = false;
				foreach (var keyCopy in copy.Keys)
				{
					if (item.Key.Equals(keyCopy))
					{
						Assert.AreNotSame(keyCopy, item.Key);
						found = true;
						break;
					}
				}
				Assert.IsTrue(found);

				var valueCopy = copy[item.Key];

				Assert.AreEqual(item.Value, valueCopy);
				if (item.Value != null)
				{
					Assert.AreNotSame(item.Value, valueCopy);
					Assert.AreEqual(item.Value.GetType(), valueCopy.GetType());
				}
			}
		}

		private void TestIList(IList list)
		{
			// This casting is necessary so we don't call the ListExtension
			var copy = (IList)(((object)list).DeepCopy());

			Assert.AreEqual(list.GetType(), copy.GetType());
			Assert.AreNotSame(list, copy);
			Assert.AreEqual(list.Count, copy.Count);
			for (int i = 0; i < list.Count; i++)
			{
				var val1 = list[i];
				var val2 = copy[i];

				Assert.AreEqual(val1, val2);
				if (val1 != null)
				{
					Assert.AreNotSame(val1, val2);
					Assert.AreEqual(val1.GetType(), val2.GetType());
				}
			}
		}

		private void TestICollection(ICollection collection)
		{
			var copy = (ICollection)((object)collection).DeepCopy();

			Assert.AreEqual(collection.GetType(), copy.GetType());
			Assert.AreNotSame(collection, copy);
			Assert.AreEqual(collection.Count, copy.Count);

			var enumeratorCopy = copy.GetEnumerator();
			foreach (var item in collection)
			{
				enumeratorCopy.MoveNext();
				var itemCopy = enumeratorCopy.Current;
				Assert.AreEqual(item, itemCopy);
				if (item != null)
				{
					Assert.AreNotSame(item, itemCopy);
					Assert.AreEqual(item.GetType(), itemCopy.GetType());
				}
			}
			
		}

		[TestMethod()]
		public void DeepCopy_BaseType_Tests()
		{
			object[] tests = new object[]
			{
				null,
				'a',
				(byte)11,
				17,
				true,
				3.14,
				(decimal)6.28,
				(float)12.3
			};

			TestSet(tests);

		}

		[TestMethod()]
		public void DeepCopy_String_Tests()
		{
			string[] tests = new[]
			{
				null,
				"",
				"abc"
			};

			TestSet(tests);
		}

		struct TestStruct
		{
			public int a;
			public string s;

			public override bool Equals(object obj)
			{
				var rhs = (TestStruct)obj;
				return rhs.a == a && rhs.s == s;
			}
		}

		[TestMethod()]
		public void DeepCopy_Struct_Tests()
		{
			object[] tests = new object[]
			{
				Color.White,
				new TestStruct { a = 1, s = "a"}
			};

			TestSet(tests);			
		}


		[TestMethod()]
		public void DeepCopy_Array_Tests()
		{
			object[] tests = new object[]
			{
				new int[] { 1 },
				new int[] { 1, 2, 3},
				new double[] { 1.1, 2.2, 3.3},
				new byte[0],
				new string[] { "abc", "def"}
			};

			foreach (Array test in tests)
			{
				var copy = (Array)test.DeepCopy();
				Assert.AreEqual(test.GetType(), copy.GetType());
				Assert.AreEqual(test.Length, copy.Length);
				for (int i = 0; i < test.Length; i++)
				{
					Assert.AreEqual(test.GetValue(i), copy.GetValue(i));
					Assert.AreNotSame(test.GetValue(i), copy.GetValue(i));
				}
			}
	
		}

		[TestMethod()]
		public void DeepCopy_AnonymousType_Tests()
		{
			var anon = new {age = 17, name = "Peter", weight = 153.2};
			var copy = anon.DeepCopy();

			Assert.AreEqual(anon.GetType(), copy.GetType());
			Assert.AreEqual(anon, copy);
			Assert.AreNotSame(anon, copy);

			Assert.AreEqual(anon.age, copy.GetType().GetProperty("age").GetValue(copy));
			Assert.AreEqual(anon.name, copy.GetType().GetProperty("name").GetValue(copy));
			Assert.AreNotSame(anon.name, copy.GetType().GetProperty("name").GetValue(copy));
			Assert.AreEqual(anon.weight, copy.GetType().GetProperty("weight").GetValue(copy));
		}

		class TestClass
		{
			public string s;
			public int i;

			public override bool Equals(object obj)
			{
				var rhs = obj as TestClass;
				if (rhs == null)
					return false;
				return rhs.s == s && rhs.i == i;
			}
		}

		[TestMethod()]
		public void DeepCopy_Class_Tests()
		{
			object[] tests = {
				Tuple.Create("abc", 123, "def", 456),
				new TestClass {s = "ghi", i = 789}
			};

			TestSet(tests);
		}


		[TestMethod()]
		public void DeepCopy_ArrayList_Tests()
		{
			ArrayList al = new ArrayList();
			al.Add(123);
			al.Add("abc");
			al.Add(7.3);
			al.Add(true);

			TestIList(al);
		}

		[TestMethod()]
		public void DeepCopy_Queue_Tests()
		{
			Queue queue = new Queue();
			queue.Enqueue("abc");
			queue.Enqueue(123);
			queue.Enqueue(new { name = "Marcelo", city = "Seattle" });
			queue.Enqueue(Color.White);

			TestICollection(queue);
		}


		[TestMethod()]
		public void DeepCopy_Stack_Tests()
		{
			Stack stack = new Stack();
			stack.Push("abc");
			stack.Push(123);
			stack.Push(new { name = "Marcelo", city = "Seattle" });
			stack.Push(Color.White);

			TestICollection(stack);

		}


		[TestMethod()]
		public void DeepCopy_Hashtable_Tests()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["abc"] = "def";
			hashtable[123] = 456;
			hashtable["Record"] = new { name = "Marcelo", city = "Seattle" };
			hashtable["Color"] = Color.White;

			TestIDictionary(hashtable);

		}

		[TestMethod()]
		public void DeepCopy_BitArray_Tests()
		{
			Random r = new Random(3);
			byte[] bytes = new byte[7];
			r.NextBytes(bytes);
			BitArray bitArray = new BitArray(bytes);

			TestICollection(bitArray);

		}

		[TestMethod()]
		public void DeepCopy_SortedList_Tests()
		{
			SortedList sortedList = new SortedList();
			sortedList["not"] = null;
			sortedList["abc"] = "def";
			sortedList["number"] = 456;
			sortedList["Record"] = new { name = "Marcelo", city = "Seattle" };
			sortedList["Color"] = Color.White;

			TestIDictionary(sortedList);

		}

		[TestMethod()]
		public void DeepCopy_DictionaryT_Tests()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["abc"] = "def";
			dictionary["123"] = 456;
			dictionary["Record"] = new { name = "Marcelo", city = "Seattle" };
			dictionary["Color"] = Color.White;

			TestIDictionary(dictionary);


		}

		[TestMethod()]
		public void DeepCopy_ListT_Tests()
		{
			List<string> list = new List<string>()
			{
				null,
				"",
				"abc"
			};

			TestIList(list);
		}

		[TestMethod()]
		public void DeepCopy_HashSetT_Tests()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["abc"] = "def";
			dictionary["123"] = 456;
			dictionary["Record"] = new { name = "Marcelo", city = "Seattle" };
			dictionary["Color"] = Color.White;

			TestIDictionary(dictionary);
		}


		[TestMethod()]
		public void DeepCopy_LinkedListT_Tests()
		{
			LinkedList<TestClass> linkedList = new LinkedList<TestClass>();

			linkedList.AddLast(new LinkedListNode<TestClass>(null));
			linkedList.AddLast(new TestClass {i = 1, s = "a"});
			linkedList.AddLast(new TestClass { i = 2, s = "b" });

			TestICollection(linkedList);
		}


		[TestMethod()]
		public void DeepCopy_QueueT_Tests()
		{
			Queue<Color> queue = new Queue<Color>();
			queue.Enqueue(Color.Blue);
			queue.Enqueue(Color.Black);
			queue.Enqueue(Color.DarkGoldenrod);

			TestICollection(queue);
			
		}

		[TestMethod()]
		public void DeepCopy_StackT_Tests()
		{
			Stack<object> stack = new Stack<object>();

			stack.Push("abc");
			stack.Push(null);
			stack.Push(123);
			stack.Push(new { name = "Marcelo"});
			stack.Push(new TestClass { i = 7, s = "z"});

			TestICollection(stack);
		}


		[TestMethod()]
		public void DeepCopy_SortedDictionaryT_Tests()
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary["abc"] = "def";
			sortedDictionary["123"] = 456;
			sortedDictionary["Record"] = new { name = "Marcelo", city = "Seattle" };
			sortedDictionary["Color"] = Color.White;

			TestIDictionary(sortedDictionary);

		}


		[TestMethod()]
		public void DeepCopy_SortedSetT_Tests()
		{
			SortedSet<string> sortedSet = new SortedSet<string>()
			{
				null,
				"abc"
			};

			TestICollection(sortedSet);
		}


		[TestMethod()]
		public void DeepCopy_SortedListT_Tests()
		{
			SortedList<string, object> sortedList = new SortedList<string, object>();
			sortedList["abc"] = "def";
			sortedList["123"] = 456;
			sortedList["Record"] = new { name = "Marcelo", city = "Seattle" };
			sortedList["Color"] = Color.White;

			TestIDictionary(sortedList);
		}

		[TestMethod()]
		public void DeepCopy_ConcurrentDictionaryT_Tests()
		{
			ConcurrentDictionary<string, object> concurrentDictionary = new ConcurrentDictionary<string, object>();
			concurrentDictionary["abc"] = "def";
			concurrentDictionary["123"] = 456;
			concurrentDictionary["Record"] = new { name = "Marcelo", city = "Seattle" };
			concurrentDictionary["Color"] = Color.White;

			TestIDictionary(concurrentDictionary);

		}

		[TestMethod()]
		public void DeepCopy_ConcurrentQueueT_Tests()
		{
			ConcurrentQueue<Color> queue = new ConcurrentQueue<Color>();
			queue.Enqueue(Color.Blue);
			queue.Enqueue(Color.Black);
			queue.Enqueue(Color.DarkGoldenrod);

			TestICollection(queue);

		}

		[TestMethod()]
		public void DeepCopy_ConcurrentStackT_Tests()
		{
			ConcurrentStack<object> stack = new ConcurrentStack<object>();

			stack.Push("abc");
			stack.Push(null);
			stack.Push(123);
			stack.Push(new { name = "Marcelo" });
			stack.Push(new TestClass { i = 7, s = "z" });

			TestICollection(stack);

		}

		public void DeepCopy_DeepObject_Tests()
		{
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

			Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
			dictionary1["abc"] = "def";
			dictionary1["123"] = 456;
			dictionary1["Record"] = new { name = "Marcelo", city = "Seattle" };
			dictionary1["Color"] = Color.White;
			dictionary1["Class"] = new TestClass {i = 9, s = "k"};
			list.Add(dictionary1);

			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["ghi"] = Tuple.Create(1, "abc");
			dictionary2["789"] = new Stack<string>();
			((Stack<string>)dictionary2["789"]).Push("Push it");
			dictionary2["NewRecord"] = new { name = "Marcelo", city = "Seattle" };
			dictionary2["Struct"] = new TestStruct {a = 27, s = "t"};
			list.Add(dictionary2);

			var listCopy = (IList)(((object)list).DeepCopy());

			Assert.AreEqual(list.GetType(), listCopy.GetType());
			Assert.AreNotSame(list, listCopy);
			Assert.AreEqual(list.Count, listCopy.Count);

			for (int i = 0; i < list.Count; i++)
			{
				var dict = (IDictionary)list[i];
				var dictCopy = (IDictionary)listCopy[i];

				Assert.AreNotSame(dict, dictCopy);
				Assert.AreEqual(dict.GetType(), dictCopy.GetType());

				foreach (DictionaryEntry item in dict)
				{
					bool found = false;
					foreach (var keyCopy in dictCopy.Keys)
					{
						if (item.Key.Equals(keyCopy))
						{
							Assert.AreNotSame(keyCopy, item.Key);
							found = true;
							break;
						}
					}
					Assert.IsTrue(found);

					var valueCopy = dictCopy[item.Key];

					Assert.AreEqual(item.Value, valueCopy);
					if (item.Value != null)
					{
						Assert.AreNotSame(item.Value, valueCopy);
						Assert.AreEqual(item.Value.GetType(), valueCopy.GetType());
					}
				}
			}


		}


	}
}
