

namespace BeSafe.Core.ViewModels
{
    using System.Windows.Input;
    using BeSafe.Core.Helpers;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;

    public class LoginViewModel : MvxViewModel
    {
        private string email;
        private string password;
        private MvxCommand loginCommand;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private readonly IPermissionService permissionService;
        private readonly IControlForegroundService controlForegroundService;
        private MvxCommand startCommand;
        private MvxCommand stopCommand;
        private bool isLoading;

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public ICommand LoginCommand
        {
            get
            {
                this.loginCommand = this.loginCommand ?? new MvxCommand(this.DoLoginCommand);
                return this.loginCommand;
            }
        }

        public ICommand StartCommand
        {
            get
            {
                this.startCommand = this.startCommand ?? new MvxCommand(this.DoStartCommand);
                return this.startCommand;
            }
        }
        public ICommand StopCommand
        {
            get
            {
                this.stopCommand = this.stopCommand ?? new MvxCommand(this.DoStopCommand);
                return this.stopCommand;
            }
        }

        public LoginViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService,
            IPermissionService permissionService,
            IControlForegroundService controlForegroundService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.permissionService = permissionService;
            this.controlForegroundService = controlForegroundService;
            this.controlForegroundService.CreateService(Settings.SavedInstanceState);
            this.Email = "desarrollo@navisaf.com";
            this.Password = "c4sc4j4l";
            this.IsLoading = false;

            GoToNextPage();
        }

        private async void GoToNextPage()
        {
            if (Settings.SavedInstanceState == true)
            {
                
            }

        }

        private async void DoLoginCommand()
        {
            if(Settings.SavedInstanceState == false)
            {
                this.permissionService.PermissionRequest("android.permission.ACCESS_FINE_LOCATION");

                if (string.IsNullOrEmpty(this.Email))
                {
                    this.dialogService.Alert("Error", "You must enter an email.", "Accept");
                    return;
                }

                if (string.IsNullOrEmpty(this.Password))
                {
                    this.dialogService.Alert("Error", "You must enter a password.", "Accept");
                    return;
                }

                this.IsLoading = true;

                var request = new TokenRequest
                {
                    Password = this.Password,
                    User = this.Email,
                    BaseName = "holcim"
                };

                var response = await this.apiService.GetTokenAsync(
                    "https://navisafsdkapi-qa.azurewebsites.net",
                    "/api",
                    "/Account/Login",
                    request);

                if (!response.IsSuccess)
                {
                    this.IsLoading = false;
                    this.dialogService.Alert("Error", "User or password incorrect.", "Accept");
                    return;
                }

                var token = (TokenResponse)response.Result;
                Settings.UserEmail = this.Email;
                Settings.Token = JsonConvert.SerializeObject(token);
                this.IsLoading = false;

                await this.navigationService.Navigate<MenuViewModel>();
            }
            else
            {
                //this.dialogService.Alert("Ok", "Fuck yeah!", "Accept");
                await this.navigationService.Navigate<MenuViewModel>();
            }

        }

        private void DoStartCommand()
        {
            Settings.SavedInstanceState = this.controlForegroundService.StartServices();
        }

        private void DoStopCommand()
        {
            Settings.SavedInstanceState = this.controlForegroundService.StopServices();
        }

    }

}
