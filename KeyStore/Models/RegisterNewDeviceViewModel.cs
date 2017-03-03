using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyStore.Models
{
    public class RegisterNewDeviceViewModel
    {
        public string AppId { get; set; }
        public string Email { get; set; }
        public string Challenge { get; set; }
        public string UserName { get; set; }
        public string RawRegisterResponse { get; set; }
    }
}