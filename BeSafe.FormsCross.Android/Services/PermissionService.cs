
namespace BeSafe.FormsCross.Droid.Services
{
    using System;
    using MvvmCross;
    using MvvmCross.Platforms.Android;
    using Android.Support.V4.Content;
    using Android.Content.PM;
    using BeSafe.Core.Interfaces;
    using Android.Support.V4.App;

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