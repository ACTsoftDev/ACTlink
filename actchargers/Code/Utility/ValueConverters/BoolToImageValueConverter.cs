using System;
using System.Globalization;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Plugins.File;

namespace actchargers
{
    public class BoolToImageValueConverter : MvxValueConverter<bool, string>
    {
        const string DROID = "0";
        const string IOS = "1";
        const string OTHER = "2";

        protected override string Convert
        (bool value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageName = BoolToImageNameConverter.BoolToImageName(value);

            switch (parameter.ToString())
            {
                case DROID:
                    return imageName.ToLower();


                case IOS:
                    return "res:" + imageName + ".png";

                case OTHER:
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        return Mvx.Resolve<IMvxFileStore>().NativePath(imageName);
                    }

                    return "";

                default:
                    return "";
            }
        }
    }
}
