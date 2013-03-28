﻿using System;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Plugins.Color;
using Cirrious.CrossCore.UI;

namespace ValueConversion.Core.Converters
{
    public class TwoWayConverter : MvxValueConverter<double, string>
    {
        protected override string Convert(double value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value * value).ToString();
        }

        protected override double ConvertBack(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			double doubleValue;
			double.TryParse(value, out doubleValue);
            return Math.Sqrt(doubleValue);
        }
    }

    public class StringLengthConverter : MvxValueConverter<string>
    {
        protected override object Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            value = value ?? string.Empty;
            return value.Length;
        }
    }

	public class ContrastColorConverter : MvxColorConverter
	{
		protected override MvxColor Convert (object value, object parameter, System.Globalization.CultureInfo culture)
		{
			var input = (MvxColor)value;
			var brightnessToUse = SimpleContrast(input.R, input.G, input.B);
			return new MvxColor(brightnessToUse,brightnessToUse,brightnessToUse);
		}

		private static int SimpleContrast(params int[] value)
		{
			// this is only a very simple contrast method
			// for more advanced methods you need to look at HSV-type approaches

			int max = 0;
			foreach (var v in value)
			{
				if (v > max)
					max = v;
			}

			return 255 - max;
			if (max < 128)
			{
				return 128 + max;
			}

			return max - 128;
		}
	}

    public class StringReverseConverter : MvxValueConverter<string>
    {
        protected override object Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            value = value ?? string.Empty;
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }

    public class TimeAgoConverter
        : MvxValueConverter<DateTime>
    {
        protected override object Convert(DateTime when, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var difference = (DateTime.UtcNow - when).TotalSeconds;

            string whichFormat;
            int valueToFormat;
            if (difference < 30.0)
            {
                whichFormat = "Just now";
                valueToFormat = 0;
            }
            else if (difference < 100.0)
            {
                whichFormat = "{0}s ago";
                valueToFormat = (int)difference;
            }
            else if (difference < 3600.0)
            {
                whichFormat = "{0}m ago";
                valueToFormat = (int)(difference / 60);
            }
            else if (difference < 24 * 3600)
            {
                whichFormat = "{0}h ago";
                valueToFormat = (int)(difference / (3600));
            }
            else
            {
                whichFormat = "{0}d ago";
                valueToFormat = (int)(difference / (3600 * 24));
            }

            return string.Format(whichFormat, valueToFormat);
        }
    }
}