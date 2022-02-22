using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Server.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "PhoneticServer"; // издатель токена
        public const string AUDIENCE = "PhoneticClient"; // потребитель токена
        const string KEY = "p1h2o3n4e5t6i7c8";   // ключ для шифрации
        public const int LIFETIME = 360; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
