using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models
{
    public class DeviceStatusInfo
    {
        public int? Bearing { get; set; }
        public TimeSpan? CurrentStateDuration { get; set; }
        public DateTime? DateTime { get; set; }
        public Device Device { get; set; }
        public Driver Driver { get; set; }
        public List<ExceptionEvent> ExceptionEvents { get; set; }
        public IList<Group> Groups { get; set; }
        public bool IsDeviceCommunicating { get; set; }
        public bool IsDriving { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public float? Speed { get; set; }
        public string Name { get; set; }
    }
}
