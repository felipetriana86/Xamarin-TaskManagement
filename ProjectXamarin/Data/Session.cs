using ProjectXamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectXamarin.Data
{
    public class Session
    {
        public Session()
        {

        }
        public Session(string authorizationToken)
        {
            AuthorizationToken = authorizationToken;
        }

        public UserModel LoggedUser { get; set; }
        public string AuthorizationToken { get; set; }


    }
}
