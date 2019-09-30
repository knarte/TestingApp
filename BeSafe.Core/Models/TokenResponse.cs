
namespace BeSafe.Core.Models
{
    using System;
    using Newtonsoft.Json;

    public class TokenResponse
    {
        
        [JsonProperty("User")]
        public User User { get; set; }

        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("BaseId")]
        public int BaseId { get; set; }
    }
}
