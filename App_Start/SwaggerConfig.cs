using System.Web.Http;
using WebActivatorEx;
using WebEndProject;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebEndProject
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration.EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "WebEndProject");
                        //swagger enable
                        c.IncludeXmlComments(string.Format(@"{0}\bin\WebEndProject.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                        c.SchemaId(x => x.FullName);
                    }).EnableSwaggerUi(c =>
                    {
                        
                    });
        }
    }
}
