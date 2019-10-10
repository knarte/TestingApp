using MvvmCross.Plugin.Color;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BeSafe.Core.Converters
{
    public class CategoryTextColorConverter : MvxNativeColorValueConverter<string>
    {
        protected override MvxColor Convert(string value, object parameter, CultureInfo culture)
        {
            if (value == string.Empty)
            {
                return MvxColors.Blue;
            }
            return MvxColors.Red;
        }
    }
}
