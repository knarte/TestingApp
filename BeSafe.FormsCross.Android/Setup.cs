using System;
namespace BeSafe.FormsCross.Droid
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
