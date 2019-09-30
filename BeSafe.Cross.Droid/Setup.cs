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

namespace BeSafe.Cross.Droid
{
    using Core;
    using Core.Interfaces;
    using MvvmCross;
    using MvvmCross.Platforms.Android.Core;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
            Mvx.IoCProvider.RegisterType<IPermissionService, PermissionService>();
            Mvx.IoCProvider.RegisterType<IControlForegroundService, ControlForegroundService>();
            base.InitializeFirstChance();                       
        }


        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            var assemblies = base.GetPluginAssemblies().ToList();
            assemblies.Add(typeof(MvvmCross.Plugin.Visibility.Platforms.Android.Plugin).Assembly);
            return assemblies;
        }
    }

}