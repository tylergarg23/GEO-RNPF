using Newtonsoft.Json;
using SERFOR.Component.GeneralCore.BusinessLogic.Facade;
using SERFOR.Component.SeguridadCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Modulos_Core_MVC.Areas.Seguridad.Controllers
{
    [AllowAnonymous]
    public class TipoDatoController : Controller
    {


        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetTrabajadores()
        {
            var personas = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione una persona", Value = "0" } };
            foreach (var t in UsuarioFacade.GetTrabajadores())
                personas.Add(new SelectListItem { Text = t.NombreCompleto, Value = t.Id.ToString() });

            return Json(new SelectList(personas, "Value", "Text"));
        }
        public JsonResult GetTrabajadores_nU()
        {
            var personas = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione una persona", Value = "0" } };
            foreach (var t in UsuarioFacade.GetTrabajadores_nU())
                personas.Add(new SelectListItem { Text = t.NombreCompleto, Value = t.Id.ToString() });

            return Json(new SelectList(personas, "Value", "Text"));
        }
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetSedes()
        {
            var sedes = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione una sede", Value = "0" } };
            foreach (var t in SedesFacade.GetSedes())
                sedes.Add(new SelectListItem { Text = t.Descripcion, Value = t.Id.ToString() });

            return Json(new SelectList(sedes, "Value", "Text"));
        }
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetRoles()
        {
            var roles = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione algun rol", Value = "0" } };
            foreach (var t in UsuarioFacade.GetRoles())
                roles.Add(new SelectListItem { Text = t.Codigo, Value = t.Id.ToString() });

            return Json(new SelectList(roles, "Value", "Text"));
        }
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetCargos()
        {
            var cargos = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un cargo", Value = "0" } };
            foreach (var t in UsuarioFacade.GetCargos())
                cargos.Add(new SelectListItem { Text = t.Nombre, Value = t.Id.ToString() });

            return Json(new SelectList(cargos, "Value", "Text"));
        }
    }
}





