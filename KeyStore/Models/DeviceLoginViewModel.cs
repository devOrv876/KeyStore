using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyStore.Models
{
  
        public class LoginDeviceViewModel
        {
            public string AppId { get; set; }
            public string Challenge { get; set; }
            public string UserName { get; set; }
            public string KeyHandle { get; set; }
            public string RawAuthenticationResponse { get; set; }
        }
}
