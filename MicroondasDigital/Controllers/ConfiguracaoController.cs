using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MicroondasDigital.Controllers
{
    public class ConfiguracaoController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UrlApi = Session["UrlApi"] ?? "https://localhost:44385/";
            ViewBag.Usuario = Session["Usuario"];

            ViewBag.EstaConectado = Session["Token"] != null;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SalvarConfiguracao(string urlApi, string usuario, string senha)
        {
            try
            {
                if (!urlApi.EndsWith("/")) urlApi += "/";

                using (var client = new HttpClient())
                {
                    var loginData = new { Usuario = usuario, Senha = senha };
                    var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{urlApi}api/auth/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(responseString);

                        Session["Token"] = (string)data.token;
                        Session["UrlApi"] = urlApi;
                        Session["Usuario"] = usuario;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Session["Token"] = null;
                        TempData["MensagemErro"] = "Falha na autenticação. Verifique usuário e senha.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao conectar na API: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Deslogar()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}