using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyStore.Models;
using FidoU2f.Models;
using System.Data.Entity;

namespace KeyStore.Services.Interfaces
{
   public interface IDataContext
    {
        int saveChanges();

        DbSet<NewUserViewModel> Users { get; set; }
        DbSet<RegisterNewDeviceViewModel> Devices { get; set; }
        DbSet<FidoStartedAuthentication> Authentication { get; set; }


    }
}
