using System;
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
	public class ColorExtensionsTests
	{
		[TestMethod()]
		public void ToColorTest()
		{
			List<Tuple<string, Color?>> tests = new List<Tuple<string, Color?>>
			{
				Tuple.Create<string, Color?>("", null),
				Tuple.Create<string, Color?>("White", Color.White),
				Tuple.Create<string, Color?>("WHITE", Color.White),
				Tuple.Create<string, Color?>(" white ", Color.White),
				Tuple.Create<string, Color?>("#fff", Color.White),
				Tuple.Create<string, Color?>("fcd", Color.FromArgb(0xff, 0xcc, 0xdd)),
				Tuple.Create<string, Color?>("ffeedd", Color.FromArgb(0xff, 0xee, 0xdd)),
				Tuple.Create<string, Color?>("#111111", Color.FromArgb(0x11, 0x11, 0x11)),

				Tuple.Create<string, Color?>("rgb(1,2,3)", Color.FromArgb(1, 2, 3)),
				Tuple.Create<string, Color?>("rgb( 1, 2, 3)", Color.FromArgb(1, 2, 3)),
				Tuple.Create<string, Color?>("rgba(2, 3, 4, 1.0)", Color.FromArgb(255, 2, 3, 4)),

				Tuple.Create<string, Color?>("hsl(120, 100%, 50%)", ColorExtensions.FromHsl(1.0/3.0, 1.0, 0.5)),
				Tuple.Create<string, Color?>("hsl(360, 10%, 25%)", ColorExtensions.FromHsl(0, .1, .25)),
				Tuple.Create<string, Color?>("hsla(30, 10%, 25%, 0.5)", ColorExtensions.FromAhsl(0.5, 1.0/12.0, .1, .25)),
			};

			foreach (var t in tests)
			{
				var expected = t.Item2;
				var actual = t.Item1.ToColor();
				if (!actual.HasValue)
				{
					Assert.IsNull(expected, "Color: " + t.Item1);
				}
				else
				{
					Assert.IsNotNull(expected, "Color: " + t.Item1);
					Assert.IsTrue(expected.Value.R == actual.Value.R 
						&& expected.Value.G == actual.Value.G 
						&& expected.Value.B == actual.Value.B,
						"Color: " + t.Item1);
				}
			}
		}

		[TestMethod()]
		public void IsAccessibilityContrastTest()
		{
			Color[] accessible = new Color[]
			{
				Color.Black, Color.White,
				Color.White, Color.Black,
				Color.Blue, Color.White,
				Color.White, Color.DarkSlateBlue
			};

			Color[] nonAccessible = new Color[]
			{
				Color.Black, Color.FromArgb(0x20, 0x30, 0x40),
				Color.Yellow, Color.White,
				Color.Red, Color.Orange,
				Color.Green, Color.LightGreen,
				Color.White, Color.White
			};

			for (int i = 0; i < accessible.Length; i+= 2)
			{
				var color1 = accessible[i];
				var color2 = accessible[i + 1];

				Assert.IsTrue(color1.IsAccessibilityContrast(color2), "Colors: " + color1.ToString() + "/" + color2.ToString() );
			}

			for (int i = 0; i < nonAccessible.Length; i += 2)
			{
				var color1 = nonAccessible[i];
				var color2 = nonAccessible[i + 1];

				Assert.IsFalse(color1.IsAccessibilityContrast(color2), "Colors: " + color1.ToString() + "/" + color2.ToString());
			}

		}

		[TestMethod()]
		public void FromHslTest()
		{
			Assert.AreEqual(Color.FromArgb(0, 0, 0), ColorExtensions.FromHsl(0, 0, 0));
			Assert.AreEqual(Color.FromArgb(0, 0, 0), ColorExtensions.FromHsl(1/3.0, 0, 0));
			Assert.AreEqual(Color.FromArgb(255,255, 255), ColorExtensions.FromHsl(1, 1, 1));

			Assert.AreEqual(Color.FromArgb(255, 0, 0), ColorExtensions.FromHsl(0, 1, .5));
			Assert.AreEqual(Color.FromArgb(0, 255, 0), ColorExtensions.FromHsl(1.0/3.0, 1, .5));
			Assert.AreEqual(Color.FromArgb(0, 0, 255), ColorExtensions.FromHsl(2.0/3.0, 1, .5));
		}


		[TestMethod()]
		public void ToHsl()
		{
			// RGB -> HSL -> RGB
			for (int r = 0; r <= 255; r += 3)
			{
				for (int g = 0; g <= 255; g += 5)
				{
					for (int b = 0; b <= 255; b += 7)
					{
						var rgb1 = Color.FromArgb(r, g, b);
						var hsl = rgb1.ToHsl();
						var rgb2 = ColorExtensions.FromHsl(hsl);
						Assert.IsTrue(rgb1.R == rgb2.R && rgb1.G == rgb2.G && rgb1.B == rgb2.B, rgb1 + " / " + rgb2);
					}
				}
			}

		}

		[TestMethod()]
		public void ToHexColorTest()
		{
			List<Tuple<Color, string>> tests = new List<Tuple<Color, string>>
			{
				Tuple.Create(Color.White, "ffffff"),
				Tuple.Create(Color.Black, "000000"),
				Tuple.Create(Color.Gray, "808080"),
				Tuple.Create(Color.FromArgb(0x10, 0x20, 0x30), "102030")
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.ToHexColor();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void ToHtmlColorTest()
		{
			List<Tuple<Color, string>> tests = new List<Tuple<Color, string>>
			{
				Tuple.Create(Color.White, "#ffffff"),
				Tuple.Create(Color.Black, "#000000"),
				Tuple.Create(Color.Gray, "#808080"),
				Tuple.Create(Color.FromArgb(0x10, 0x20, 0x30), "#102030")
			};

			foreach (var test in tests)
			{
				var expected = test.Item2;
				var actual = test.Item1.ToHtmlColor();
				Assert.AreEqual(expected, actual);
			}
		}

	}
}
