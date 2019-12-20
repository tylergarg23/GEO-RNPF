using SERFOR.Component.GeneralCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Modulos_Core_MVC.Areas.General.Controllers
{
    [AllowAnonymous]
    public class UbigeoController : Controller
    {
        //[OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetDepartamentos(int sedeId=0)
        {
            var departamentos = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un departamento", Value = "" } };
            foreach (var u in UbigeoFacade.GetDepartamentos(sedeId))
                departamentos.Add(new SelectListItem { Text = u.NombreDepartamento, Value = u.CodigoDepartamento });

            return Json(new SelectList(departamentos, "Value", "Text"));
        }
        
        public JsonResult GetProvincias( string codigoDep, int sedeId = 0)
        {
            var provincias = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione una provincia", Value = "" } };
            foreach (var u in UbigeoFacade.GetProvincias(codigoDep,sedeId))
                provincias.Add(new SelectListItem { Text = u.NombreProvincia, Value = u.CodigoProvincia });

            return Json(new SelectList(provincias, "Value", "Text"));
        }

        public JsonResult GetDistritos(string codigoDep, string codigoProv, int sedeId = 0)
        {
            var distritos = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un distrito", Value = "0" } };
            foreach (var u in UbigeoFacade.GetDistritos(codigoDep, codigoProv,sedeId))
                distritos.Add(new SelectListItem { Text = u.NombreDistrito, Value = u.Id.ToString() });

            return Json(new SelectList(distritos, "Value", "Text"));
        }
    }
}