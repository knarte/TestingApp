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
    using Android.OS;
    using Core.ViewModels;
    using MvvmCross.Platforms.Android.Views;

    [Activity(Label = "@string/app_name")]
    public class TipView : MvxActivity<TipViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.TipPage);
        }
    }

}