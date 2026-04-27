using System;

namespace MicroondasDigital.API.Models
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem) : base(mensagem) { }
    }
}