using System;
using System.Text;

namespace MicroondasDigital.Models
{
    public class MicroondasModel
    {
     
        private int _tempo;
        private int _potencia;
    
        private const int TempoMaximo = 120;
        private const int TempoMinimo = 1;
        private const int PotenciaPadrao = 10;

        public string IniciarAquecimento(int segundos, int? potenciaInput)
        {
            if (segundos < TempoMinimo || segundos > TempoMaximo)
            {
                return "Erro: O tempo deve ser entre 1 e 120 segundos.";
            }

            _tempo = segundos;
            _potencia = potenciaInput ?? PotenciaPadrao;

            StringBuilder progresso = new StringBuilder();
            for (int i = 0; i < _tempo; i++)
            {
                progresso.Append(".");
            }

            return progresso.ToString() + " Aquecimento concluído";
        }
    }
}