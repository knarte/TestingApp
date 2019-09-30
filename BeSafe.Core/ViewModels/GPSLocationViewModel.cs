using BeSafe.Core.Helpers;
using BeSafe.Core.Interfaces;
using BeSafe.Core.Models;
using BeSafe.Core.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Location;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeSafe.Core.ViewModels
{
    public class GPSLocationViewModel: MvxViewModel
    {
        private double latitude;
        private double longitude;
        private double speed;
        private double altitude;
        private double heading;
        private double accuracy;
        private double altitudeAccuracy;
        private double headingAccuracy;

        private readonly IDialogService dialogService;
        private readonly IMvxMessenger messenger;
        private readonly IMvxLocationWatcher watcher;
        private readonly MvxSubscriptionToken token;
        private readonly IControlForegroundService controlForegroundService;
        private MvxCommand startCommand;
        private MvxCommand stopCommand;
        public double Latitude
        {
            get => this.latitude;
            set => this.SetProperty(ref this.latitude, value);
        }
        public double Longitude
        {
            get => this.longitude;
            set => this.SetProperty(ref this.longitude, value);
        }

        public double Speed
        {
            get => this.speed;
            set => this.SetProperty(ref this.speed, value);
        }
        public double Altitude
        {
            get => this.altitude;
            set => this.SetProperty(ref this.altitude, value);
        }
        public double Heading
        {
            get => this.heading;
            set => this.SetProperty(ref this.heading, value);
        }
        public double Accuracy
        {
            get => this.accuracy;
            set => this.SetProperty(ref this.accuracy, value);
        }
        public double AltitudeAccuracy
        {
            get => this.altitudeAccuracy;
            set => this.SetProperty(ref this.altitudeAccuracy, value);
        }
        public double HeadingAccuracy
        {
            get => this.headingAccuracy;
            set => this.SetProperty(ref this.headingAccuracy, value);
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


        public GPSLocationViewModel(
            ILocationService locationService,
            IMvxMessenger messenger,
            IDialogService dialogService,
            IControlForegroundService controlForegroundService
            )
        {
            this.dialogService = dialogService;
            this.messenger = messenger;
            this.controlForegroundService = controlForegroundService;
            this.controlForegroundService.CreateService(Settings.SavedInstanceState);
            token = messenger.Subscribe<LocationMessage>(OnLocationMessage);
        }


        private void OnLocationMessage(LocationMessage locationMessage)
        {
            Latitude = locationMessage.Lat;
            Longitude = locationMessage.Lng;
            Speed = locationMessage.Speed;
            Altitude = locationMessage.Altitude;
            Heading = locationMessage.Heading;
            Accuracy = locationMessage.Accuracy;
            AltitudeAccuracy = locationMessage.AltitudeAccuracy;
            HeadingAccuracy = locationMessage.HeadingAccuracy;
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
