
namespace BeSafe.Cross.Droid.Views
{

    using BeSafe.Core.ViewModels;
    using BeSafe.Cross.Droid;
    using global::Android.App;
    using global::Android.OS;
    using MvvmCross.Platforms.Android.Views;

    [Activity(Label = "@string/app_name")]
    public class GPSLocationView : MvxActivity<GPSLocationViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.GPSLocationPage);
        }
    }
}