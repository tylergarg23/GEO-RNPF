using Modulos_Core_MVC.Security;
using Newtonsoft.Json;
using SERFOR.Component.DTEntities.General;
using SERFOR.Component.PlantacionCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Modulos_Core_MVC.Areas.Plantaciones.Controllers
{
    [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPATFFS")]    
    public class InteresadoController : Controller
    {
       /* [HttpPost]
        public JsonResult Search(string texto, int idPlt)
        {
            string cadena = "";
            var plantacion = SERFOR.Component.PlantacionCore.BusinessLogic.Facade.PlantacionFacade.GetRowById(idPlt);
            if (plantacion != null)
            {
                foreach (var persona in plantacion.Personas)
                {
                    if (cadena != "")
                    {
                        cadena = cadena + "," + persona.Id.ToString();
                    }
                    else
                    {
                        cadena = cadena + persona.Id.ToString();
                    }
                }
            }
            string mensaje = string.Empty;
            IEnumerable<PersonaItemListDTe> list = null;

            try
            {
                list = PersonaFacade.GetBySearchText(texto, cadena);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(list);
        }
        [HttpPost]
        public JsonResult SearchTipoNumero(string tipo, string numero)
        {
            string mensaje = string.Empty;
            IEnumerable<PersonaItemListDTe> list = null;

            try
            {
                list = PersonaFacade.GetByPersona_xTipoNumero(tipo, numero);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(list);
        }
        */

        public ActionResult Index()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }

        public ActionResult _DetailsPopUp(int id)
        {
            return PartialView(PersonaFacade.GetDetailById(id));
        }
        [Authorize(Roles = "ADMINPLNT,ESPATFFS")]
        public ActionResult Create()
        {
            return RedirectToAction("Register", "Interesado", new { area = "Plantaciones", id = 0 });
        }

        [Authorize(Roles = "ADMINPLNT,ESPATFFS")]
        public ActionResult Edit(int id)
        {
            ViewBag.RoleName = GetRol();
            return RedirectToAction("Register", "Interesado", new { area = "Plantaciones", id = id });
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

        [Authorize(Roles = "ADMINPLNT,ESPATFFS")]
        public ActionResult Register(int id)
        {
            PersonaTableRowDTe persona = null;

            if (id > 0)
                persona = PersonaFacade.GetRowInteresadoById(id);
            else
                persona = new PersonaTableRowDTe() { Id = 0, EsJuridica = false, EsAdministrado = true };
            ViewBag.RoleName = GetRol();
            return View(persona);
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            int id = 0;
            string mensaje = string.Empty;
            try
            {
                var persona = new PersonaTableRowDTe();

                bool esjuridica = Convert.ToBoolean(form["EsJuridica"]);
                persona.EsJuridica = esjuridica;
                persona.Sexo = "";
                persona.EstadoCivil = "";
                persona.ApellidoPaterno = "";
                persona.ApellidoMaterno = "";

                // Infomración personal
                if (esjuridica)
                {
                    persona.TipoDocumento_Id = 2;
                    persona.Documento = form["NumeroRUC"];
                    persona.NombreCompleto = form["RazonSocial"];
                }
                else
                {
                    persona.TipoDocumento_Id = Convert.ToByte(form["TipoDocumento"]);
                    persona.Documento = form["NumeroDocumento"];
                    if (persona.TipoDocumento_Id == 2)
                    {
                        persona.NombreCompleto = form["RazonSocial"];
                    }
                    else
                    {
                        persona.NombreCompleto = form["Nombres"];
                        persona.ApellidoPaterno = form["ApellidoPaterno"];
                        persona.ApellidoMaterno = form["ApellidoMaterno"];
                    }
                    persona.Sexo = form["Sexo"];

                    DateTime fechaNac;
                    if (!string.IsNullOrEmpty(form["FechaNacimiento"]) && DateTime.TryParse(form["FechaNacimiento"], out fechaNac))
                        persona.FechaNacimiento = fechaNac;

                    persona.EstadoCivil = form["EstadoCivil"];
                }

                // Información de contacto
                persona.Telefono = form["Telefono"];
                persona.Celular = form["Celular"];
                persona.Email = form["Email"];
                // Ubigeo y dirección
                persona.Ubigeo_Id = Convert.ToInt16(form["Distrito"]);
                persona.Direccion = form["Direccion"];

                // Información de registro
                persona.UsuarioCreacion = persona.UsuarioModificacion = UserInfo.GetUserName();
                persona.FechaCreacion = persona.FechaModificacion = DateTime.Now;
                if (Convert.ToInt32(form["Id"]) == 0)
                {
                    persona.EsTrabajador = false;
                    persona.EsAdministrado = true;
                }

                id = (Convert.ToInt32(form["Id"]) == 0) ? PersonaFacade.Insert(persona) : PersonaFacade.Update(Convert.ToInt32(form["Id"]), persona);


            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }


            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }

        /*public ContentResult GetPersonas(int pageSize, int pageNumber, string sortName, string sortOrder, string searchText)
        {
            var list = PersonaFacade.GetByParams(pageSize * (pageNumber - 1), pageSize, string.Format("{0}_{1}", sortName, sortOrder.ToUpper()), searchText);

            dynamic foo = new ExpandoObject();
            foo.total = PersonaFacade.GetTotal();
            foo.rows = list;

            return Content(JsonConvert.SerializeObject(foo));
        }*/

        public ContentResult GetAllInteresados()
        {
            return Content(JsonConvert.SerializeObject(PersonaFacade.GetInteresados()));
        }

       


        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS")]
        public JsonResult Delete(int id)
        {
            return Json(PersonaFacade.DeleteInteresadoById(id));
        }

        

    }
}
