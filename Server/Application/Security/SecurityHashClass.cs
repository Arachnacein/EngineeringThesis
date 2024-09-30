using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Security
{
    class SecurityHashClass : ISecurityHashClass
    {
        public string hashPassword(string password)
        {
            var sha256 = SHA256.Create();
            var getBytes = Encoding.Default.GetBytes(password);
            var hashPassword = sha256.ComputeHash(getBytes);
            return Convert.ToBase64String(hashPassword);
        }
    }
}
