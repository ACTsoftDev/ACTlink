using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace actchargers
{
    public class BoolToAlphaValueConverter : MvxValueConverter<bool, Single>
    {
        protected override Single Convert
        (bool value, Type targetType, object parameter, CultureInfo culture)
        {
            Single alphavalue;

            if (value)
                alphavalue = 1.0f;
            else
                alphavalue = 0.35f;

            return alphavalue;
        }
    }
}
