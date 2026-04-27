using System;
using System.Security.Cryptography;
using System.Text;

namespace MicroondasDigital.API.Models
{
    public static class SegurancaUtils
    {
        public static string HashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}