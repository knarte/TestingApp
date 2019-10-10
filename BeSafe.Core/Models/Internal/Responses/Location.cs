using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models.Internal.Responses
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public double Accuracy { get; set; }
    }
}
