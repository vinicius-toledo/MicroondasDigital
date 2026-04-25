using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroondasDigital.Models
{
    public class ProgramaAquecimento
    {
        public string Nome { get; private set; }
        public string Alimento { get; private set; }
        public int Tempo { get; private set; }
        public int Potencia { get; private set; }
        public char Caractere { get; private set; }
        public string Instrucoes { get; private set; }

        public bool AlimentoCustomizado { get; private set; }

        public ProgramaAquecimento(string nome, string alimento, int tempo, int potencia, char caractere, string instrucoes, bool alimentoCustomizado = false)
        {
            Nome = nome;
            Alimento = alimento;
            Tempo = tempo;
            Potencia = potencia;
            Caractere = caractere;
            Instrucoes = instrucoes;
            AlimentoCustomizado = alimentoCustomizado;
        }
    }
}