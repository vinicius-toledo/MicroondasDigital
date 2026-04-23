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
        public ActionResult Iniciar(int? segundos, int? potencia)
        {
            var microondas = new MicroondasModel();

            string resultado = microondas.Aquecer(segundos, potencia);

            ViewBag.Resultado = resultado;

            return View("Index");

        }

    }
}