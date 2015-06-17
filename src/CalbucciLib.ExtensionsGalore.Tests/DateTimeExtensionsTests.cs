using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using CalbucciLib.ExtensionsGalore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CalbucciLib.ExtensionsGalore.Tests
{
	[TestClass()]
	public class DateTimeExtensionsTests
	{
		[TestMethod()]
		public void ParseTimeTest()
		{
			List<Tuple<string, DateTime?>> tests = new List<Tuple<string, DateTime?>>
			{
				Tuple.Create<string, DateTime?>("7a", new DateTime(1, 1, 1, 7, 0, 0)),
				Tuple.Create<string, DateTime?>("3pm", new DateTime(1, 1, 1, 15, 0, 0)),
				Tuple.Create<string, DateTime?>("1:37", new DateTime(1, 1, 1, 1, 37, 0)),
				Tuple.Create<string, DateTime?>("13:28", new DateTime(1, 1, 1, 13, 28, 0)),
				Tuple.Create<string, DateTime?>("0:0", new DateTime(1, 1, 1, 0, 0, 0)),
				Tuple.Create<string, DateTime?>("2:49:21", new DateTime(1, 1, 1, 2, 49, 21)),
				Tuple.Create<string, DateTime?>("15:07:14", new DateTime(1, 1, 1, 15, 7, 14)),
				Tuple.Create<string, DateTime?>("2am", new DateTime(1, 1, 1, 2, 0, 0)),
				Tuple.Create<string, DateTime?>("1:27 p.m.", new DateTime(1, 1, 1, 13, 27, 0)),
				Tuple.Create<string, DateTime?>("2:32:43 PM", new DateTime(1, 1, 1, 14, 32, 43)),

				// Invalid
				Tuple.Create<string, DateTime?>("", null),
				Tuple.Create<string, DateTime?>("-1:32:43 PM", null),
				Tuple.Create<string, DateTime?>("25:32:43 PM", null),
				Tuple.Create<string, DateTime?>("10:61", null),
				Tuple.Create<string, DateTime?>("10:-1", null),
				Tuple.Create<string, DateTime?>("10:01:-27", null),
				Tuple.Create<string, DateTime?>("10:01:93", null),
			};

			foreach (var test in tests)
			{
				string time = test.Item1;
				var expected = test.Item2;
				var actual = DateTimeExtensions.ParseTime(time);

				Assert.AreEqual(expected, actual, "Time: " + time);
			}

		}

		[TestMethod()]
		public void ToUnixTimeTest()
		{

			DateTime dt = new DateTime(1970, 1, 1);
			var unixTime = dt.ToUnixTime();
			Assert.AreEqual(unixTime, (long)0);


		}

		[TestMethod()]
		public void IsBetweenTest()
		{
			DateTime now = DateTime.Now;

			Assert.IsTrue(now.IsBetween(now.AddDays(1), now.AddDays(-1)));
			Assert.IsTrue(now.IsBetween(now.AddMilliseconds(-1), now.AddMilliseconds(1)));
		}

		[TestMethod()]
		public void GetFirstDayOfMonthTest()
		{
			Assert.AreEqual(new DateTime(2015, 5, 15).GetFirstDayOfMonth(), new DateTime(2015, 5, 1));
			Assert.AreEqual(new DateTime(2010, 1, 1).GetFirstDayOfMonth(), new DateTime(2010, 1, 1));
		}

		[TestMethod()]
		public void GetFirstDayOfPreviousMonthTest()
		{
			Assert.AreEqual(new DateTime(2015, 5, 15).GetFirstDayOfPreviousMonth(), new DateTime(2015, 4, 1));
			Assert.AreEqual(new DateTime(2010, 1, 1).GetFirstDayOfPreviousMonth(), new DateTime(2009, 12, 1));
		}

		[TestMethod()]
		public void GetFirstDayOfNextMonthTest()
		{
			Assert.AreEqual(new DateTime(2015, 5, 15).GetFirstDayOfNextMonth(), new DateTime(2015, 6, 1));
			Assert.AreEqual(new DateTime(2010, 12, 1).GetFirstDayOfNextMonth(), new DateTime(2011, 1, 1));
		}

		[TestMethod()]
		public void GetFirstDayOfMonthTest1()
		{
			Assert.AreEqual(new DateTime(2015, 5, 15).GetFirstDayOfMonth(DayOfWeek.Friday), new DateTime(2015, 5, 1));
			Assert.AreEqual(new DateTime(2015, 5, 15).GetFirstDayOfMonth(DayOfWeek.Monday), new DateTime(2015, 5, 4));
			Assert.AreEqual(new DateTime(2014, 2, 1).GetFirstDayOfMonth(DayOfWeek.Monday), new DateTime(2014, 2, 3));

		}

		[TestMethod()]
		public void ToRelativeTimeTest()
		{
			DateTime baseDate = DateTime.UtcNow;
			List<Tuple<DateTime, string>> tests = new List<Tuple<DateTime, string>>
			{
				Tuple.Create(baseDate, "now"),
				Tuple.Create(baseDate.AddSeconds(-1), "one second ago"),
				Tuple.Create(baseDate.AddSeconds(-10), "10 seconds ago"),
				Tuple.Create(baseDate.AddSeconds(-90), "one minute ago"),
				Tuple.Create(baseDate.AddSeconds(-90), "one minute ago"),
				Tuple.Create(baseDate.AddSeconds(-120), "2 minutes ago"),
				Tuple.Create(baseDate.AddMinutes(-7), "7 minutes ago"),
				Tuple.Create(baseDate.AddHours(-1), "one hour ago"),
				Tuple.Create(baseDate.AddHours(-9), "9 hours ago"),
				Tuple.Create(baseDate.AddHours(-24), "one day ago"),
				Tuple.Create(baseDate.AddDays(-12), "12 days ago"),
				Tuple.Create(baseDate.AddMonths(-1), "one month ago"),

				Tuple.Create(baseDate.AddSeconds(1), "in one second"),
				Tuple.Create(baseDate.AddMinutes(10), "in 10 minutes"),
				Tuple.Create(baseDate.AddHours(23), "in 23 hours"),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.ToRelativeTime(baseDate);
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void GetFirstMondayOfMonthTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 1, 1), new DateTime(2015, 1, 5), 
				new DateTime(2015, 1, 1, 13, 27, 35), new DateTime(2015, 1, 5), 
				new DateTime(2015, 3, 31, 23, 59, 59), new DateTime(2015, 3, 2), 
				new DateTime(2015, 4, 1), new DateTime(2015, 4, 6), 
				new DateTime(2015, 11, 16), new DateTime(2015, 11, 2), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetFirstMondayOfMonth();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void GetFirstDayOfQuarterTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 1, 1), new DateTime(2015, 1, 1), 
				new DateTime(2015, 1, 1, 13, 27, 35), new DateTime(2015, 1, 1), 
				new DateTime(2015, 3, 31, 23, 59, 59), new DateTime(2015, 1, 1), 
				new DateTime(2015, 4, 1), new DateTime(2015, 4, 1), 
				new DateTime(2015, 11, 16), new DateTime(2015, 10, 1), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetFirstDayOfQuarter();
				Assert.AreEqual(expected, actual);
			}


			Tuple<DateTime, DayOfWeek, DateTime>[] tests2 = new Tuple<DateTime, DayOfWeek, DateTime>[]
			{
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Monday, new DateTime(2015, 1, 5)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Tuesday, new DateTime(2015, 1, 6)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Wednesday, new DateTime(2015, 1, 7)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Thursday, new DateTime(2015, 1, 1)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Friday, new DateTime(2015, 1, 2)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Saturday, new DateTime(2015, 1, 3)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Sunday, new DateTime(2015, 1, 4)),

				Tuple.Create(new DateTime(2015, 3, 15), DayOfWeek.Wednesday, new DateTime(2015, 1, 7)),
				Tuple.Create(new DateTime(2015, 4, 10), DayOfWeek.Tuesday, new DateTime(2015, 4, 7)),

			};

			foreach(var test in tests2)
			{
				var expected = test.Item3;
				var actual = test.Item1.GetFirstDayOfQuarter(test.Item2);
				Assert.AreEqual(expected, actual);
			}

		}

		[TestMethod()]
		public void GetLastDayOfQuarterTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 1, 1), new DateTime(2015, 3, 31), 
				new DateTime(2015, 1, 1, 13, 27, 35), new DateTime(2015, 3, 31), 
				new DateTime(2015, 3, 31, 23, 59, 59), new DateTime(2015, 3, 31), 
				new DateTime(2015, 4, 1), new DateTime(2015, 6, 30), 
				new DateTime(2015, 11, 16), new DateTime(2015, 12, 31), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetLastDayOfQuarter();
				Assert.AreEqual(expected, actual);
			}


			Tuple<DateTime, DayOfWeek, DateTime>[] tests2 = new Tuple<DateTime, DayOfWeek, DateTime>[]
			{
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Monday, new DateTime(2015, 3, 30)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Tuesday, new DateTime(2015, 3, 31)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Wednesday, new DateTime(2015, 3, 25)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Thursday, new DateTime(2015, 3, 26)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Friday, new DateTime(2015, 3, 27)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Saturday, new DateTime(2015, 3, 28)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Sunday, new DateTime(2015, 3, 29)),

				Tuple.Create(new DateTime(2015, 3, 15), DayOfWeek.Wednesday, new DateTime(2015, 3, 25)),
				Tuple.Create(new DateTime(2015, 4, 10), DayOfWeek.Tuesday, new DateTime(2015, 6, 30)),

			};

			foreach (var test in tests2)
			{
				var expected = test.Item3;
				var actual = test.Item1.GetLastDayOfQuarter(test.Item2);
				Assert.AreEqual(expected, actual);
			}
		}


		[TestMethod()]
		public void GetPreviousTest()
		{
			Tuple<DateTime, DayOfWeek, DateTime>[] tests = new Tuple<DateTime, DayOfWeek, DateTime>[]
			{
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Monday, new DateTime(2014, 12, 29)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Tuesday, new DateTime(2014, 12, 30)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Wednesday, new DateTime(2014, 12, 31)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Thursday, new DateTime(2014, 12, 25)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Friday, new DateTime(2014, 12, 26)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Saturday, new DateTime(2014, 12, 27)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Sunday, new DateTime(2014, 12, 28)),

				Tuple.Create(new DateTime(2015, 3, 15), DayOfWeek.Wednesday, new DateTime(2015, 3, 11)),
				Tuple.Create(new DateTime(2015, 4, 10), DayOfWeek.Tuesday, new DateTime(2015, 4, 7)),
			};

			foreach (var test in tests)
			{
				var expected = test.Item3;
				var actual = test.Item1.GetPrevious(test.Item2);
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void GetNextTest()
		{
			Tuple<DateTime, DayOfWeek, DateTime>[] tests = new Tuple<DateTime, DayOfWeek, DateTime>[]
			{
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Monday, new DateTime(2015, 1, 5)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Tuesday, new DateTime(2015, 1, 6)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Wednesday, new DateTime(2015, 1, 7)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Thursday, new DateTime(2015, 1, 8)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Friday, new DateTime(2015, 1, 2)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Saturday, new DateTime(2015, 1, 3)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Sunday, new DateTime(2015, 1, 4)),

				Tuple.Create(new DateTime(2015, 3, 15), DayOfWeek.Wednesday, new DateTime(2015, 3, 18)),
				Tuple.Create(new DateTime(2015, 4, 10), DayOfWeek.Tuesday, new DateTime(2015, 4, 14)),
			};

			foreach (var test in tests)
			{
				var expected = test.Item3;
				var actual = test.Item1.GetNext(test.Item2);
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void GetAgeInYearsTest()
		{
			DateTime utc = DateTime.UtcNow;

			Assert.AreEqual(utc.GetAgeInYears(), 0);
			Assert.AreEqual(utc.AddYears(-1).GetAgeInYears(), 1);
			Assert.AreEqual(utc.AddYears(-5).AddDays(1).GetAgeInYears(), 4);
		}

		[TestMethod()]
		public void GetWeekOfYearTest()
		{
			Tuple<DateTime, int>[] tests = new Tuple<DateTime, int>[]
			{
				Tuple.Create(new DateTime(2015, 1, 1), 1),
				Tuple.Create(new DateTime(2015, 1, 2), 1),
				Tuple.Create(new DateTime(2015, 1, 3), 1),
				Tuple.Create(new DateTime(2015, 1, 4), 2),
				Tuple.Create(new DateTime(2015, 1, 5), 2),
				Tuple.Create(new DateTime(2015, 1, 6), 2),
				Tuple.Create(new DateTime(2015, 1, 7), 2),
				Tuple.Create(new DateTime(2015, 1, 8), 2),
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.GetWeekOfYear();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void RoundToSecondsTest()
		{
			List<Tuple<DateTime, DateTimeSignificance, DateTime>> tests = new List<Tuple<DateTime, DateTimeSignificance, DateTime>>
			{
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToSeconds, new DateTime(2000, 2, 3, 4, 5, 6)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToMinutes, new DateTime(2000, 2, 3, 4, 5, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToHours, new DateTime(2000, 2, 3, 4, 0, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToDays, new DateTime(2000, 2, 3, 0, 0, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToMonths, new DateTime(2000, 2, 1, 0, 0, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToYears, new DateTime(2000, 1, 1, 0, 0, 0)),

				Tuple.Create(new DateTime(2000, 7, 17, 15, 38, 49, 720), DateTimeSignificance.ToSeconds, new DateTime(2000,  7, 17, 15, 38, 50)),
				Tuple.Create(new DateTime(2000, 7, 17, 15, 38, 49, 720), DateTimeSignificance.ToMinutes, new DateTime(2000,  7, 17, 15, 39, 0)),
				Tuple.Create(new DateTime(2000, 7, 17, 15, 38, 49, 720), DateTimeSignificance.ToHours, new DateTime(2000,  7, 17, 16, 0, 0)),
				Tuple.Create(new DateTime(2000, 7, 17, 15, 38, 49, 720), DateTimeSignificance.ToDays, new DateTime(2000,  7, 18, 0, 0, 0)),
				Tuple.Create(new DateTime(2000, 7, 17, 15, 38, 49, 720), DateTimeSignificance.ToMonths, new DateTime(2000,  8, 1, 0, 0, 0)),
				Tuple.Create(new DateTime(2000, 7, 17, 15, 38, 49, 720), DateTimeSignificance.ToYears, new DateTime(2001,  1, 1, 0, 0, 0)),

			};

			foreach (var test in tests)
			{
				var expected = test.Item3;
				var actual = test.Item1.Round(test.Item2);

				Assert.AreEqual(expected, actual, test.Item2.ToString());
			}
		}


		[TestMethod()]
		public void GetLastDayOfMonthTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2000, 1, 1), new DateTime(2000, 1, 31), 
				new DateTime(1900, 2, 14), new DateTime(1900, 2, 28),  // exception for leap-year
				new DateTime(2000, 2, 14), new DateTime(2000, 2, 29), 
			};

			for (int i = 0; i < tests.Length; i+=2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetLastDayOfMonth();
				Assert.AreEqual(expected, actual);
			}

			List<Tuple<DateTime, DayOfWeek, DateTime>> tests2 = new List<Tuple<DateTime, DayOfWeek, DateTime>>
			{
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Monday, new DateTime(2015, 1, 26)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Tuesday, new DateTime(2015, 1, 27)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Wednesday, new DateTime(2015, 1, 28)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Thursday, new DateTime(2015, 1, 29)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Friday, new DateTime(2015, 1, 30)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Saturday, new DateTime(2015, 1, 31)),
				Tuple.Create(new DateTime(2015, 1, 1), DayOfWeek.Sunday, new DateTime(2015, 1, 25)),
			};

			foreach (var test in tests2)
			{
				var expected = test.Item3;
				var actual = test.Item1.GetLastDayOfMonth(test.Item2);
				Assert.AreEqual(expected, actual, test.Item2.ToString());
			}
		}

		[TestMethod()]
		public void TruncateTest()
		{
			List<Tuple<DateTime, DateTimeSignificance, DateTime>> tests = new List<Tuple<DateTime, DateTimeSignificance, DateTime>>
			{
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToSeconds, new DateTime(2000, 2, 3, 4, 5, 6)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToMinutes, new DateTime(2000, 2, 3, 4, 5, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToHours, new DateTime(2000, 2, 3, 4, 0, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToDays, new DateTime(2000, 2, 3, 0, 0, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToMonths, new DateTime(2000, 2, 1, 0, 0, 0)),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToYears, new DateTime(2000, 1, 1, 0, 0, 0)),
			};

			foreach (var test in tests)
			{
				var expected = test.Item3;
				var actual = test.Item1.Truncate(test.Item2);

				Assert.AreEqual(expected, actual, test.Item2.ToString());
			}
		}

		[TestMethod()]
		public void FromUnixTimeTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(1970, 1, 1), 
				new DateTime(1991, 1, 2), 
				new DateTime(2000, 1, 2, 3, 4, 5)
			};

			foreach (var test in tests)
			{
				var expected = test;
				var unixTime = test.ToUnixTime();
				var actual = DateTimeExtensions.FromUnixTime(unixTime);
			}
		}

		[TestMethod()]
		public void CompareTest()
		{
			List<Tuple<DateTime, DateTime, DateTimeSignificance>> tests = new List<Tuple<DateTime, DateTime, DateTimeSignificance>>
			{
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToYears),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToMonths),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToDays),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToHours),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToMinutes),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 7), DateTimeSignificance.ToSeconds),

				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 1, 1, 1, 1, 1, 1), DateTimeSignificance.ToYears),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 1, 1, 1, 1, 1), DateTimeSignificance.ToMonths),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 1, 1, 1, 1), DateTimeSignificance.ToDays),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 1, 1, 1), DateTimeSignificance.ToHours),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 1, 1), DateTimeSignificance.ToMinutes),
				Tuple.Create(new DateTime(2000, 2, 3, 4, 5, 6, 7), new DateTime(2000, 2, 3, 4, 5, 6, 1), DateTimeSignificance.ToSeconds),

			};

			foreach (var test in tests)
			{
				int compare = test.Item1.CompareTo(test.Item2, test.Item3);
				Assert.IsTrue(compare == 0, test.Item1 + " / " + test.Item2 + " / " + test.Item3);
			}

		}

		[TestMethod()]
		public void GetFirstSundayOfMonthTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 1, 15), new DateTime(2015, 1, 4), 
				new DateTime(2015, 2, 16), new DateTime(2015, 2, 1), 
				new DateTime(2015, 6, 17), new DateTime(2015, 6, 7), 
				new DateTime(2016, 5, 18), new DateTime(2016, 5, 1), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetFirstSundayOfMonth();

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void GetPreviousSundayTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 6, 15), new DateTime(2015, 6, 14), 
				new DateTime(2015, 6, 16), new DateTime(2015, 6, 14), 
				new DateTime(2015, 6, 17), new DateTime(2015, 6, 14), 
				new DateTime(2015, 6, 18), new DateTime(2015, 6, 14), 
				new DateTime(2015, 6, 19), new DateTime(2015, 6, 14), 
				new DateTime(2015, 6, 20), new DateTime(2015, 6, 14), 
				new DateTime(2015, 6, 21), new DateTime(2015, 6, 14), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetPreviousSunday();

				Assert.AreEqual(expected, actual);
			}

		}

		[TestMethod()]
		public void GetPreviousMondayTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 6, 16), new DateTime(2015, 6, 15), 
				new DateTime(2015, 6, 17), new DateTime(2015, 6, 15), 
				new DateTime(2015, 6, 18), new DateTime(2015, 6, 15), 
				new DateTime(2015, 6, 19), new DateTime(2015, 6, 15), 
				new DateTime(2015, 6, 20), new DateTime(2015, 6, 15), 
				new DateTime(2015, 6, 21), new DateTime(2015, 6, 15), 
				new DateTime(2015, 6, 22), new DateTime(2015, 6, 15), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetPreviousMonday();

				Assert.AreEqual(expected, actual);
			}

		}

		[TestMethod()]
		public void GetNextSundayTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 6, 14), new DateTime(2015, 6, 21), 
				new DateTime(2015, 6, 15), new DateTime(2015, 6, 21), 
				new DateTime(2015, 6, 16), new DateTime(2015, 6, 21), 
				new DateTime(2015, 6, 17), new DateTime(2015, 6, 21), 
				new DateTime(2015, 6, 18), new DateTime(2015, 6, 21), 
				new DateTime(2015, 6, 19), new DateTime(2015, 6, 21), 
				new DateTime(2015, 6, 20), new DateTime(2015, 6, 21), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetNextSunday();

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void GetNextMondayTest()
		{
			DateTime[] tests = new DateTime[]
			{
				new DateTime(2015, 6, 15), new DateTime(2015, 6, 22), 
				new DateTime(2015, 6, 16), new DateTime(2015, 6, 22), 
				new DateTime(2015, 6, 17), new DateTime(2015, 6, 22), 
				new DateTime(2015, 6, 18), new DateTime(2015, 6, 22), 
				new DateTime(2015, 6, 19), new DateTime(2015, 6, 22), 
				new DateTime(2015, 6, 20), new DateTime(2015, 6, 22), 
				new DateTime(2015, 6, 21), new DateTime(2015, 6, 22), 
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				var expected = tests[i + 1];
				var actual = tests[i].GetNextMonday();

				Assert.AreEqual(expected, actual);
			}
		}
	}
}
