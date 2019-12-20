using Modulos_Core_MVC.Filters;
using System.Web;
using System.Web.Mvc;

namespace Modulos_Core_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorFilter());
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
        }
    }
}
