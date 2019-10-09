using System;
namespace BeSafe.Core.Models
{

    public class AuthorizedFunction
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public DateTime CreationDate { get; set; }
        public string IconPath { get; set; }
    }
    
}
