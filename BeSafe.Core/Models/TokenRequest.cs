using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models
{
    public class TokenRequest
    {
        public string User { get; set; }

        public string Password { get; set; }

        public string BaseName { get; set; }

        public string GuidMobilDevice { get; set; }

        public string Language { get; set; }

        public string Platform { get; set; }
    }
}
