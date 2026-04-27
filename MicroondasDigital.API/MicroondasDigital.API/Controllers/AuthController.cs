using System;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MicroondasDigital.API.Models; 

namespace MicroondasDigital.API.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
      
        private const string SecretKey = "Chave_Muito_Secreta_Para_O_Microondas_2026";

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest login)
        {
            
            string senhaCadastradaHash = SegurancaUtils.HashSenha("admin123");

            string senhaDigitadaHash = SegurancaUtils.HashSenha(login.Senha);

            if (login.Usuario == "admin" && senhaDigitadaHash == senhaCadastradaHash)
            {
                var token = GerarTokenJWT(login.Usuario);
                return Ok(new { token = token });
            }

            return Unauthorized();
        }

        private string GerarTokenJWT(string usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: "MicroondasAPI",
                audience: "MicroondasApp",
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}