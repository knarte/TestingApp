using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models
{
    public class Group
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Comments")]
        public string Comments { get; set; }

        [JsonProperty("Children")]
        public List<Group> Children { get; set; }

        [JsonProperty("Parent")]
        public Group Parent { get; set; }

        [JsonProperty("Reference")]
        public string Reference { get; set; }
    }
}
