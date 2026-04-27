using System;
using System.IO;
using System.Web;

namespace MicroondasDigital.API.Models
{
    public static class GerenciadorErros
    {
        public static void RegistrarErro(Exception ex)
        {
            try
            {
                string pastaLog = HttpContext.Current.Server.MapPath("~/App_Data");

                if (!Directory.Exists(pastaLog)) Directory.CreateDirectory(pastaLog);

                string caminhoArquivo = Path.Combine(pastaLog, "logs_erro.txt");

                string mensagem = $"\r\n[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] ------------------\r\n" +
                                 $"MENSAGEM: {ex.Message}\r\n" +
                                 $"TIPO: {ex.GetType().Name}\r\n" +
                                 $"STACKTRACE: {ex.StackTrace}\r\n" +
                                 $"INNER: {ex.InnerException?.Message}\r\n" +
                                 $"{new string('-', 30)}\r\n";

                File.AppendAllText(caminhoArquivo, mensagem);
            }
            catch
            {
            }
        }
    }
}