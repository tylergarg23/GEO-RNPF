using Modulos_Core_MVC.Helpers;
using Newtonsoft.Json;
using SERFOR.Component.GeneralCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Modulos_Core_MVC.Areas.General.Controllers
{
    [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPFORDIR,ESPATFFS")]
    public class AutoridadForestalController : Controller
    {
        // GET: General/AutoridadForestal
        public ActionResult Index()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }

        public ContentResult GetAllAutoridades()
        {
            return Content(JsonConvert.SerializeObject(AutoridadForestalFacade.GetAll()));
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

        [HttpPost]
        public JsonResult UploadLogoFile(int autoridadId)
        {
            string mensaje = string.Empty;
            bool exito = false;
            try
            {
                if (Request.Files.Count > 0)
                {

                    var fileContent = Request.Files[0];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        var uri = AzureStorageHelper.UploadFile(fileContent.InputStream,
                            string.Format("logo-{0}-{1}{2}", autoridadId, Guid.NewGuid().ToString(), Path.GetExtension(fileContent.FileName)),
                            AzureStorageHelper.ContainerFlolder.Logos);

                        exito = (!string.IsNullOrEmpty(uri) && AutoridadForestalFacade.SetLogoUriFile(autoridadId, uri));

                    }
                    else
                    {
                        mensaje = "Archivo no encontrado!";
                        exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                mensaje = ex.Message;
                exito = false;
            }

            return Json(new { success = exito, responseText = mensaje, innerId = autoridadId }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UpdateDatosContacto(int autoridadId, string nombre, string direccion, string email, string telefono)
        {
            return Json(AutoridadForestalFacade.UpdateDatosContacto(autoridadId, nombre, direccion, telefono, email));

        }

    }
}