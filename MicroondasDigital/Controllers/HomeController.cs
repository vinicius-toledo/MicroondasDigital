using System.Linq;
using System.Web.Mvc;
using MicroondasDigital.Models;

namespace MicroondasDigital.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarPrograma(string nomePrograma)
        {
            var microondas = new MicroondasModel();
            var programa = microondas.ObterTodosOsProgramas().FirstOrDefault(p => p.Nome == nomePrograma);

            if (programa != null)
            {
                ViewBag.Segundos = programa.Tempo;
                ViewBag.Potencia = programa.Potencia;
                ViewBag.ProgramaAtivo = true;
                ViewBag.Caractere = programa.Caractere;
                ViewBag.Instrucao = programa.Instrucoes;
                ViewBag.NomePrograma = programa.Nome;

                string resultado = microondas.Aquecer(programa.Tempo, programa.Potencia, 0, programa.Caractere, true);

                ViewBag.Resultado = resultado;
                ViewBag.TempoFormatado = microondas.FormatarTempo(programa.Tempo);
                ViewBag.TempoRestante = programa.Tempo;
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Iniciar(int? segundos, int? potencia, int? tempoRestanteAtual, bool programaAtivo = false, char caractere = '.')
        {
            var microondas = new MicroondasModel();

            int tempoBase = (segundos == null || segundos == 0) ? 30 : segundos.Value;
            int tempoFinal = (tempoRestanteAtual ?? 0) > 0 ? (tempoRestanteAtual.Value + 30) : tempoBase;

            string resultado = microondas.Aquecer(segundos, potencia, tempoRestanteAtual, caractere, programaAtivo);

            ViewBag.Resultado = resultado;
            ViewBag.TempoFormatado = microondas.FormatarTempo(tempoFinal);
            ViewBag.Segundos = segundos;
            ViewBag.Potencia = potencia ?? 10;

            if (!resultado.StartsWith("Erro"))
                ViewBag.TempoRestante = tempoFinal;
            else
                ViewBag.TempoRestante = tempoRestanteAtual;

            ViewBag.ProgramaAtivo = programaAtivo;
            ViewBag.Caractere = caractere;

            return View("Index");
        }

        [HttpPost]
        public ActionResult PausarCancelar(int? tempoRestante, bool jaPausado)
        {
            if (jaPausado || tempoRestante == null || tempoRestante == 0)
            {
                ViewBag.Resultado = null;
                ViewBag.Segundos = null;
                ViewBag.Potencia = 10;
                ViewBag.TempoRestante = 0;
                ViewBag.Pausado = false;
                ViewBag.ProgramaAtivo = false;
            }
            else
            {
                ViewBag.Resultado = "Aquecimento pausado...";
                ViewBag.TempoRestante = tempoRestante;
                ViewBag.Pausado = true;
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult CadastrarPrograma(string nome, string alimento, int tempo, int potencia, char caractere, string instrucoes)
        {
            var microondas = new MicroondasModel();

            var novoPrograma = new ProgramaAquecimento(
                nome: nome,
                alimento: alimento,
                tempo: tempo,
                potencia: potencia,
                caractere: caractere,
                instrucoes: instrucoes ?? "",
                alimentoCustomizado: true
            );

            string resultado = microondas.SalvarProgramaCUstomizado(novoPrograma);

            if (resultado.StartsWith("Erro"))
                ViewBag.ErroCadastro = resultado;
            else
                ViewBag.SucessoCadastro = resultado;

            return View("Index");
        }
    }
}