using System;
using Newtonsoft.Json;

namespace BeSafe.Core.Models.DB
{
    public class GPSEvaluated
    {
        [JsonProperty("IdDataSpeedCero")]
        public int IdDataSpeedCero { get; set; }

        [JsonProperty("IdDataDetention")]
        public int IdDataDetention { get; set; }

        [JsonProperty("LastBroadcastDate")]
        public DateTime LastBroadcastDate { get; set; }

        [JsonProperty("IdLastBroascast")]
        public int IdLastBroascast { get; set; }
    }
}
