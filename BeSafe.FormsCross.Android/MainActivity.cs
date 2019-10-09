

namespace BeSafe.FormsCross.Droid
{

    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using BeSafe.Core.Interfaces;
    using BeSafe.FormsCross.Droid.Services;
    using Core;
    using MvvmCross;
    using MvvmCross.Forms.Platforms.Android.Core;
    using MvvmCross.Forms.Platforms.Android.Views;
    using MvvmCross.Platforms.Android.Core;

    [Activity(
        Label = "Tip Calc",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : MvxFormsAppCompatActivity<MvxFormsAndroidSetup<App, FormsApp>, App, FormsApp>
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);


        }
       



    }

}