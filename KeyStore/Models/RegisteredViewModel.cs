using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FidoU2f.Models;

namespace KeyStore.Models
{
    public class RegistrationsViewModel
    {
        public ICollection<FidoStartedRegistration> StartedRegistrations { get; set; }

        public ICollection<FidoDeviceRegistration> DeviceRegistrations { get; set; }
        public ICollection<NewUserViewModel> UserRegistration { get; set; }


        public RegistrationsViewModel()
        {
            StartedRegistrations = new List<FidoStartedRegistration>();
            DeviceRegistrations = new List<FidoDeviceRegistration>();
            UserRegistration = new List<NewUserViewModel>();
        }
    }
}