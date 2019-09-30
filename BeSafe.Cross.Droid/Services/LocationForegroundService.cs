using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using BeSafe.Core.Helpers;

namespace BeSafe.Cross.Droid.Services
{
    [Service]
    public class LocationForegroundService : Service
    {
        static readonly string TAG = typeof(LocationForegroundService).FullName;

        Handler handler;
        Action runnable;
        bool isStarted;
        int count = 0;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "OnCreate: the service is initializing.");

            handler = new Handler();

            runnable = new Action(() =>
            {
               

            });
        }

        public override void OnDestroy()
        {
            Log.Debug(TAG, "Finish");
            Log.Info(TAG, "OnDestroy: The started service is shutting down");

            handler.RemoveCallbacks(runnable);

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(Constant.SERVICE_RUNNING_NOTIFICATION_ID);

            isStarted = false;
            base.OnDestroy();
        }

        //[return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals(Constant.ACTION_START_SERVICE))
            {
                if (isStarted)
                {
                    Log.Info(TAG, "OnStartCommand: The service is already running.");
                }
                else
                {
                    Log.Info(TAG, "OnStartCommand: The service is starting.");
                    RegisterForegroundService();
                    handler.PostDelayed(runnable, Constant.DELAY_BETWEEN_LOG_MESSAGES);
                    isStarted = true;
                }
            }
            else if (intent.Action.Equals(Constant.ACTION_STOP_SERVICE))
            {
                Log.Info(TAG, "OnStartCommand: The service is stopping.");
                //timestamper = null;
                StopForeground(true);
                StopSelf();
                isStarted = false;

            }
            else if (intent.Action.Equals(Constant.ACTION_RESTART_TIMER))
            {
                Log.Info(TAG, "OnStartCommand: Restarting the timer.");
                //timestamper.Restart();

            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }

        void RegisterForegroundService()
        {
            var notification = new Notification.Builder(this)
                .SetContentTitle(Resources.GetString(Resource.String.app_name))
                .SetContentText(Resources.GetString(Resource.String.notification_text))
                .SetSmallIcon(Resource.Drawable.ic_stat_name)
                //.SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                .AddAction(BuildRestartTimerAction())
                .AddAction(BuildStopServiceAction())
                .Build();


            // Enlist this instance of the service as a foreground service
            StartForeground(Constant.SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }

        /// <summary>
		/// Builds a Notification.Action that will instruct the service to restart the timer.
		/// </summary>
		/// <returns>The restart timer action.</returns>
		Notification.Action BuildRestartTimerAction()
        {
            var restartTimerIntent = new Intent(this, GetType());
            restartTimerIntent.SetAction(Constant.ACTION_RESTART_TIMER);
            var restartTimerPendingIntent = PendingIntent.GetService(this, 0, restartTimerIntent, 0);

            var builder = new Notification.Action.Builder(Resource.Drawable.ic_action_restart_timer,
                                              GetText(Resource.String.restart_timer),
                                              restartTimerPendingIntent);

            return builder.Build();
        }

        /// <summary>
        /// Builds the Notification.Action that will allow the user to stop the service via the
        /// notification in the status bar
        /// </summary>
        /// <returns>The stop service action.</returns>
        Notification.Action BuildStopServiceAction()
        {
            var stopServiceIntent = new Intent(this, GetType());
            stopServiceIntent.SetAction(Constant.ACTION_STOP_SERVICE);
            var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

            var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
                                                          GetText(Resource.String.stop_service),
                                                          stopServicePendingIntent);
            return builder.Build();

        }

        //void UpdateNotification(string content)
        //{
        //    var notification = GetNotification(content, pendingIntent);

        //    NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
        //    notificationManager.Notify(NOTIFICATION_ID, notification);
        //}

        //Notification GetNotification(string content, PendingIntent intent)
        //{
        //    return new Notification.Builder(this)
        //            .SetContentTitle(tag)
        //            .SetContentText(content)
        //            .SetSmallIcon(Resource.Drawable.NotifyLg)
        //            .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.Icon))
        //            .SetContentIntent(intent).Build();
        //}
    }
}