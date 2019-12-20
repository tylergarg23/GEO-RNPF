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
    public class DatosTipoController : Controller
    {
       
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetEstadosCivil()
        {
            var estadosCivil = new List<SelectListItem>() {
                   new SelectListItem { Text = "Seleccione un estado civil", Value = "" },
                   new SelectListItem { Text = "Soltero", Value = "S" },
                   new SelectListItem { Text = "Casado", Value = "C" },
                   new SelectListItem { Text = "Viudo", Value = "V" },
                   new SelectListItem { Text = "Divorciado", Value = "D"},
                   new SelectListItem { Text = "Deceso", Value = "X"},
                   //new SelectListItem { Text = "Feliz", Value = "F"}
            };

            return Json(new SelectList(estadosCivil, "Value", "Text"));
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetTiposAutorizacion()
        {
            var tiposAutorizacion = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un tipo", Value = "0" } };
            foreach (var t in TiposFacade.GetTiposAutorizacion())
                tiposAutorizacion.Add(new SelectListItem { Text = t.Descripcion, Value = t.Id.ToString() });

            return Json(new SelectList(tiposAutorizacion, "Value", "Text"));
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetTiposZona()
        {
            var tiposZona = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un tipo de zona", Value = "" } };
            foreach (var t in TiposFacade.GetTiposZona())
                tiposZona.Add(new SelectListItem { Text = t.Descripcion, Value = t.Id.ToString() });

            return Json(new SelectList(tiposZona, "Value", "Text"));
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetTiposComunidades()
        {
            var tiposComunidades = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un tipo de comunidad", Value = "" } };
            foreach (var t in TiposFacade.GetTiposComunidades())
                tiposComunidades.Add(new SelectListItem { Text = t.Descripcion, Value = t.Id.ToString() });

            return Json(new SelectList(tiposComunidades, "Value", "Text"));
        }        

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetTiposDocumento()
        {
            var tiposDocumento = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione un tipo de documento", Value = "0" } };
            foreach (var t in TiposFacade.GetTiposDocumento())
                tiposDocumento.Add(new SelectListItem { Text = t.SufijoAcronimo, Value = t.Id.ToString() });

            return Json(new SelectList(tiposDocumento, "Value", "Text"));
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetUnidadesMedida()
        {
            var unidadesMedida = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione", Value = "" } };
            foreach (var t in TiposFacade.GetUnidadesMedida())
                unidadesMedida.Add(new SelectListItem { Text = t.SufijoAcronimo, Value = t.Id.ToString() });

            return Json(new SelectList(unidadesMedida, "Value", "Text"));
        }
    }
}