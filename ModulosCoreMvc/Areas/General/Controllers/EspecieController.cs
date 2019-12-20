using SERFOR.Component.DTEntities.Especie;
using SERFOR.Component.GeneralCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Modulos_Core_MVC.Areas.General.Controllers
{
    public class EspecieController : Controller
    {
        [HttpPost]
        public JsonResult Search(string texto)
        {
            string mensaje = string.Empty;
            IEnumerable<EspecieTableRowDTe> list = null;

            try
            {
                list = EspecieFacade.GetBySearchText(texto);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(list);
        }
    }
}