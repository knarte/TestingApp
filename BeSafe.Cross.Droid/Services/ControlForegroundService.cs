using System;
using Android.App;
using Android.Content;
using Android.Util;
using BeSafe.Core.Helpers;
using BeSafe.Core.Interfaces;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace BeSafe.Cross.Droid.Services
{
    public class ControlForegroundService : IControlForegroundService
    {
        static readonly string TAG = typeof(ControlForegroundService).FullName;

        Intent startServiceIntent;
        Intent stopServiceIntent;
        LocationForegroundService locationForegroundService;
        static Activity act;
        static Context context;
        bool isStarted = false;

        public void CreateService(bool? savedInstanceState)
        {
            this.locationForegroundService = new LocationForegroundService();

            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            act = top.Activity;
            context = act;

            OnNewIntent(act.Intent);
            
            if (savedInstanceState != null)
            {
                isStarted = savedInstanceState.Value;
            }

            startServiceIntent = new Intent(act, typeof(LocationForegroundService));
            startServiceIntent.SetAction(Constant.ACTION_START_SERVICE);

            stopServiceIntent = new Intent(act, typeof(LocationForegroundService));
            stopServiceIntent.SetAction(Constant.ACTION_STOP_SERVICE);

        }

        protected void OnNewIntent(Intent intent)
        {
            if (intent == null)
            {
                return;
            }

            var bundle = intent.Extras;
            if (bundle != null)
            {
                if (bundle.ContainsKey(Constant.SERVICE_STARTED_KEY))
                {
                    isStarted = true;
                }
            }
        }

        public bool StopServices()
        {
            try
            {
                Log.Info(TAG, "User requested that the service be stopped.");
                context.StopService(stopServiceIntent);
                isStarted = false;
                return isStarted;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool StartServices()
        {
            try
            {
                Log.Info(TAG, "User requested that the service be started.");
                context.StartService(startServiceIntent);               
                isStarted = true;
                return isStarted;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void DestroitService()
        {

        }

    }
}