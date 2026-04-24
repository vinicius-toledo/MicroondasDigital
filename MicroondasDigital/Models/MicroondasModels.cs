using System;
using System.Collections.Generic;
using System.Text;

namespace MicroondasDigital.Models
{
    public class MicroondasModel
    {
        private const int PotenciaPadrao = 10;
        private const int TempoMaximo = 120;
        private const int TempoMinimo = 1;


        public IReadOnlyList<ProgramaAquecimento> ProgramasPadrao { get; } = new List<ProgramaAquecimento>
        {
           new ProgramaAquecimento(
               nome: "Pipoca",
               alimento: "Pipoca (de micro-ondas)",
               tempo: 180,
               potencia: 7,
               caractere: '!',
               instrucoes: "Observar o barulho de estouros do milho, caso houverum intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento"
               ),

            new ProgramaAquecimento(
                nome: "Leite",
                alimento:"Leite",
                tempo: 300,
                potencia: 5,
                caractere: '@',
                instrucoes:"Cuidado com aquecimento de liquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras"
                ),
            
            new ProgramaAquecimento(
                nome: "Carne de boi",
                alimento:"Carne em pedaço ou fatias",
                tempo: 840,
                potencia: 4,
                caractere: '#',
                instrucoes:"Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."
                ),

             new ProgramaAquecimento(
                nome: "Frango",
                alimento:"Frango ( qualquer corte)",
                tempo: 480,
                potencia: 7,
                caractere: '$',
                instrucoes:"Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."
                ),

             new ProgramaAquecimento(
                nome: "Feijão",
                alimento:"Feijão congelado",
                tempo: 480,
                potencia: 9,
                caractere: '%',
                instrucoes:"Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas."
                )
        };

        public string Aquecer(int? segundos, int? potenciaInput, int? tempoRestanteAtual = 0)
        {

            if(tempoRestanteAtual >0)
            {
                int novoTempo = tempoRestanteAtual.Value + 30;

                if(novoTempo > 120) 
                {
                  return "Erro: O tempo total não pode exceder 120 segundos.";
                }

                return GerarStringAquecimento(novoTempo, potenciaInput ?? 10, '.');

            }
  
            int tempo = (segundos == null || segundos == 0) ? 30 : segundos.Value;
            int potencia = (potenciaInput == null || potenciaInput == 0) ? PotenciaPadrao : potenciaInput.Value;

            if (tempo < TempoMinimo || tempo > TempoMaximo)
                return "Erro: O tempo deve ser entre 1 e 120 segundos.";

            if (potencia < 1 || potencia > 10)
                return "Erro: A potência deve ser entre 1 e 10.";

            return GerarStringAquecimento(tempo, potencia, '.');
        }

        public string FormatarTempo(int segundos)
        {
            if (segundos >= 60)
            {
                int minutos = segundos / 60;
                int segundosRestantes = segundos % 60;
                return $"{minutos}:{segundosRestantes:D2}";
            }

            return segundos.ToString() + "s";
        }

        private string GerarStringAquecimento(int tempo, int potencia, char caractere)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tempo; i++)
            {
                for (int p = 0; p < potencia; p++)
                {
                    sb.Append(caractere);
                }
                sb.Append(" ");
            }

            return sb.ToString() + " Aquecimento concluído";
        }
    }
}