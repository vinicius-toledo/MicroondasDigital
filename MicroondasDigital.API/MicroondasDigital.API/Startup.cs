using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Text;
using System.Web.Http;

[assembly: OwinStartup(typeof(MicroondasDigital.API.Startup))]
namespace MicroondasDigital.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var key = Encoding.UTF8.GetBytes("Chave_Muito_Secreta_Para_O_Microondas_2026");

            // 1. Configura o leitor de Token JWT
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "MicroondasAPI",
                    ValidAudience = "MicroondasApp",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });

            // 2. Configura as rotas e manda a API confiar no Token
            HttpConfiguration config = new HttpConfiguration();

            // ESSAS DUAS LINHAS SALVAM A VIDA: Elas forçam a API a usar o seu JWT
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}