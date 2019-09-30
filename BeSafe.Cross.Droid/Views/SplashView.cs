using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BeSafe.Cross.Droid.Views
{
    using Android.App;
    using Android.Content.PM;
    using Core;
    using MvvmCross.Platforms.Android.Core;
    using MvvmCross.Platforms.Android.Views;

    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashView : MvxSplashScreenActivity
    {
        public SplashView() : base(Resource.Layout.SplashPage)
        {
        }
    }

}