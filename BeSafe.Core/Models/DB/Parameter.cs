using System;
using Newtonsoft.Json;

namespace BeSafe.Core.Models.DB
{
    public class Parameter
    {
        [JsonProperty("Parameter")]
        public string Parameters { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
