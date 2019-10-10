using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Support.V4.EventSource;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Platforms.Android;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using BeSafe.Core.Interfaces;
using Android.Support.V4.App;
using System.Threading.Tasks;

namespace BeSafe.Cross.Droid.Services
{
    public class PermissionService : IPermissionService
    {
        public void PermissionRequest(string permissionCode)
        {
            var top = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            if (ContextCompat.CheckSelfPermission(act, permissionCode) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(act, new String[] { permissionCode }, 1);
            }

        }
    }
}