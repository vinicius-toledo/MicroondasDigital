using System;
using System.Text;

namespace MicroondasDigital.Models
{
    public class MicroondasModel
    {
        private const int PotenciaPadrao = 10;
        private const int TempoMaximo = 120;
        private const int TempoMinimo = 1;

        public string Aquecer(int? segundos, int? potenciaInput)
        {
  
            int tempo = (segundos == null || segundos == 0) ? 30 : segundos.Value;
            int potencia = (potenciaInput == null || potenciaInput == 0) ? PotenciaPadrao : potenciaInput.Value;

            if (tempo < TempoMinimo || tempo > TempoMaximo)
                return "Erro: O tempo deve ser entre 1 e 120 segundos.";

            if (potencia < 1 || potencia > 10)
                return "Erro: A potência deve ser entre 1 e 10.";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tempo; i++)
            {
                for (int p = 0; p < potencia; p++)
                {
                    sb.Append(".");
                }
                sb.Append(" "); 
            }

            return sb.ToString() + " Aquecimento concluído";
        }
    }
}