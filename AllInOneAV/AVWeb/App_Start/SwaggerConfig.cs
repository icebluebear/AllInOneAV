using System.Web.Http;
using WebActivatorEx;
using AVWeb;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace AVWeb
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "AVWeb");

                    c.IncludeXmlComments(string.Format("{0}/bin/AVWeb.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                    //c.IncludeXmlComments(string.Format("{0}/bin/Model.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                })
                .EnableSwaggerUi(c =>
                {

                });
        }

    }
}
