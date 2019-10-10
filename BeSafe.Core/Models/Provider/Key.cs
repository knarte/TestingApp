using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models.Provider
{
    public enum DriverKeyType
    {
        Usb = 0,
        Nfc = 1,
        CustomNfc = 2
    }

    public class Key
    {
        public DriverKeyType? DriverKeyType { get; set; }
        public long? KeyId { get; set; }
        public string SerialNumber { get; set; }
    }
}
