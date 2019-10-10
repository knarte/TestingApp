

namespace BeSafe.Core.Models.Provider
{
    using SQLite;
    using System;
    using System.Collections.Generic;

    [Table("Zone")]
    public class Zone
    {

        public string Id { get; set; }


        public DateTime? ActiveFrom { get; set; }


        public DateTime? ActiveTo { get; set; }
        public string Comment { get; set; }
        [Ignore]
        public bool? Displayed { get; set; }
        public string ExternalReference { get; set; }
        [Ignore]
        public bool? MustIdentifyStops { get; set; }
        public string Name { get; set; }
        public double? CentroidLongitude { get; set; }
        public double? CentroidLatitude { get; set; }
        public string Hash { get; set; }
        public double Radius { get; set; }
        public bool? IsRural { get; set; }
        [Ignore]
        public bool IsInsideOfZone { get; set; }

        [Ignore]
        public List<Group> Groups { get; set; }

        [Ignore]
        public List<Coordinate> Points { get; set; }

        [Ignore]
        public List<ZoneType> ZoneTypes { get; set; }

        [Ignore]
        public List<Coordinate> Extent { get; set; }
    }
}
