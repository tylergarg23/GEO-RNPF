using Modulos_Core_MVC.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SERFOR.Component.SeguridadCore.DataAccess;
using SERFOR.Component.Tools.Cryptography;
using SERFOR.Component.DTEntities.Seguridad;
using SERFOR.Component.SeguridadCore.BusinessLogic.Facade;
using Newtonsoft.Json;

namespace Modulos_Core_MVC.Areas.Seguridad.Controllers
{
    [Authorize(Roles = "ADMINSIS")]
    public class UsuariosController : Controller
    {
        private SecuritySchema db = new SecuritySchema();

        // GET: Seguridad/Usuarios
        public ActionResult Index()
        {
            ViewBag.RoleName = GetRol();
            var usuario = db.Usuario.Include(u => u.Persona);
            return View(usuario.ToList());
        }

        public ContentResult GetAllUsuarios()
        {
            string json = JsonConvert.SerializeObject(UsuarioFacade.GetAll());
            return Content(json);
        }

        // GET: Seguridad/Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Seguridad/Usuarios/Create
        public ActionResult Create()
        {
            UsuarioTableRowDTe usuario = null;

            usuario = new UsuarioTableRowDTe() { Id = 0 };
            ViewBag.RoleName = GetRol();
            return View(usuario);
        }
    // POST: Seguridad/Usuarios/Create
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
    // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
    /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Usuario_Id,Nombre,Clave,Activo,FechaInicio,FechaFin,Sede_Id,SedePrincipal_Id,EsSedePrincipal,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Clave = HashCrypter.Sha1Encrypter(usuario.Clave);
                usuario.FechaCreacion = DateTime.Today;
                usuario.FechaModificacion = DateTime.Today;
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Usuario_Id = new SelectList(db.Persona, "Id", "Nombres", usuario.Usuario_Id);
            return View(usuario);
        }*/
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            bool rspta = false;
            string mensaje = string.Empty;
            try
            {
                var usuario = new UsuarioTableRowDTe();
                usuario.NombreUsuario = form["nombreUsuario"];
                usuario.Password = HashCrypter.Sha1Encrypter(form["clave"]);
                usuario.Id = Convert.ToInt32(form["PersonaList"]);
                DateTime fechaIni;
                if (!string.IsNullOrEmpty(form["FechaInicio"]) && DateTime.TryParse(form["FechaInicio"], out fechaIni))
                    usuario.FechaInicio = fechaIni;
                DateTime fechaFin;
                if (!string.IsNullOrEmpty(form["FechaFinal"]) && DateTime.TryParse(form["FechaFinal"], out fechaFin))
                    usuario.FechaFin = fechaFin;

                usuario.Sede_Id = Convert.ToByte(form["Sede"]);
                usuario.SedePrincipal_Id = Convert.ToByte(form["SedePrincipal"]);

                bool essedeprincipal = Convert.ToBoolean(form["EsSedePrincipal"]);
                usuario.EsSedePrincipal = essedeprincipal;

                usuario.UsuarioModifica = UserInfo.GetUserName();

                if (form["Roles"] != null) { 
                string roles = form["Roles"];

                string[] lst_roles = roles.Split(',');
                foreach (string rol in lst_roles)
                {
                    RolDTe r = new RolDTe();
                    r.Id = byte.Parse(rol);
                    usuario.Roles.Add(r);
                }
            }
                usuario.Directorio.Cargo_Id = Convert.ToInt16(form["Cargo"]);
                usuario.Directorio.Sede_Id = Convert.ToByte(form["Sede"]);
                usuario.Directorio.Persona_Id = Convert.ToInt32(form["PersonaList"]);
                usuario.Directorio.Email = form["Email"];
                bool esresponsable = Convert.ToBoolean(form["EsResponsable"]);
                usuario.Directorio.EsResponsable = esresponsable;
                rspta = UsuarioFacade.Insert(usuario);

                //id = (Convert.ToInt32(form["Id"]) == 0) ? PersonaFacade.Insert(persona) : PersonaFacade.Update(Convert.ToInt32(form["Id"]), persona);


            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { success = rspta, responseText = mensaje, innerId = form["PersonaList"] }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            int id2 = id ?? 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioTableRowDTe usuario = UsuarioFacade.GetRowById(id2);

            if (usuario == null)
            {
                return HttpNotFound();
            }
            usuario.Password = "";
            ViewBag.Usuario_Id = new SelectList(db.Persona, "Id", "Nombres", usuario.Persona.Id);
            ViewBag.Sede_Id = new SelectList(db.SedeAutoridadForestal, "Id", "Nombre", usuario.Sede_Id);
            ViewBag.SedePrincipal_Id = new SelectList(db.SedeAutoridadForestal, "Id", "Nombre", usuario.SedePrincipal_Id);
            ViewBag.RoleName = GetRol();
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            bool rspta = false;
            string mensaje = string.Empty;
            try
            {
                var usuario = new UsuarioTableRowDTe();
                usuario.NombreUsuario = form["nombreUsuario"];
                if(form["clave"].Length>0) usuario.Password = HashCrypter.Sha1Encrypter(form["clave"]);
                usuario.Id = Convert.ToInt32(form["Id"]);
                DateTime fechaIni;
                if (!string.IsNullOrEmpty(form["FechaInicio"]) && DateTime.TryParse(form["FechaInicio"], out fechaIni))
                    usuario.FechaInicio = fechaIni;
                DateTime fechaFin;
                if (!string.IsNullOrEmpty(form["FechaFinal"]) && DateTime.TryParse(form["FechaFinal"], out fechaFin))
                    usuario.FechaFin = fechaFin;

                usuario.Sede_Id = Convert.ToByte(form["Sede"]);
                usuario.SedePrincipal_Id = Convert.ToByte(form["SedePrincipal"]);

                bool essedeprincipal = Convert.ToBoolean(form["EsSedePrincipal"]);
                usuario.EsSedePrincipal = essedeprincipal;

                usuario.UsuarioModifica = UserInfo.GetUserName();

                if (form["Roles"] != null)
                {
                    string roles = form["Roles"];

                    string[] lst_roles = roles.Split(',');
                    foreach (string rol in lst_roles)
                    {
                        RolDTe r = new RolDTe();
                        r.Id = byte.Parse(rol);
                        usuario.Roles.Add(r);
                    }
                }
                usuario.Directorio.Cargo_Id = Convert.ToInt16(form["Cargo"]);
                usuario.Directorio.Sede_Id = Convert.ToByte(form["Sede"]);
                usuario.Directorio.Persona_Id = Convert.ToInt32(form["Id"]);
                usuario.Directorio.Email = form["Email"];
                bool esresponsable = Convert.ToBoolean(form["EsResponsable"]);
                usuario.Directorio.EsResponsable = esresponsable;
                //rspta = UsuarioFacade.Insert(usuario);
                rspta = UsuarioFacade.Update(Convert.ToInt32(form["Directorio_Id"]), form["Roles"], usuario);

                //id = (Convert.ToInt32(form["Id"]) == 0) ? PersonaFacade.Insert(persona) : PersonaFacade.Update(Convert.ToInt32(form["Id"]), persona);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { success = rspta, responseText = mensaje, innerId = form["Id"] }, JsonRequestBehavior.AllowGet);
        }

        // GET: Seguridad/Usuarios/Delete/5
      /*  public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }*/

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ViewBag.RoleName = GetRol();
            return Json(UsuarioFacade.DeleteById(id));
        }

        // POST: Seguridad/Usuarios/Delete/5
       /* [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}
