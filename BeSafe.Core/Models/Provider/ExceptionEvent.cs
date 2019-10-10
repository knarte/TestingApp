using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models.Provider
{
    public class ExceptionEvent
    {
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public Device Device { get; set; }
        public float? Distance { get; set; }
        public Driver Driver { get; set; }
        public TimeSpan? Duration { get; }
        public Rule Rule { get; set; }
        public long? Version { get; set; }
        public string Id { get; set; }
    }
}
