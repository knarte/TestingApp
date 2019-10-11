using MvvmCross.Platform.UI;
using MvvmCross.Plugin.Color;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace BeSafe.Core.Converters
{

    public class TextToColorValueConverter : MvxColorValueConverter<string>
    {
        private static readonly Color HeaderGroupWhite = Color.White;
        private static readonly Color HeaderGroupBlack = Color.Black;
        private static readonly Color HeaderGroupYellow = Color.Yellow;
        private static readonly Color HeaderGroupRed= Color.Red;
        protected override Color Convert(string value, object parameter, CultureInfo culture)
        {
            if (value.ToLower() == "white")
            {
                return HeaderGroupWhite;
            }else if (value.ToLower() == "black")
            {
                return HeaderGroupBlack;
            }
            else if (value.ToLower() == "yellow")
            {
                return HeaderGroupYellow;
            }
            else if (value.ToLower() == "red")
            {
                return HeaderGroupRed;
            }
            else
            {
                return HeaderGroupBlack;
            }

        }
    }
}
