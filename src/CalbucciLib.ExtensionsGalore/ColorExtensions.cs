using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{

	public static class ColorExtensions
	{

		public static Color? ToColor(string str, bool htmlSafe = false)
		{
			if (string.IsNullOrWhiteSpace(str))
				return null;

			str = str.Trim();
			if (str.StartsWith("#"))
			{
				str = str.Substring(1);
			}
			else if (str.StartsWith("rgb("))
			{
				var parts = str.Substring("rgb(".Length).Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length != 3)
					return null;
				int r = parts[0].ToInt();
				int g = parts[1].ToInt();
				int b = parts[2].ToInt();

				if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
					return null;

				return Color.FromArgb(r, g, b);
			}
			else if (str.StartsWith("rgba("))
			{
				var parts = str.Substring("rgba(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length != 4)
					return null;
				int r = parts[0].ToInt();
				int g = parts[1].ToInt();
				int b = parts[2].ToInt();
				int a = (int)(255 * parts[3].ToDouble());

				if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255 || a < 0 || a > 255)
					return null;

				return Color.FromArgb(a, r, g, b);
				
			}
			else if (str.StartsWith("hsl("))
			{
				var parts = str.Substring("hsl(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length != 3)
					return null;
				double h = parts[0].ToDouble() / 360.0;
				double s = parts[1].ToDouble();
				double l = parts[2].ToDouble();

				// h can be any value actually, since it gets converted to 0-360 (wheel of color)
				if (s < 0 || s > 1 || l < 0 || l > 1)
					return null;

				return ColorExtensions.FromHsl(h, s, l);
			}
			else if (str.StartsWith("hsla("))
			{
				var parts = str.Substring("hsla(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length != 4)
					return null;
				double h = parts[0].ToDouble() / 360.0;
				double s = parts[1].ToDouble();
				double l = parts[2].ToDouble();
				double a = parts[3].ToDouble();

				if (s < 0 || s > 1 || l < 0 || l > 1 || a < 0 || a > 1)
					return null;

				return ColorExtensions.FromAhsl(a, h, s, l);
				
			}
			else 
			{
				try
				{
					var color = Color.FromName(str);
					if(!(color.R == 0 && color.G == 0 && color.B == 0 && color.A == 0) || string.Compare(str, "black", true) == 0)
						return color;
				}
				catch (Exception)
				{
				}
				if (htmlSafe)
					return null; // must be either a named color or start with #/rgb to be HTML safe
			}
			

			if (str.Length == 3)
			{
				str = string.Concat(str[0], str[0], str[1], str[1], str[2], str[2]);
			}
			else if (str.Length != 6)
			{
				return null;
			}

			byte[] bytes = str.ToBytesFromHex();

			if (bytes == null || bytes.Length != 3)
				return null;

			return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
			
		}
		// ==========================================================================
		//
		//    Accessibility
		//
		// ==========================================================================
		/// <summary>
		/// Test two colors for enough contrast between them for one to be the foreground and the other the background color
		/// according to the Web Content Accessibility Guidelines (WCAG). http://www.w3.org/TR/WCAG20/
		/// </summary>
		/// <param name="secondColor"></param>
		/// <param name="minContrastRatio">4.5 for normal (default), use 3.0 for large text, use 7.0 for AAA</param>
		public static bool IsAccessibilityContrast(this Color color, Color secondColor, double minContrastRatio = 4.5)
		{
			return color.GetContrastRatio(secondColor) >= minContrastRatio;
		}

		/// <summary>
		/// Return the contrast ratio between two colors. Maximum contrast is 21 (black on white).
		/// </summary>
		public static double GetContrastRatio(this Color color, Color secondColor)
		{
			Func<int, double> gets = cv =>
			{
				double d = cv/255.0;
				d = (d <= 0.03928) ? d/12.92 : Math.Pow(((d + 0.055)/1.055), 2.4);
				return d;
			};

			Func<Color, double> getl = c =>
			{
				var r = gets(c.R);
				var g = gets(c.G);
				var b = gets(c.B);
				return (0.2126*r + 0.7152*g + 0.0722*b);
			};

			var l1 = getl(color);
			var l2 = getl(secondColor);

			var ratio = (Math.Max(l1, l2) + 0.05)/(Math.Min(l1, l2) + 0.05);

			return ratio;
		}



		//public static bool IsColorBlindAccessible(this Color color, Color secondColor)
		//{
		//	throw new NotImplementedException();
		//}

		// ==========================================================================
		//
		//   Convert
		//
		// ==========================================================================

		public static Color FromHsl(double hue, double saturation, double luminosity)
		{
			return FromAhsl(1.0, hue, saturation, luminosity);
		}

		public static Color FromAhsl(double alpha, double hue, double saturation, double luminosity)
		{
			return FromHsl(Hsl.FromAhsl(alpha, hue, saturation, luminosity));
		}
		public static Color FromHsl(Hsl hsl)
		{
			Func<double, double, double, double> hueToRgb = (c, t1, t2) =>
			{
				if (c < 0) c += 1.0;
				if (c > 1) c -= 1.0;
				if (6.0*c < 1.0) return t1 + (t2 - t1)*6.0*c;
				if (2.0*c < 1.0) return t2;
				if (3.0*c < 2.0) return t1 + (t2 - t1)*(2.0/3.0 - c)*6.0;
				return t1;
			};

			int alpha = (int)Math.Round(hsl.Alpha*255.0);
			if (Math.Abs(hsl.Saturation) < Hsl.MaxHslColorPrecision)
			{
				var mono = (int) Math.Round(hsl.Luminosity*255.0);
				return Color.FromArgb(alpha, mono, mono, mono);
			}
			else
			{
				double t2 = hsl.Luminosity < 0.5
					? hsl.Luminosity*(1.0 + hsl.Saturation)
					: (hsl.Luminosity + hsl.Saturation) - (hsl.Luminosity*hsl.Saturation);
				double t1 = 2.0*hsl.Luminosity - t2;

				var r = (int) Math.Round(hueToRgb(hsl.Hue + 1.0/3.0, t1, t2)*255.0);
				var g = (int) Math.Round(hueToRgb(hsl.Hue, t1, t2)*255.0);
				var b = (int) Math.Round(hueToRgb(hsl.Hue - 1.0/3.0, t1, t2)*255.0);

				return Color.FromArgb(alpha, r, g, b);
			}
		}

		public static Hsl ToHsl(this Color color)
		{
			double r = (color.R / 255.0);
			double g = (color.G / 255.0);
			double b = (color.B / 255.0);

			double min = Math.Min(Math.Min(r, g), b);
			double max = Math.Max(Math.Max(r, g), b);
			double delta = max - min;

			Hsl hsl = new Hsl();
			hsl.Alpha = color.A / 255.0;
			hsl.Luminosity = ((max + min) / 2.0);

			if (Math.Abs(delta) > Hsl.MaxHslColorPrecision)
			{
				if (hsl.Luminosity < 0.5)
				{
					hsl.Saturation = (delta / (max + min));
				}
				else
				{
					hsl.Saturation = (delta / (2.0 - max - min));
				}

				if (Math.Abs(r - max) < Hsl.MaxHslColorPrecision)
				{
					hsl.Hue = (g - b) / delta + (g < b ? 6 : 0);
				}
				else if (Math.Abs(g - max) < Hsl.MaxHslColorPrecision)
				{
					hsl.Hue = 2 + (b - r) / delta;
				}
				else 
				{
					hsl.Hue = 4 + (r - g) / delta;
				}
				hsl.Hue = hsl.Hue/6.0;
			}

			return hsl;
		}

		public static string ToHexColor(this Color color)
		{
			return color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("x2");
		}

		public static string ToHtmlColor(this Color color)
		{
			return "#" + color.ToHexColor();
		}


		// ==========================================================================
		//
		//    Manipulate Colors
		//
		// ==========================================================================
		//public static Color ChangeSaturation(this Color color, double delta)
		//{
		//}

		//public static Color ChangeBrightness(this Color color, double delta)
		//{
		//}


	}
}
