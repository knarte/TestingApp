using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models.Provider
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System;

    public enum DeviceType
    {
        None = 0,
        OldGeotab = 1,
        GO2 = 2,
        GO3 = 3,
        GO4 = 4,
        GO4v3 = 5,
        GO5 = 6,
        GO6 = 7,
        GO7 = 8,
        GO8 = 9,
        GO9 = 10,
        CustomDevice = 11,
        GoDriveDevice = 12
    }

    public partial class Device
    {

        [JsonProperty("ActiveTo")]
        public virtual DateTime? ActiveTo { get; set; }

        [JsonProperty("ActiveFrom")]
        public virtual DateTime? ActiveFrom { get; set; }

        [JsonProperty("Comment")]
        public string Comment { get; set; }

        [JsonProperty("MaxSecondsBetweenLogs")]
        public float? MaxSecondsBetweenLogs { get; set; }

        [JsonProperty("DeviceType")]
        public DeviceType? DeviceType { get; }

        [JsonProperty("HardwareId")]
        public int? HardwareId { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ProductId")]
        public virtual int? ProductId { get; set; }

        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("TimeZoneId")]
        public string TimeZoneId { get; set; }

        [JsonProperty("Groups")]
        public List<Group> Groups { get; set; }
    }
}
