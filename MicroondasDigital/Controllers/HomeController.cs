using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MicroondasDigital.Models;
using Newtonsoft.Json;

namespace MicroondasDigital.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient ObterClienteApi()
        {
            var token = Session["Token"] as string;
            var urlBase = Session["UrlApi"] as string;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(urlBase))
                return null;

            var client = new HttpClient();
            client.BaseAddress = new Uri(urlBase);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        // NOVO: Método ajudante que sempre recarrega os botões da API
        private async Task CarregarProgramasNaTela()
        {
            var client = ObterClienteApi();
            if (client != null)
            {
                try
                {
                    var response = await client.GetAsync("api/microondas/programas");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        ViewBag.Programas = JsonConvert.DeserializeObject<List<ProgramaAquecimento>>(json);
                    }
                }
                catch
                {
                    ViewBag.Programas = null;
                }
            }
        }

        public async Task<ActionResult> Index()
        {
            if (Session["Token"] == null)
            {
                return RedirectToAction("Index", "Configuracao");
            }

            // Carrega os botões quando entra na página
            await CarregarProgramasNaTela();

            ViewBag.SucessoCadastro = TempData["SucessoCadastro"];
            ViewBag.ErroCadastro = TempData["ErroCadastro"];

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IniciarPrograma(string nomePrograma)
        {
            var client = ObterClienteApi();
            if (client == null)
            {
                ViewBag.Resultado = "Erro: API não configurada ou Token expirado.";
                await CarregarProgramasNaTela();
                return View("Index");
            }

            // AGORA SIM: Busca o programa na lista da API, que inclui os customizados!
            await CarregarProgramasNaTela();
            var programas = ViewBag.Programas as List<ProgramaAquecimento>;
            var programa = programas?.FirstOrDefault(p => p.Nome == nomePrograma);

            if (programa != null)
            {
                ViewBag.Segundos = programa.Tempo;
                ViewBag.Potencia = programa.Potencia;
                ViewBag.ProgramaAtivo = true;
                ViewBag.Caractere = programa.Caractere;
                ViewBag.Instrucao = programa.Instrucoes;
                ViewBag.NomePrograma = programa.Nome;

                var requestData = new
                {
                    Segundos = programa.Tempo,
                    Potencia = programa.Potencia,
                    Caractere = programa.Caractere
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync("api/microondas/aquecer/programa", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultadoDaApi = await response.Content.ReadAsStringAsync();
                        ViewBag.Resultado = resultadoDaApi.Trim('"');
                        ViewBag.TempoFormatado = new MicroondasModel().FormatarTempo(programa.Tempo);
                        ViewBag.TempoRestante = programa.Tempo;
                    }
                    else
                    {
                        var errorMsg = await response.Content.ReadAsStringAsync();
                        ViewBag.Resultado = $"Erro da API ({response.StatusCode}): {errorMsg.Trim('"')}";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Resultado = $"Erro de conexão com a API: {ex.Message}";
                }
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Iniciar(int? segundos, int? potencia, int? tempoRestanteAtual, bool programaAtivo = false, char caractere = '.')
        {
            var client = ObterClienteApi();
            if (client == null)
            {
                ViewBag.Resultado = "Erro: API não configurada.";
                await CarregarProgramasNaTela();
                return View("Index");
            }

            int tempoBase = (segundos == null || segundos == 0) ? 30 : segundos.Value;
            int tempoFinal = (tempoRestanteAtual ?? 0) > 0 ? (tempoRestanteAtual.Value + 30) : tempoBase;

            var requestData = new
            {
                Segundos = tempoFinal,
                Potencia = potencia
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("api/microondas/aquecer", content);

                if (response.IsSuccessStatusCode)
                {
                    var resultadoDaApi = await response.Content.ReadAsStringAsync();
                    ViewBag.Resultado = resultadoDaApi.Trim('"');
                    ViewBag.TempoFormatado = new MicroondasModel().FormatarTempo(tempoFinal);
                    ViewBag.Segundos = segundos;
                    ViewBag.Potencia = potencia ?? 10;
                    ViewBag.TempoRestante = tempoFinal;
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ViewBag.Resultado = $"Erro da API ({response.StatusCode}): {errorMsg.Trim('"')}";
                    ViewBag.TempoRestante = tempoRestanteAtual;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Resultado = $"Erro de conexão com a API: {ex.Message}";
                ViewBag.TempoRestante = tempoRestanteAtual;
            }

            ViewBag.ProgramaAtivo = programaAtivo;
            ViewBag.Caractere = caractere;

            // Recarrega os botões antes de desenhar a tela
            await CarregarProgramasNaTela();
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> PausarCancelar(int? tempoRestante, bool jaPausado)
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

            // Recarrega os botões antes de desenhar a tela
            await CarregarProgramasNaTela();
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarPrograma(string nome, string alimento, int tempo, int potencia, string caractere, string instrucoes)
        {
            var client = ObterClienteApi();
            if (client == null)
            {
                TempData["ErroCadastro"] = "Erro: API não configurada.";
                return RedirectToAction("Index");
            }

            char c = !string.IsNullOrEmpty(caractere) ? caractere[0] : '.';

            var novoPrograma = new ProgramaAquecimento(
                nome: nome,
                alimento: alimento,
                tempo: tempo,
                potencia: potencia,
                caractere: c,
                instrucoes: instrucoes ?? "",
                alimentoCustomizado: true
            );

            var content = new StringContent(JsonConvert.SerializeObject(novoPrograma), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("api/microondas/programas", content);

                if (response.IsSuccessStatusCode)
                {
                    var resultadoDaApi = await response.Content.ReadAsStringAsync();
                    TempData["SucessoCadastro"] = resultadoDaApi.Trim('"');
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    TempData["ErroCadastro"] = $"Erro da API ({response.StatusCode}): {errorMsg.Trim('"')}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErroCadastro"] = $"Erro de conexão com a API: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}