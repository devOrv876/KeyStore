using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyStore.Models
{
    public class NewUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public NewUserViewModel()
        {

        }

        public NewUserViewModel(string username, string email)
        {
            if (username == null) throw new ArgumentNullException("Username");
            if (email == null) throw new ArgumentNullException("Email");

            UserName = username;
            Email = email;
        }
    }
}