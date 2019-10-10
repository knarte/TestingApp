using System;
using SQLite;
namespace BeSafe.Core.Models.DB
{
    [Table("GPSUserData")]
    public class GPSUserData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Speed { get; set; }
        public double Distance { get; set; }
        public string UserName { get; set; }
        public DateTime DtmEvent { get; set; }
        public double? StopEnd { get; set; }
        public double? StopFlag { get; set; }
    }
}