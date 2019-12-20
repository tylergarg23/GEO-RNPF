using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modulos_Core_MVC.Security;
using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.BusinessLogic.Facade;

namespace Modulos_Core_MVC.Areas.Plantaciones.Controllers
{
    [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,CONSULTOR")]
    public class RNPController : Controller
    {
        // GET: Plantaciones/RNP
        public ActionResult Index()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }
        private string GetRol()
        {
            String result = "";
            if (User.IsInRole("CONSULTOR")) result = "CONSULTOR";
            if (User.IsInRole("REGISTRADOR")) result = "REGISTRADOR";
            if (User.IsInRole("ESPATFFS")) result = "ESPATFFS";
            if (User.IsInRole("ESPFORDIR")) result = "ESPFORDIR";
            if (User.IsInRole("ESPCATAST")) result = "ESPCATAST";
            if (User.IsInRole("ADMINPLNT")) result = "ADMINPLNT";
            if (User.IsInRole("ADMINSIS")) result = "ADMINSIS";
            return result;
        }
        public JsonResult GetRNP(string order, string offset = "-1", string limit = "-1", string search = "", string sort = "")
        {
            //IEnumerable<PlantacionItemListDTe> list = new List<PlantacionItemListDTe>();
            Resultado res = new Resultado();

            if (User.IsInRole("ESPFORDIR") || User.IsInRole("ESPCATAST") || User.IsInRole("ADMINPLNT"))
            {
                res = PlantacionFacade.GetRNP(order, int.Parse(offset), int.Parse(limit), search, sort);
            }
            else
            {
                if (User.IsInRole("ESPATFFS") || User.IsInRole("REGISTRADOR") || User.IsInRole("CONSULTOR"))
                {
                    res = PlantacionFacade.GetRNPBySedeId(UserInfo.GetSedeId().Value, UserInfo.GetEsSedePrincipal().Value, order, int.Parse(offset), int.Parse(limit), search, sort);
                }
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS")]
        public JsonResult Anular(int id)
        {
            bool rspta = PlantacionFacade.AnularById(id, UserInfo.GetUserName());
            if (rspta)
            {
                InsertHistorico(id);
            }
            return Json(rspta);

        }

        [HttpPost]
        public JsonResult InsertHistorico(int id)
        {
            return Json(HistoricoFacade.InsertByPlantacionId(id, UserInfo.GetUserName()));
        }
    }
}