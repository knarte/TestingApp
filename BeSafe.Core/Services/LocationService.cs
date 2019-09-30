using BeSafe.Core.Interfaces;
using MvvmCross.Plugin.Location;
using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Services
{
    public class LocationMessage
    : MvxMessage
    {
        public LocationMessage(object sender, double lat, double lng, double speed, double altitude, double heading, double accuracy, double headingAccuracy, double altitudeAccuracy)
        : base(sender)
        {
            Lng = lng;
            Lat = lat;
            Speed = speed;
            Altitude = altitude;
            Heading = heading;
            Accuracy = accuracy;
            HeadingAccuracy = headingAccuracy;
            AltitudeAccuracy = altitudeAccuracy;
        }

        public double Lat
        {
            get;
            private set;
        }
        public double Lng
        {
            get;
            private set;
        }

        public double Speed
        {
            get;
            private set;
        }

        public double Altitude
        {
            get;
            private set;
        }

        public double Heading
        {
            get;
            private set;
        }

        public double Accuracy
        {
            get;
            private set;
        }

        public double HeadingAccuracy
        {
            get;
            private set;
        }

        public double AltitudeAccuracy
        {
            get;
            private set;
        }
    }

    public class LocationService : ILocationService

    {
        private readonly IMvxLocationWatcher _watcher;
        private readonly IMvxMessenger _messenger;

        public LocationService(IMvxLocationWatcher watcher, IMvxMessenger messenger)
        {
            _watcher = watcher;
            _messenger = messenger;
            _watcher.Start(new MvxLocationOptions() { Accuracy = MvxLocationAccuracy.Fine, TrackingMode = MvxLocationTrackingMode.Background}, OnLocation, OnError);
  
        }


        private void OnLocation(MvxGeoLocation location)
        {
            MvxMessage message = new LocationMessage(this,
                                                location.Coordinates.Latitude,
                                                location.Coordinates.Longitude,
                                                string.IsNullOrEmpty(location.Coordinates.Speed.ToString()) ? 0: location.Coordinates.Speed.Value,
                                                string.IsNullOrEmpty(location.Coordinates.Altitude.ToString()) ? 0 : location.Coordinates.Altitude.Value,
                                                string.IsNullOrEmpty(location.Coordinates.Heading.ToString()) ? 0 : location.Coordinates.Heading.Value,
                                                string.IsNullOrEmpty(location.Coordinates.Accuracy.ToString()) ? 0 : location.Coordinates.Accuracy.Value,
                                                string.IsNullOrEmpty(location.Coordinates.HeadingAccuracy.ToString()) ? 0 : location.Coordinates.HeadingAccuracy.Value,
                                                string.IsNullOrEmpty(location.Coordinates.AltitudeAccuracy.ToString()) ? 0 : location.Coordinates.AltitudeAccuracy.Value
                                                );

            _messenger.Publish(message);
        }

        private void OnError(MvxLocationError error)
        {
            var x = error;
        }
    }
}
