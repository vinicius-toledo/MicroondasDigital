using System;
using System.Web.Http;
using MicroondasDigital.Models;
using MicroondasDigital.API.Models; 
namespace MicroondasDigital.API.Controllers
{
    [RoutePrefix("api/microondas")]
    [Authorize]
    public class MicroondasController : ApiController
    {
        private readonly MicroondasModel _model = new MicroondasModel();

        private IHttpActionResult ResponderErro(Exception ex)
        {
            GerenciadorErros.RegistrarErro(ex);

            if (ex is NegocioException)
            {
                return BadRequest(ex.Message); 
            }

            return InternalServerError(); 
        }

        [HttpGet]
        [Route("programas")]
        public IHttpActionResult ObterProgramas()
        {
            try
            {
                var programas = _model.ObterTodosOsProgramas();
                return Ok(programas);
            }
            catch (Exception ex) { return ResponderErro(ex); }
        }

        [HttpPost]
        [Route("programas")]
        public IHttpActionResult CadastrarPrograma([FromBody] ProgramaAquecimento programa)
        {
            try
            {
                if (programa == null)
                    throw new NegocioException("Os dados do programa não foram informados.");

                string resultado = _model.SalvarProgramaCUstomizado(programa);

                if (resultado.StartsWith("Erro"))
                    throw new NegocioException(resultado);

                return Ok(resultado);
            }
            catch (Exception ex) { return ResponderErro(ex); }
        }

        [HttpPost]
        [Route("aquecer")]
        public IHttpActionResult Aquecer([FromBody] AquecimentoRequest request)
        {
            try
            {
                if (request == null)
                    throw new NegocioException("Dados de aquecimento inválidos.");

                string resultado = _model.Aquecer(request.Segundos, request.Potencia);

                if (resultado.StartsWith("Erro"))
                    throw new NegocioException(resultado);

                return Ok(resultado);
            }
            catch (Exception ex) { return ResponderErro(ex); }
        }

        [HttpPost]
        [Route("aquecer/programa")]
        public IHttpActionResult AquecerPorPrograma([FromBody] AquecimentoProgramaRequest request)
        {
            try
            {
                if (request == null)
                    throw new NegocioException("Dados do programa inválidos.");

                string resultado = _model.Aquecer(
                    request.Segundos,
                    request.Potencia,
                    caractere: request.Caractere,
                    isPrograma: true
                );

                if (resultado.StartsWith("Erro"))
                    throw new NegocioException(resultado);

                return Ok(resultado);
            }
            catch (Exception ex) { return ResponderErro(ex); }
        }
    }

    public class AquecimentoRequest
    {
        public int? Segundos { get; set; }
        public int? Potencia { get; set; }
    }

    public class AquecimentoProgramaRequest
    {
        public int? Segundos { get; set; }
        public int? Potencia { get; set; }
        public char Caractere { get; set; }
    }
}