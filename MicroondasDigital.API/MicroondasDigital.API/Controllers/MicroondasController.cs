using System.Web.Http;
using MicroondasDigital.Models;

namespace MicroondasDigital.API.Controllers
{
    [RoutePrefix("api/microondas")]
    [Authorize]
    public class MicroondasController : ApiController
    {

        private readonly MicroondasModel _model = new MicroondasModel();


        [HttpGet]
        [Route("programas")]
        public IHttpActionResult ObterProgramas()
        {
            var programas = _model.ObterTodosOsProgramas();
            return Ok(programas);
        }

        [HttpGet]
        [Route("programas/customizados")]
        public IHttpActionResult ObterProgramasCustomizados()
        {
            var programas = _model.ObterProgramasCustomizados();
            return Ok(programas);
        }

        [HttpPost]
        [Route("programas")]
        public IHttpActionResult CadastrarPrograma([FromBody] ProgramaAquecimento programa)
        {
            if (programa == null)
                return BadRequest("Dados inválidos.");

            string resultado = _model.SalvarProgramaCUstomizado(programa);

            if (resultado.StartsWith("Erro"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost]
        [Route("aquecer")]
        public IHttpActionResult Aquecer([FromBody] AquecimentoRequest request)
        {
            if (request == null)
                return BadRequest("Dados inválidos.");

            string resultado = _model.Aquecer(request.Segundos, request.Potencia);

            if (resultado.StartsWith("Erro"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPost]
        [Route("aquecer/programa")]
        public IHttpActionResult AquecerPorPrograma([FromBody] AquecimentoProgramaRequest request)
        {
            if (request == null)
                return BadRequest("Dados inválidos.");

            string resultado = _model.Aquecer(
                request.Segundos,
                request.Potencia,
                caractere: request.Caractere,
                isPrograma: true
            );

            if (resultado.StartsWith("Erro"))
                return BadRequest(resultado);

            return Ok(resultado);
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