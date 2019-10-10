using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace BeSafe.Core.Models.DB
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("IdEvent")]
        public int IdEvent { get; set; }

        [JsonProperty("EventType")]
        public int EventType { get; set; }

        [JsonProperty("CreationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("FinishEvent")]
        public DateTime? FinishEvent { get; set; }

        [JsonProperty("Distance")]
        public double Distance { get; set; }

        [JsonProperty("IsNotified")]
        public int IsNotified { get; set; }

        [JsonProperty("NotifiedDate")]
        public DateTime? NotifiedDate { get; set; }

        [JsonProperty("ValueUser")]
        public double? ValueUser { get; set; }

        [JsonProperty("ValueRef")]
        public double? ValueRef { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("InitialGPSUserData")]
        public int? InitialGPSUserData { get; set; }

        [JsonProperty("EndGPSUserData")]
        public int? EndGPSUserData { get; set; }
    }
}