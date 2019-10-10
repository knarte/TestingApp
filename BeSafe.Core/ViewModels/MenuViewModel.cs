using System;
namespace BeSafe.Core.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Helpers;
    using Interfaces;
    using Models.DB;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;

    public class MenuViewModel : MvxViewModel
    {
        private List<AuthorizedFunction> menuItems;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        public List<AuthorizedFunction> MenuItems
        {
            get => this.menuItems;
            set => this.SetProperty(ref this.menuItems, value);
        }


        private MvxCommand itemSelectedCommand;
        public ICommand ItemSelectedCommand
        {
            get
            {
                this.itemSelectedCommand = this.itemSelectedCommand ?? new MvxCommand(this.GoToStartRoutePage);
                return this.itemSelectedCommand;
            }
        }


        public MenuViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.LoadMenuItems();
        }

        private async void GoToStartRoutePage()
        {
            await this.navigationService.Navigate<RouteViewModel>();
        }

        private async void LoadMenuItems()
        {
            this.MenuItems = new List<AuthorizedFunction>();

            this.MenuItems.Add(new AuthorizedFunction
            {
                Id = 1,
                Name = "Iniciar ruta",
                IconPath = "ic_start_route.png"
            });

            //var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            //var request = new
            //{
            //    token.AccessToken,
            //    token.BaseId
            //};

            //var response = await this.apiService.APIRequestPOST<List<Device>>(
            //    Endpoint.URL_BASE,
            //    "/api",
            //    "/Devices/GetDeviceStatusInfoProvider",
            //    request,
            //    false
            //    );

            ////var response = await this.apiService.GetListAsync<Product>(
            ////    "https://shopzulu.azurewebsites.net",
            ////    "/api",
            ////    "/Products",
            ////    "bearer",
            ////    token.AccessToken);

            //if (!response.IsSuccess)
            //{
            //    this.dialogService.Alert("Error", response.Message, "Accept");
            //    return;
            //}

            //this.Products = (List<Device>)response.Result;
        }
    }
}
