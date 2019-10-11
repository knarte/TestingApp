using System;
using System.Threading.Tasks;
using BeSafe.Core.Helpers;
using BeSafe.Core.ViewModels;
using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace BeSafe.Core
{
    public class AppStart : MvxAppStart
    {
        private bool isLogged;

        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
    : base(app, mvxNavigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            if (Settings.IsRemember)
            {
                return NavigationService.Navigate<MenuViewModel>();
            }
            else
            {
                return NavigationService.Navigate<LoginViewModel>();
            }
        }
    }
}
