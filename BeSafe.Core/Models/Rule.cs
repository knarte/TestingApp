using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models
{
    public enum ExceptionRuleBaseType
    {
        //
        // Resumen:
        //     Unknown base type.
        Unknown = -1,
        //
        // Resumen:
        //     Custom Exception rule. All user created rules are custom rules.
        Custom = 0,
        //
        // Resumen:
        //     Route Definition base rule
        RouteDefinition = 23,
        //
        // Resumen:
        //     Stock (canned) exception rule. These are the common rules available to switch
        //     on/off in MyGeotab.
        Stock = 25,
        //
        // Resumen:
        //     Zone stop rule. When a Geotab.Checkmate.ObjectModel.Zone's MustIdentifyStops
        //     property is set to true, the system creates a rule to identify when a device
        //     is stopped in the zone. These rules are of type ZoneStop.
        ZoneStop = 26
    }
    public class Rule
    {
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public ExceptionRuleBaseType? BaseType { get; set; }
        public string Comment { get; set; }
        public IList<Group> Groups { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Version { get; set; }
    }
}
