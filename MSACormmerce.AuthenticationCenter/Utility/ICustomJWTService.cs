using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSACormmerce.AuthenticationCenter.Model;

namespace MSACormmerce.AuthenticationCenter.Utility
{
    public interface ICustomJWTService
    {
        string GetToken(string UserName, string password, User user);
    }
}
