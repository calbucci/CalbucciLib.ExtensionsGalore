using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalbucciLib.ExtensionsGalore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CalbucciLib.ExtensionsGalore.Tests
{
	[TestClass()]
	public class TimeZoneInfoExtensionsTests
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			// Init the timezone tables
			var firstTz = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault();
			// Init the TimeZoneExtensions
			var olson = firstTz.ToOlsonTimeZone();
		}

		[TestMethod()]
		public void ToOlsonTimeZoneTest()
		{
			foreach (var tzi in TimeZoneInfo.GetSystemTimeZones())
			{
				string olson = tzi.ToOlsonTimeZone();
				Assert.IsNotNull(olson, "{0} {1}", tzi.Id, tzi.BaseUtcOffset);
			}

			string[] tests = new[]
			{
				"Alaskan Standard Time", "US/Alaska",
				"Pacific Standard Time", "US/Pacific"
			};

			for (int i = 0; i < tests.Length; i+=2)
			{
				string expected = tests[i + 1];
				TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(tests[i]);
				string actual = tzi.ToOlsonTimeZone();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void FromOlsonToTimeZoneIdTest()
		{
			string[] tests = new[]
			{
				"US/Alaska", "Alaskan Standard Time",
				"US/Pacific", "Pacific Standard Time",
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string expected = tests[i + 1];
				string actual = TimeZoneInfoExtensions.FromOlsonToTimeZoneId(tests[i]);
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void FromOlsonToTimeZoneInfoTest()
		{
			string[] tests = new[]
			{
				"US/Alaska", "Alaskan Standard Time",
				"US/Pacific", "Pacific Standard Time",
			};

			for (int i = 0; i < tests.Length; i += 2)
			{
				string expected = tests[i + 1];
				var tzi = TimeZoneInfoExtensions.FromOlsonToTimeZoneInfo(tests[i]);
				Assert.IsNotNull(tzi);
				Assert.AreEqual(expected, tzi.Id);
			}
		}
	}
}
