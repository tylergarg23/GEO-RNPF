using System.Web.Mvc;

namespace Modulos_Core_MVC.Areas.ConsultasDIR
{
    public class ConsultasDIRAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ConsultasDIR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ConsultasDIR_default",
                "ConsultasDIR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}