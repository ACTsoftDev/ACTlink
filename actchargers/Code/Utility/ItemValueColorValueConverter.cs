using System;
using System.Globalization;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Color;

namespace actchargers
{
	public class ItemValueColorValueConverter: MvxColorValueConverter<Boolean>
	{
		
		protected override MvxColor Convert(bool value, object parameter, CultureInfo culture)
		{
			if (value)//NativeColor
			{
				return new MvxColor(0,111,168);
			}

			return new MvxColor(216, 216, 216);
		}
	}
}
