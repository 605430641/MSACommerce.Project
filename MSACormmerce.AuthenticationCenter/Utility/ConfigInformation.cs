using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSACormmerce.AuthenticationCenter.Utility
{
    public class ConfigInformation
    {
        public string RootUrl { get; set; }

        public string UserUrl { get; set; }

        public JWTTokenOptions JWTTokenOptions { get; set; }
    }


    public class JWTTokenOptions
    {
        public string Audience
        {
            get;
            set;
        }
        public string SecurityKey
        {
            get;
            set;
        }
        //public SigningCredentials Credentials
        //{
        //    get;
        //    set;
        //}
        public string Issuer
        {
            get;
            set;
        }
    }
}
