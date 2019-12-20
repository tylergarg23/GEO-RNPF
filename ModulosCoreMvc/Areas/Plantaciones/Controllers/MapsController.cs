using SERFOR.Component.PlantacionCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Modulos_Core_MVC.Areas.Plantaciones.Controllers
{
    [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR,CONSULTOR")]
    public class MapsController : Controller
    {
        [HttpGet]
        public ActionResult MapCoordenadasBloque(int bloqueId)
        {
            return PartialView(PlantacionFacade.GetBloqueById(bloqueId));
        }


        [HttpGet]
        public JsonResult GetCoordenadasBloque(int bloqueId)
        {
            var json = Json(PlantacionFacade.GetCoordenadasByBloqueId(bloqueId), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;

        }

        [HttpGet]
        public ActionResult MapCoordenadasPlantacion(int plantacionId)
        {
            return PartialView(PlantacionFacade.GetRowById(plantacionId));
        }


        [HttpGet]
        public JsonResult GetCoordenadasPlantacion(int plantacionId)
        {
            var json = Json(PlantacionFacade.GetCoordenadasByPlantacionId(plantacionId), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }

        [HttpGet]
        public JsonResult GetCapaBySede(int sedeId)
        {
            var json = Json(PlantacionFacade.GetCapaArcGISPlantacionesBySede(sedeId), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }

        [HttpGet]
        public JsonResult GetCoordenadaPredio(int plantacionId)
        {
            var json = Json(PlantacionFacade.GetCoordenadaPredio(plantacionId), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }

        [HttpGet]
        public ActionResult ArcMapCoordenadasPlantacion(int plantacionId)
        {
            return PartialView(PlantacionFacade.GetRowById(plantacionId));
        }

        [HttpGet]
        public ActionResult ArcMapCoordenadasBloque(int bloqueId)
        {
            return PartialView(PlantacionFacade.GetBloqueById(bloqueId));
        }

    }
}