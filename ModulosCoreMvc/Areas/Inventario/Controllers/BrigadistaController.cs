using SERFOR.Component.DTEntities.Inventario;
using SERFOR.Component.InventarioCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Modulos_Core_MVC.Areas.Inventario.Controllers
{
    public class BrigadistaController : Controller
    {
        // GET: Inventario/Brigadista
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SearchPersonas(string searchText)
        {
            IEnumerable<Persona> list = new List<Persona>();

            list = BrigadistaFacade.GetBySearchText(searchText);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}