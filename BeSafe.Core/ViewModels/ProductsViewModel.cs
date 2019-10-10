using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BeSafe.Core.Models.Internal.Responses;
    using BeSafe.Core.Models.Provider;
    using Helpers;
    using Interfaces;
    using Models;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;

    public class ProductsViewModel : MvxViewModel
    {
        private List<Device> products;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;

        public List<Device> Products
        {
            get => this.products;
            set => this.SetProperty(ref this.products, value);
        }

        public ProductsViewModel(
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var request = new
            {
                token.AccessToken,
                token.BaseId
            };

            var response = await this.apiService.APIRequestPOST<List<Device>>(
                Endpoint.URL_BASE,
                "/api",
                "/Devices/GetDeviceStatusInfoProvider",
                request,
                false
                );

            //var response = await this.apiService.GetListAsync<Product>(
            //    "https://shopzulu.azurewebsites.net",
            //    "/api",
            //    "/Products",
            //    "bearer",
            //    token.AccessToken);

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            this.Products = (List<Device>)response.Result;
        }
    }

}
