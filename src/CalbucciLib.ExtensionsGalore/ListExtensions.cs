using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	public static class ListExtensions
	{
		// ====================================================================
		//
		//    Conversion
		//
		// ====================================================================
		private static string ToSeparatedLine<T>(this IList<T> fields, string separator, bool removeEmptyEntries,
			Func<string, string> conversionFunc)
		{
			if (fields == null || fields.Count == 0)
				return "";

			string[] fieldStrings = new string[fields.Count];

			int i = 0;
			foreach (var field in fields)
			{
				string fieldString = "";
				if (field == null)
				{
					if (removeEmptyEntries)
						continue;
				}
				else
				{
					fieldString = field.ToString();
					if (removeEmptyEntries && string.IsNullOrWhiteSpace(fieldString))
						continue;
				}

				fieldStrings[i++] = conversionFunc(fieldString);
			}
			if (i == 0)
				return "";

			return string.Join(separator, fieldStrings.Take(i));			
		}

		public static string ToCSVLine<T>(this IList<T> fields, bool removeEmptyEntries = false)
		{
			return ToSeparatedLine(fields, ",", removeEmptyEntries, s => StringExtensions.EscapeCSV(s, true));
		}

		public static string ToTabSeparatedLine<T>(this IList<T> fields, bool removeEmptyEntries = false)
		{
			return ToSeparatedLine(fields, "\t", removeEmptyEntries, s => StringExtensions.EscapeTabDelimited(s, true));
		}

	    public static string ToString<T>(this IList<T> items, string separator, string lastSeparator = null)
	    {
	        if (items == null)
	            return "";

	        var nonNullItems = items.Where(item => item != null).ToList();

            if(nonNullItems.Count == 0)
	            return "";

	        if (nonNullItems.Count == 1)
	            return nonNullItems[0].ToString();

	        if (nonNullItems.Count == 2)
	            return string.Concat(nonNullItems[0], lastSeparator, nonNullItems[1]);

	        if (lastSeparator == null)
	            lastSeparator = separator;

	        if (typeof (T) == typeof (string))
	        {
	            int totalLength = 0;
	            foreach (var item in nonNullItems)
	            {
	                totalLength = (item as string).Length + separator.Length;
	            }
	            totalLength += lastSeparator.Length - separator.Length;

                StringBuilder sb = new StringBuilder(totalLength);
	            for (int i = 0; i < nonNullItems.Count; i++)
	            {
	                if (i == nonNullItems.Count - 1)
	                {
	                    sb.Append(lastSeparator);
	                }
                    else if (i > 0)
                    {
                        sb.Append(separator);
                    }
	                sb.Append(nonNullItems[i]);
	            }
	            return sb.ToString();
	        }
	        else
	        {
                int totalLength = 0;
                List<string> stringList = new List<string>(nonNullItems.Count);
	            foreach (var item in nonNullItems)
	            {
	                var stringItem = item.ToString();
                    stringList.Add(stringItem);
                    totalLength = stringItem.Length + separator.Length;
                }
                totalLength += lastSeparator.Length - separator.Length;

                StringBuilder sb = new StringBuilder(totalLength);
                for (int i = 0; i < stringList.Count; i++)
                {
                    if (i == stringList.Count - 1)
                    {
                        sb.Append(lastSeparator);
                    }
                    else if (i > 0)
                    {
                        sb.Append(separator);
                    }
                    sb.Append(stringList[i]);
                }
                return sb.ToString();

            }

        }
		

		// ====================================================================
		//
		//   Manipulation of content
		//
		// ====================================================================
		public static List<T> Randomize<T>(this IList<T> list, int seed = -1)
		{
			if (list == null || list.Count == 0)
				return new List<T>();

		    if (list.Count == 1)
		        return new List<T>(list);

			Random rng = (seed >= 0) ? new Random(seed) : new Random();

			var newList = new List<T>(list.Count);
			newList.AddRange(list);

			int n = newList.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = newList[k];
				newList[k] = newList[n];
				newList[n] = value;
			}
			return newList;
		}

		public static List<T> DeepCopy<T>(this IList<T> list, bool removeEmptyEntries = false) where T : class
		{
			if (list == null)
				return null;
			if (list.Count == 0)
				return new List<T>(list);

			var newList = new List<T>(list.Count);
			foreach (var item in list)
			{
				if (item == null)
				{
					if (!removeEmptyEntries)
						newList.Add(null);
					continue;
				}

				var newItem = (T)item.DeepCopy();

				newList.Add(newItem);
			}
			return newList;
		}

		/// <summary>
		/// Results a subset of the list for that pageNumber (0-based index), assuming resultsPerPage as the number of items per page.
		/// </summary>
		public static List<T> GetPagination<T>(this IList<T> list, int resultsPerPage, ref int pageNumber)
		{
			int maxPages = 0;
			if (list == null)
			{
				pageNumber = 0;
				return null;
			}
			if (list.Count == 0)
			{
				pageNumber = 0;
				return new List<T>();
			}

			maxPages = 1 + (list.Count - 1) / resultsPerPage;

			if (pageNumber >= maxPages)
				pageNumber = maxPages - 1;
			if (pageNumber < 0)
				pageNumber = 0;

			int start = pageNumber * resultsPerPage;
			int end = start + resultsPerPage;
			if (end > list.Count)
				end = list.Count;

			return list.Skip(start).Take(end - start).ToList();
		}


		// ====================================================================
		//
		//   Item Results
		//
		// ====================================================================
		public static T GetRandomItem<T>(this IList<T> list, Random randomGenerator = null)
		{
			if (list == null || list.Count == 0)
				return default(T);

			if (list.Count == 1)
				return list[0];

			if(randomGenerator == null)
				randomGenerator = new Random();
			return list[randomGenerator.Next(list.Count)];
		}


		// ====================================================================
		//
		//   Comparison
		//
		// ====================================================================
		public static bool IsEqualUnordered<T>(this IList<T> list, IList<T> secondList)
		{
			if (list == null || list.Count == 0)
			{
				if (secondList == null || secondList.Count == 0)
					return true;
				return false;
			}
			else if (secondList == null || secondList.Count == 0)
			{
				return false;
			}

			if (list.Count != secondList.Count)
				return false;

			//TODO: Use IEquatable if available, IComparer

			bool[] firstListMatch = new bool[list.Count];
			foreach (var item in secondList)
			{
				var pos = list.IndexOf(item);
				if (pos == -1)
					return false;

				if (!firstListMatch[pos])
				{
					firstListMatch[pos] = true;
					continue;
				}

				if (pos == list.Count - 1)
					return false;

				// Already found it once, maybe there are multiple instances of the same item
				do
				{
					pos++;
					while (pos < list.Count)
					{
						// TODO: Support IList<T>, List, ICollection
						if (list[pos].Equals(item))
							break;
						pos++;
					}
					if (pos == list.Count)
						return false;
					
				} while (firstListMatch[pos] && pos < list.Count - 1);
				firstListMatch[pos] = true;
			}

			return true;
		}

		public static bool IsEqualOrdered(this IList list, IList secondList)
		{
			if (list == null || list.Count == 0)
			{
				if (secondList == null || secondList.Count == 0)
					return true;
				return false;
			}
			else if (secondList == null || secondList.Count == 0)
				return false;

			if (list.Count != secondList.Count)
				return false;

			for (int i = 0; i < list.Count; i++)
			{
				var item1 = list[i];
				var item2 = secondList[i];
				if (item1 == null)
				{
					if (item2 == null)
						continue;
					return false;
				}
				else if (item2 == null)
					return false;

				if (item1 is IList)
				{
					if (!IsEqualOrdered(item1 as IList, item2 as IList))
						return false;
				}

				if (!item1.Equals(item2))
					return false;
			}
			return true;
		}

	}
}
