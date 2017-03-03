using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FidoU2f.Models;

namespace KeyStore.Services
{
    public interface IUserRepository
    {
        void StoreStartedRegistration(string userName, FidoStartedRegistration startedRegistration);

        FidoStartedRegistration GetStartedRegistration(string userName, string challenge);

        IEnumerable<FidoStartedRegistration> GetAllStartedRegistrationsOfUser(string userName);

        void RemoveStartedRegistration(string userName, string challenge);

        void StoreDeviceRegistration(string userName, FidoDeviceRegistration deviceRegistration);

        void UpdateDeviceRegistrationCounter(string userName, FidoKeyHandle keyHandle, uint counter);

        IEnumerable<FidoDeviceRegistration> GetDeviceRegistrationsOfUser(string userName);
    }
}
