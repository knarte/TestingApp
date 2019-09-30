using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Helpers
{
    public class Endpoint
    {
        public static string URL_BASE = "https://navisafsdkapi-qa.azurewebsites.net";
    }

    public class Constant
    {
        public const int DELAY_BETWEEN_LOG_MESSAGES = 5000; // milliseconds
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string SERVICE_STARTED_KEY = "has_service_been_started";
        public const string BROADCAST_MESSAGE_KEY = "broadcast_message";
        public const string NOTIFICATION_BROADCAST_ACTION = "BeSafe.Cross.Droid.Services.Notification.Action";

        public const string ACTION_START_SERVICE = "BeSafe.Cross.Droid.Services.action.START_SERVICE";
        public const string ACTION_STOP_SERVICE = "BeSafe.Cross.Droid.Services.action.STOP_SERVICE";
        public const string ACTION_RESTART_TIMER = "BeSafe.Cross.Droid.Services.action.RESTART_TIMER";
        public const string ACTION_MAIN_ACTIVITY = "BeSafe.Cross.Droid.Services.action.MAIN_ACTIVITY";
    }
}
