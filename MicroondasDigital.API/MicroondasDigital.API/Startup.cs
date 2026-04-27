using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using System.Net.Http;
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
            HttpConfiguration config = new HttpConfiguration();
            var key = Encoding.UTF8.GetBytes("Chave_Muito_Secreta_Para_O_Microondas_2026");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
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

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config); 
        }
    }
}