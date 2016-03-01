using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore
{
	public class Hsl
	{
		public static readonly double MaxHslColorPrecision = 0.003; 

		public double Alpha { get; set; }
		public double Hue { get; set; }
		public double Saturation { get; set; }
		public double Luminosity { get; set; }

		public Hsl()
		{
			Alpha = 1.0;
		}

		public static Hsl FromHsl(double hue, double saturation, double luminosity)
		{
			var hsl = new Hsl
			{
				Hue = hue,
				Saturation = saturation,
				Luminosity = luminosity
			};

			return hsl;
		}

		public static Hsl FromAhsl(double alpha, double hue, double saturation, double luminosity)
		{
			var hsl = new Hsl
			{
				Alpha = alpha,
				Hue = hue,
				Saturation = saturation,
				Luminosity = luminosity
			};

			return hsl;
		}

		public override bool Equals(object obj)
		{
			var rhs = obj as Hsl;
			if (rhs == null)
				return false;


			return Math.Abs(Hue - rhs.Hue) < MaxHslColorPrecision
			       && Math.Abs(Luminosity - rhs.Luminosity) < MaxHslColorPrecision
			       && Math.Abs(Saturation - rhs.Saturation) < MaxHslColorPrecision
			       && Math.Abs(Alpha - rhs.Alpha) < MaxHslColorPrecision;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public override string ToString()
		{
			return "HSL[A=" + Alpha.ToString("N4")
			       + ", H=" + Hue.ToString("N4")
			       + ", S=" + Saturation.ToString("N4")
			       + ", L=" + Luminosity.ToString("N4")
			       + "]";
		}
	}
}
