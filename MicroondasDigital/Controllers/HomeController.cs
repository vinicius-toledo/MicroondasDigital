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
        public ActionResult Iniciar(int? segundos, int? potencia, int? tempoRestanteAtual)
        {
            var microondas = new MicroondasModel();

            int tempoBase = (segundos == null || segundos == 0) ? 30 : segundos.Value;
            int tempoFinal = (tempoRestanteAtual ?? 0) > 0 ? (tempoRestanteAtual.Value + 30) : tempoBase;

            string resultado = microondas.Aquecer(segundos, potencia, tempoRestanteAtual);

            ViewBag.Resultado = resultado;
            ViewBag.TempoFormatado = microondas.FormatarTempo(tempoFinal); 
            ViewBag.Segundos = segundos;
            ViewBag.Potencia = potencia ?? 10;
            ViewBag.TempoRestante = tempoFinal;

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
            }
            else
            {
                ViewBag.Resultado = "Aquecimento pausado...";
                ViewBag.TempoRestante = tempoRestante;
                ViewBag.Pausado = true;
            }

            return View("Index");
        }
    }
}