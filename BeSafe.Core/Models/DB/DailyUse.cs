using System;
using Newtonsoft.Json;

namespace BeSafe.Core.Models.DB
{
    public class DailyUse
    {
        [JsonProperty("CalendarDay")]
        public DateTime CalendarDay { get; set; }

        [JsonProperty("CalendarTime")]
        public DateTime CalendarTime { get; set; }

        [JsonProperty("TotalReg")]
        public int TotalReg { get; set; }
    }
}
