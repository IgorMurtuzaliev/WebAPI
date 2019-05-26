using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCore.BLL.Infrastructure
{
    public class AuthOptions
    {
        public const string ISSUER = "Issuer"; 
        public const string AUDIENCE = "Audience";
        const string KEY = "mysupersecret_secretkey!123";  
        public const int LIFETIME = 15; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
