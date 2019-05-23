using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCore.BLL.Infrastructure
{
    public class AuthOptions
    {
        public const string ISSUER = "http://localhost:44357"; 
        public const string AUDIENCE = "http://localhost:44357";
        const string KEY = "mysupersecret_secretkey!123";  
        public const int LIFETIME = 15; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
