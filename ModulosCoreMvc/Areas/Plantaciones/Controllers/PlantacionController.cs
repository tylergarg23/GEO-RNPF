
using System.Web.Mvc;
using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.BusinessLogic.Facade;
using System.Collections.Generic;
//using Newtonsoft.Json;
using System;
using Modulos_Core_MVC.Security;
using System.Web.UI;
//using System.Web;
using System.IO;
using System.Net;
//using SERFOR.Component.DTEntities.General;
using Modulos_Core_MVC.Helpers;
using SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho;
using SERFOR.Component.Tools.WebServices;
using OfficeOpenXml;
using System.Linq;
using QRCoder;

namespace Modulos_Core_MVC.Areas.Plantaciones.Controllers
{
    [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
    public class PlantacionController : Controller
    {

        [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPATFFS,REGISTRADOR")]
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

        [Authorize(Roles = "ADMINPLNT,ESPATFFS,ESPFORDIR,ESPCATAST,REGISTRADOR")]
        public ActionResult Register(int id)
        {
            PlantacionTableRowDTe plantacion = null;

            if (id > 0)
                plantacion = PlantacionFacade.GetRowById(id);
            else
                plantacion = new PlantacionTableRowDTe() { Id = 0, Anexos = AnexoFacade.GetAll() };
            List<System.Security.Claims.Claim> lista = new List<System.Security.Claims.Claim>(((System.Security.Claims.ClaimsIdentity)User.Identity).Claims);

            ViewBag.RoleName = GetRol();
            ViewBag.SedeId = lista[6].Value;

            return View(plantacion);
        }


        public ActionResult _DatesFormato1(int plantacionId)
        {
            ViewBag.PlantacionId = plantacionId;

            return PartialView(PlantacionFacade.GetDatesByPlantacionId(plantacionId));
        }

        public ActionResult _Documents(int plantacionId)
        {
            return PartialView(AnexoFacade.GetByPlantacionId(plantacionId));
        }

        public ActionResult _History(int plantacionId)
        {
            return PartialView(HistoricoFacade.GetByPlantacionId(plantacionId));
        }

        public ActionResult _ItemBloque(BloqueTableRowDTe bloque)
        {
            return PartialView(bloque);
        }

        public ActionResult _ItemEspecie(EspecieItemListDTe especie)
        {
            return PartialView(especie);
        }

        [HttpPost]
        public ActionResult _ItemRevision(int plantacionId, bool aprobado, string observacion)
        {

            var revision = new RevisionPlantacionTableRowDTe()
            {
                Plantacion_Id = plantacionId,
                Descripcion = observacion,
                EsAprobado = aprobado,
                //Poligonos = null,
                UsuarioCreacion = UserInfo.GetUserName(),
                FechaEvento = DateTime.Now,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = UserInfo.GetUserName(),
                FechaModificacion = DateTime.Now
            };
            bool aprobadoATFFS = false;
            bool aprobadoDIR = false;
            bool aprobadoCatastro = false;

            bool secuenciaCorrecta = true;
            if (User.IsInRole("ESPATFFS"))
            {
                aprobadoATFFS = aprobado;
                revision.Rol = "Especialista de la ATFFS";
                if (aprobado)
                {
                    var plantacion = PlantacionFacade.GetRowById(plantacionId);
                    if (string.IsNullOrEmpty(plantacion.NumeroCertificado))
                    {
                        var codigoGen = new CodigoDerechoFactory().CreateRegistroProvider(TipoRegistro.PLANTACIONES, Convert.ToInt16(DateTime.Now.Year), plantacion.Sede_Id.Value, plantacion.Ubigeo_Id.Value);
                        secuenciaCorrecta = PlantacionFacade.SetNumeroCertificado(plantacionId, codigoGen.Componer(true)) && codigoGen.GeneracionExitosa;

                        if (secuenciaCorrecta)
                            revision.Descripcion += " Se generó el Número de certificado " + codigoGen.CodigoGenerado;
                    }
                    else
                    {
                        secuenciaCorrecta = PlantacionFacade.SetEstado(plantacionId);
                        if (secuenciaCorrecta)
                            revision.Descripcion += " Se ingresó el Número de certificado " + plantacion.NumeroCertificado;
                    }

                }
            }
            if (User.IsInRole("ESPFORDIR"))
            {
                aprobadoDIR = aprobado;
                revision.Rol = "Especialista de DIR";
            }
            if (User.IsInRole("ESPCATAST"))
            {
                aprobadoCatastro = aprobado;
                revision.Rol = "Especialista de Catastro";
            }

            if (!secuenciaCorrecta || RevisionFacade.Insert(revision, aprobadoATFFS, aprobadoDIR, aprobadoCatastro) == 0)
            {
                throw new Exception("No se pudo registrar la revisión. Es posible que se haya intentado generar un nùmero de certificado incorrecto.");
            }
            if (User.IsInRole("ESPATFFS"))
            {
                if (aprobado)
                {
                    PlantacionFacade.GeneraLocalKey(plantacionId);
                    //GRABA INSCRIPCION EN HISTIRICO
                    InsertHistorico(plantacionId);

                    // IEnumerable<BloqueCoordenadasDTE> poligonos = PlantacionFacade.GetCoordenadasByPlantacionId(plantacionId);
                    // revision.Poligonos = poligonos;

                }
            }
            return PartialView(revision);
        }

        public ActionResult _ListBloque(int plantacionId)
        {
            ViewBag.PlantacionId = plantacionId;

            return PartialView(PlantacionFacade.GetBloquesByPlantacionId(plantacionId));
        }

        public ActionResult _Revisions(int plantacionId)
        {
            ViewBag.PlantacionId = plantacionId;

            return PartialView(RevisionFacade.GetByPlantacionId(plantacionId));
        }

        public ActionResult CertificatePrint(int plantacionId)
        {

            return PartialView(PlantacionFacade.GetRowById(plantacionId));
        }

        public ActionResult _RolPersona(int id, int idplt, string rol)
        {
            return PartialView(PersonaFacade.InsertByPlantacionId(idplt, id, rol, UserInfo.GetUserName()));
        }

        /*REDERIGIDOS*/
        [Authorize(Roles = "ADMINPLNT,ESPATFFS,REGISTRADOR")]
        public ActionResult Create()
        {
            return RedirectToAction("Register", "Plantacion", new { area = "Plantaciones", id = 0 });
        }

        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public ActionResult Edit(int id)
        {
            ViewBag.RoleName = GetRol();
            return RedirectToAction("Register", "Plantacion", new { area = "Plantaciones", id = id });
        }

        [HttpGet]
        public ActionResult Revisions()
        {
            return PartialView("_Revisions");
        }


        /*JSON RESULT*/
        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public ActionResult RegisterSectionOne(FormCollection form)
        {
            int id = 0;
            string mensaje = string.Empty;
            try
            {
                var plantacion = new PlantacionTableRowDTe();

                DateTime fechaTMP;
                
                if (!string.IsNullOrEmpty(form["FechaRecepcion"]) && DateTime.TryParse(form["FechaRecepcion"], out fechaTMP))
                    plantacion.FechaRecepcion = fechaTMP;

                if (!string.IsNullOrEmpty(form["FechaRecepcionARFFS"]) && DateTime.TryParse(form["FechaRecepcionARFFS"], out fechaTMP))
                    plantacion.FechaRecepcionARFFS = fechaTMP;

                if (!string.IsNullOrEmpty(form["numeroPredios"]))
                {
                    plantacion.NumeroPredio = form["numeroPredios"];
                }
                    
                // Información de registro
                plantacion.UsuarioCreacion = plantacion.UsuarioModificacion = UserInfo.GetUserName();
                plantacion.FechaCreacion = plantacion.FechaModificacion = DateTime.Now;

                if (Convert.ToInt32(form["Section1Id"]) == 0)
                {
                    plantacion.Sede_Id = UserInfo.GetSedeId();
                    plantacion.SedePrincipal_Id = UserInfo.GetSedePrincipalId();

                    plantacion.NumeroCertificado = plantacion.NombrePredio = string.Empty;
                    plantacion.Area = decimal.Zero;
                    plantacion.AprobadoCatastro = plantacion.AprobadoDIR = plantacion.AprobadoEspecialista = plantacion.EsPropietario = plantacion.EsInversionista = plantacion.EsPosesionario = false;

                    id = PlantacionFacade.InsertSectionOne(plantacion);
                }
                else
                {
                    id = PlantacionFacade.UpdateSectionOne(Convert.ToInt32(form["Section1Id"]), plantacion);
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }


        //TYLER
        [HttpGet]
        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public string GetStatusGeo(int plantacionId)
        {
            PlantacionTableRowDTe platantacionDTe = PlantacionFacade.GetRowById(plantacionId);
            return platantacionDTe.EstadoGeo;
        }
        //TYLER 12.12.19
        [HttpGet]
        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public ActionResult GetInfoPlantacion(int plantacionId)
        {            
            return  Json(PlantacionFacade.GetAllRowById(plantacionId), JsonRequestBehavior.AllowGet) ;
        }

        //Tyler
        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public ActionResult UpdateObjectId(FormCollection form)
        {
            int id = 0;
            string mensaje = string.Empty;
            try
            {
                var plantacion = new PlantacionTableRowDTe();
               
                plantacion.Id = Convert.ToInt32(form["plantacionId"]);
                plantacion.UsuarioModificacion = UserInfo.GetUserName();
                plantacion.ObjectId = form["objectId"];
                id = PlantacionFacade.UpdateObjectId(plantacion);

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public ActionResult UpdateStatusGeo(FormCollection form)
        {
            int id = 0;
            string mensaje = string.Empty;
            try
            {
                var plantacion = new PlantacionTableRowDTe();
                plantacion.UsuarioModificacion = UserInfo.GetUserName();
                plantacion.EstadoGeo = form["estadoGeo"];
                plantacion.Id = Convert.ToInt32(form["plantacionId"]);
                id = PlantacionFacade.UpdateStatusGeo(plantacion);

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }

        //FIN TYLER

        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPFORDIR,ESPATFFS,ESPCATAST,REGISTRADOR")]
        public ActionResult RegisterSectionTwo(FormCollection form)
        {
            int id = 0;
            string mensaje = string.Empty;
            try
            {

                var plantacion = new PlantacionTableRowDTe();
                plantacion.NombrePredio = form["NombrePredio"];

                //iNICIO TYLER
                if (!string.IsNullOrEmpty(form["numeroPredios"]))
                {
                    plantacion.NumeroPredio = form["numeroPredios"];
                }
                //FIN TYLER
                if (!string.IsNullOrEmpty(form["CantidadArea"]))
                {
                    plantacion.Area = Convert.ToDecimal(form["CantidadArea"]);
                }
                plantacion.EsPropietario = Convert.ToInt16(form["Condicion"]) == 0;
                plantacion.EsInversionista = Convert.ToInt16(form["Condicion"]) == 1;
                plantacion.EsPosesionario = Convert.ToInt16(form["Condicion"]) == 2;
                plantacion.Ubigeo_Id = Convert.ToInt16(form["DistritoPredio"]);

                if (!string.IsNullOrEmpty(form["TipoZonaPredio"]))
                    plantacion.TipoZona_Id = Convert.ToByte(form["TipoZonaPredio"]);
                if (!string.IsNullOrEmpty(form["TipoComunidad"]))
                    plantacion.TipoComunidad_Id = Convert.ToByte(form["TipoComunidad"]);
                plantacion.NombreZona = form["NombreZonaPredio"];

                plantacion.TipoAutorizacion_Id = Convert.ToByte(form["TipoAutorizacion"]);
                plantacion.DocumentoCondicion = form["DocumentoCondicion"];
                plantacion.DocumentoContrato = form["DocumentoContrato"];
                plantacion.Observaciones = form["Observaciones"];
                plantacion.SistemaCoordenada_Id = Convert.ToInt32(form["SistemaCoordenadasPredio"]);
                plantacion.CoordenadaEsteUTM = form["CoordenadaEstePredio"];
                plantacion.CoordenadaNorteUTM = form["CoordenadaNortePredio"];
                plantacion.UsuarioModificacion = UserInfo.GetUserName();
                plantacion.FechaModificacion = DateTime.Now;

                id = PlantacionFacade.UpdateSectionTwo(Convert.ToInt32(form["Section2Id"]), plantacion);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS,REGISTRADOR")]
        public ActionResult Register(FormCollection form)
        {
            int id = 0;
            string mensaje = string.Empty;
            try
            {
                var plantacion = new PlantacionTableRowDTe();

                DateTime fechaRecepcion;
                if (!string.IsNullOrEmpty(form["FechaNacimiento"]) && DateTime.TryParse(form["FechaRecepcion"], out fechaRecepcion))
                    plantacion.FechaRecepcion = fechaRecepcion;

                // Información de contacto

                // Ubigeo y dirección
                plantacion.Ubigeo_Id = Convert.ToInt16(form["Distrito"]);

                if (!string.IsNullOrEmpty(form["TipoZona"]))
                    plantacion.TipoZona_Id = Convert.ToByte(form["TipoZona"]);
                plantacion.NombreZona = form["NombreZona"];

                // Información de registro
                plantacion.UsuarioCreacion = plantacion.UsuarioModificacion = UserInfo.GetUserName();
                plantacion.FechaCreacion = plantacion.FechaModificacion = DateTime.Now;

                //id = (Convert.ToInt32(form["Id"]) == 0) ? PlantacionFacade.Insert(plantacion) : PlantacionFacade.Update(Convert.ToInt32(form["Id"]), plantacion);

                id = 1;

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }


            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _ItemBloque(FormCollection form)
        {
            var bloque = new BloqueTableRowDTe()
            {
                Nombre = form["NombreBloque"],
                Plantacion_Id = Convert.ToInt32(form["Section3Id"]),
                SistemaPlantacion_Id = Convert.ToByte(form["TipoPlantacion"]),
                //UnidadMedida_Id = Convert.ToByte(form["UnidadMedida"]),
                //Especie_Id = Convert.ToInt16(form["Especie_Id"]),
                Finalidad_Id = Convert.ToByte(form["FinalidadProducto"]),
                //EspecieNombreComun = form["NombreComun"],
                //EspecieNombreCientifico = form["NombreCientifico"],
                AreaPredio = (string.IsNullOrEmpty(form["CantidadSuperficieHa"])) ? Convert.ToDecimal(form["CantidadSuperficieMl"]) / 10000 : Convert.ToDecimal(form["CantidadSuperficieHa"]),
                AnnioEstablecimiento = Convert.ToInt16(form["AnnioBloque"]),
                MesEstablecimiento = Convert.ToByte(form["MesBloque"]),
                //AlturaPromedio = (string.IsNullOrEmpty(form["AlturaPromedio"])) ? decimal.Zero : Convert.ToDecimal(form["AlturaPromedio"]),
                CantidadSuperficieHa = (string.IsNullOrEmpty(form["CantidadSuperficieHa"])) ? Convert.ToDecimal(form["CantidadSuperficieMl"]) / 10000 : Convert.ToDecimal(form["CantidadSuperficieHa"]),
                CantidadSuperficieMl = (string.IsNullOrEmpty(form["CantidadSuperficieMl"])) ? decimal.Zero : Convert.ToDecimal(form["CantidadSuperficieMl"]),
                SistemaCoordenada_Id = Convert.ToByte(form["SistemaCoordenadas"]),
                CoordenadaEsteUTM = form["CoordenadaEsteBloque"],
                CoordenadaNorteUTM = form["CoordenadaNorteBloque"],
                //ProduccionEstimada = (string.IsNullOrEmpty(form["ProduccionEstimada"])) ? decimal.Zero : Convert.ToDecimal(form["ProduccionEstimada"]),                
                //TotalPlantado = (string.IsNullOrEmpty(form["TotalPlantado"])) ? Convert.ToByte(0) : Convert.ToInt32(form["TotalPlantado"]),
                UsuarioCreacion = UserInfo.GetUserName(),
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = UserInfo.GetUserName(),
                FechaModificacion = DateTime.Now,
                LatitudMin = Convert.ToDecimal(form["Section3LatMin"]),
                LatitudMax = Convert.ToDecimal(form["Section3LatMax"]),
                LongitudMin = Convert.ToDecimal(form["Section3LonMin"]),
                LongitudMax = Convert.ToDecimal(form["Section3LonMax"])
            };
            bloque.SistemaPlantacionDescripcion = form["Section3TipoPlantacionDesc"];
            bloque.FinalidadDescripcion = form["Section3Fines"];
            bloque.MesDescripcion = getMesDescripcion(Convert.ToByte(form["MesBloque"]));
            //if (form["TipoPlantado"] != "")
            //    bloque.TipoPlantado = Convert.ToChar(form["TipoPlantado"]);

            bloque.Id = PlantacionFacade.InsertBloque(bloque);
            if (bloque.Id == 0)
            {
                throw new Exception("No se pudo registrar el bloque o predio. Por favor corrija los datos ingresados e intente nuevamente.");
            }

            return PartialView(bloque);
        }

        [HttpPost]
        public ActionResult _ItemEspecie(FormCollection form)
        {
            var bloque1 = new BloqueTableRowDTe()
            {
                Nombre = form["BloqueAsigDesc"]
            };
            var especie = new EspecieItemListDTe()
            {
                UnidadMedida_Id = Convert.ToByte(form["UnidadMedida"]),
                UnidadMedidaSimbolo = form["UnidadMedidaSimbolo"],
                Especie_Id = Convert.ToInt16(form["Especie_Id"]),
                EspecieCodigo = Convert.ToInt16(form["Especie_Id"]),
                NombreComun = form["NombreComun"],
                EspecieNombre = form["NombreCientifico"],
                AlturaPromedio = (string.IsNullOrEmpty(form["AlturaPromedio"])) ? decimal.Zero : Convert.ToDecimal(form["AlturaPromedio"]),
                ProduccionEstimada = (string.IsNullOrEmpty(form["ProduccionEstimada"])) ? decimal.Zero : Convert.ToDecimal(form["ProduccionEstimada"]),
                TotalPlantado = (string.IsNullOrEmpty(form["TotalPlantado"])) ? Convert.ToByte(0) : Convert.ToInt32(form["TotalPlantado"]),
                TipoPlantado = form["TipoPlantado"],
                TipoPlantadoDesc = form["TipoPlantadoDesc"],
                PlantacionDetalle_Id = Convert.ToInt16(form["BloqueAsig"]),
                UsuarioCreacion = UserInfo.GetUserName(),
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = UserInfo.GetUserName(),
                FechaModificacion = DateTime.Now,
                bloque = bloque1
            };
            //if (form["TipoPlantado"] != "")
            //    bloque.TipoPlantado = Convert.ToChar(form["TipoPlantado"]);

            especie.Id = PlantacionFacade.InsertEspecie(especie);
            if (especie.Id == 0)
            {
                throw new Exception("No se pudo registrar el bloque o predio. Por favor corrija los datos ingresados e intente nuevamente.");
            }

            return PartialView(especie);
        }
        private string getMesDescripcion(byte? num)
        {
            string s = "";
            switch (num)
            {
                case 1: s = "Enero"; break;
                case 2: s = "Febrero"; break;
                case 3: s = "Marzo"; break;
                case 4: s = "Abril"; break;
                case 5: s = "Mayo"; break;
                case 6: s = "Junio"; break;
                case 7: s = "Julio"; break;
                case 8: s = "Agosto"; break;
                case 9: s = "Septiembre"; break;
                case 10: s = "Octubre"; break;
                case 11: s = "Noviembre"; break;
                case 12: s = "Diciembre"; break;
                default: s = ""; break;
            }
            return s;
        }

        [HttpPost]
        public ActionResult _UpdateBloque(FormCollection form)
        {
            var bloque = new BloqueTableRowDTe()
            {
                Id = Convert.ToInt32(form["Section3IdBloque"]),
                Nombre = form["NombreBloque"],
                Plantacion_Id = Convert.ToInt32(form["Section3Id"]),
                SistemaPlantacion_Id = Convert.ToByte(form["TipoPlantacion"]),
                //UnidadMedida_Id = Convert.ToByte(form["UnidadMedida"]),
                //Especie_Id = Convert.ToInt16(form["Especie_Id"]),
                Finalidad_Id = Convert.ToByte(form["FinalidadProducto"]),
                //EspecieNombreComun = form["NombreComun"],
                //EspecieNombreCientifico = form["NombreCientifico"],
                AreaPredio = (string.IsNullOrEmpty(form["CantidadSuperficieHa"])) ? decimal.Zero : Convert.ToDecimal(form["CantidadSuperficieHa"]),
                AnnioEstablecimiento = Convert.ToInt16(form["AnnioBloque"]),
                MesEstablecimiento = Convert.ToByte(form["MesBloque"]),
                //AlturaPromedio = (string.IsNullOrEmpty(form["AlturaPromedio"])) ? decimal.Zero : Convert.ToDecimal(form["AlturaPromedio"]),
                CantidadSuperficieHa = (string.IsNullOrEmpty(form["CantidadSuperficieHa"])) ? decimal.Zero : Convert.ToDecimal(form["CantidadSuperficieHa"]),
                CantidadSuperficieMl = (string.IsNullOrEmpty(form["CantidadSuperficieMl"])) ? decimal.Zero : Convert.ToDecimal(form["CantidadSuperficieMl"]),
                SistemaCoordenada_Id = Convert.ToByte(form["SistemaCoordenadas"]),
                CoordenadaEsteUTM = form["CoordenadaEsteBloque"],
                CoordenadaNorteUTM = form["CoordenadaNorteBloque"],
                //ProduccionEstimada = (string.IsNullOrEmpty(form["ProduccionEstimada"])) ? decimal.Zero : Convert.ToDecimal(form["ProduccionEstimada"]),                
                //TotalPlantado = (string.IsNullOrEmpty(form["TotalPlantado"])) ? Convert.ToByte(0) : Convert.ToInt32(form["TotalPlantado"]),
                UsuarioModificacion = UserInfo.GetUserName(),
                FechaModificacion = DateTime.Now
            };
            //if (form["TipoPlantado"] != "")
            //    bloque.TipoPlantado = Convert.ToChar(form["TipoPlantado"]);

            bloque.Id = PlantacionFacade.UpdateBloque(bloque);
            if (bloque.Id == 0)
            {
                throw new Exception("No se pudo actualizar el bloque o predio. Por favor corrija los datos ingresados e intente nuevamente.");
            }

            //return PartialView(bloque);
            return Json(new { success = (bloque.Id > 0), responseText = "No se pudo actualizar el bloque o predio. Por favor corrija los datos ingresados e intente nuevamente.", innerId = bloque.Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _UpdateEspecie(FormCollection form)
        {
            var bloque1 = new BloqueTableRowDTe()
            {
                Nombre = form["BloqueAsigDesc"]
            };
            var especie = new EspecieItemListDTe()
            {
                Id = Convert.ToInt32(form["Section8IdEspecie"]),
                UnidadMedida_Id = Convert.ToByte(form["UnidadMedida"]),
                UnidadMedidaSimbolo = form["UnidadMedidaSimbolo"],
                Especie_Id = Convert.ToInt16(form["Especie_Id"]),
                EspecieCodigo = Convert.ToInt16(form["Especie_Id"]),
                NombreComun = form["NombreComun"],
                EspecieNombre = form["NombreCientifico"],
                AlturaPromedio = (string.IsNullOrEmpty(form["AlturaPromedio"])) ? decimal.Zero : Convert.ToDecimal(form["AlturaPromedio"]),
                ProduccionEstimada = (string.IsNullOrEmpty(form["ProduccionEstimada"])) ? decimal.Zero : Convert.ToDecimal(form["ProduccionEstimada"]),
                TotalPlantado = (string.IsNullOrEmpty(form["TotalPlantado"])) ? Convert.ToByte(0) : Convert.ToInt32(form["TotalPlantado"]),
                TipoPlantado = form["TipoPlantado"],
                TipoPlantadoDesc = form["TipoPlantadoDesc"],
                PlantacionDetalle_Id = Convert.ToInt16(form["BloqueAsig"]),
                UsuarioModificacion = UserInfo.GetUserName(),
                FechaModificacion = DateTime.Now,
                bloque = bloque1
            };

            especie.Id = PlantacionFacade.UpdateEspecie(especie);
            if (especie.Id == 0)
            {
                throw new Exception("No se pudo actualizar la especie. Por favor corrija los datos ingresados e intente nuevamente.");
            }

            //return PartialView(bloque);
            return Json(new { success = (especie.Id > 0), responseText = "No se pudo actualizar la especie. Por favor corrija los datos ingresados e intente nuevamente.", innerId = especie.Id }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult InsertVertice(FormCollection form)
        {
            var vertice = new CoordenadaTableRowDTe()
            {
                Bloque_Id = Convert.ToInt32(form["Section6Id"]),
                NumeroSecuencia = Convert.ToByte(form["NumeroSecuencia"]),
                CoordenadaEsteUTM = form["CoordenadaEsteUTM"],
                CoordenadaNorteUTM = form["CoordenadaNorteUTM"],
                UsuarioCreacion = UserInfo.GetUserName(),
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = UserInfo.GetUserName(),
                FechaModificacion = DateTime.Now
            };

            string mensaje = string.Empty;
            decimal id = PlantacionFacade.InsertCoordenada(vertice);
            if (id == 0)
            {
                mensaje = "No se pudo registrar el vértice o coordenada. Por favor corrija los datos ingresados e intente nuevamente.";
            }

            return Json(new { success = (id > 0), responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPlantaciones()
        {
            IEnumerable<PlantacionItemListDTe> list = new List<PlantacionItemListDTe>();

            if (User.IsInRole("ESPFORDIR") || User.IsInRole("ESPCATAST") || User.IsInRole("ADMINPLNT"))
            {
                list = PlantacionFacade.GetAll();
            }
            else
            {
                if (User.IsInRole("ESPATFFS") || User.IsInRole("REGISTRADOR"))
                {
                    list = PlantacionFacade.GetBySedeId(UserInfo.GetSedeId().Value, UserInfo.GetEsSedePrincipal().Value);
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS,REGISTRADOR")]
        public JsonResult Delete(int id)
        {
            return Json(PlantacionFacade.DeleteById(id));
        }

        [HttpPost]
        public JsonResult GetEmpresaSUNAT(string ruc)
        {
            /*  ProxyConsultaRuc.ConsultaRucSoapBindingImplClient proxy = new ProxyConsultaRuc.ConsultaRucSoapBindingImplClient();
              var emp = proxy.getDatosPrincipales(ruc);
              Empresa empresa = new Empresa();
              empresa.RUC = emp.ddp_numruc;
              empresa.RazonSocial = emp.ddp_nombre;
              empresa.Departamento = emp.desc_dep;*/



            ConsultaWebService cws = new ConsultaWebService();
            Empresa emp = cws.getDatosEmpresa(ruc);
            emp.Direccion = cws.getDireccion(ruc);
            string mensaje = "";
            if (emp.RUC != ruc) mensaje = "Empresa no encontrada";
            //return Json(emp);
            return Json(new { success = (emp.RUC == ruc), responseText = mensaje, data = emp });
        }

        [HttpPost]
        public JsonResult InsertHistorico(int id)
        {
            return Json(HistoricoFacade.InsertByPlantacionId(id, UserInfo.GetUserName()));
        }



        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS,REGISTRADOR")]
        public JsonResult DeletePersona(int id)
        {
            return Json(PersonaFacade.DeleteById(id));
        }

        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS,REGISTRADOR")]
        public JsonResult DeleteBloque(int id)
        {
            return Json(PlantacionFacade.DeleteBloqueById(id));

        }

        [HttpPost]
        [Authorize(Roles = "ADMINPLNT,ESPATFFS,REGISTRADOR")]
        public JsonResult DeleteEspecie(int id)
        {
            return Json(PlantacionFacade.DeleteEspecieById(id));

        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetFinalidadProducto()
        {
            var finalidad = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione el fin", Value = "0" } };
            foreach (var t in TiposFacade.GetFinalidadProducto())
                finalidad.Add(new SelectListItem { Text = t.Descripcion, Value = t.Id.ToString() });

            return Json(new SelectList(finalidad, "Value", "Text"));
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetSistemaCoordenadas()
        {
            var sistemasCoor = new List<SelectListGeoItem>() { new SelectListGeoItem { Text = "Seleccione el sistema", Value = "" } };
            foreach (var t in TiposFacade.GetSistemaCoordenadas())
                sistemasCoor.Add(new SelectListGeoItem { Text = t.Descripcion, Value = t.Id.ToString(), dataLatitudMin = t.LatitudMin, dataLatitudMax = t.LatitudMax, dataLongitudMin = t.LongitudMin, dataLongitudMax = t.LongitudMax });

            return Json(sistemasCoor);
        }


        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetSistemasPlantacion()
        {
            var sistemasPlant = new List<SistemaPlantacionItemDTe>() { new SistemaPlantacionItemDTe { Descripcion = "Seleccione el Sistema", Id = 0 } };
            foreach (var t in TiposFacade.GetSistemasPlantacion())
                sistemasPlant.Add(new SistemaPlantacionItemDTe { Descripcion = t.Descripcion, Id = t.Id, Tipo = t.SufijoAcronimo });

            return Json(sistemasPlant);
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server, VaryByParam = "none")]
        public JsonResult GetTipoPlantado()
        {
            var tiposPlant = new List<SelectListItem>() { new SelectListItem { Text = "Seleccione el tipo", Value = "" } };
            foreach (var t in TiposFacade.GetTiposPlantado())
                tiposPlant.Add(new SelectListItem { Text = t.Descripcion, Value = t.SufijoAcronimo });

            return Json(new SelectList(tiposPlant, "Value", "Text"));
        }




        public JsonResult GetCoordenadas(int id)
        {
            return Json(PlantacionFacade.GetCoordenadasByBloqueId(id));
        }

        public JsonResult GetBloque(int id)
        {
            try
            {
                return Json(PlantacionFacade.GetBloqueById(id));
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

        }

        public JsonResult GetEspecie(int id)
        {
            return Json(PlantacionFacade.GetEspecieById(id));
        }

        public JsonResult GetBloques(int id)
        {
            var bloques = new List<BloqueTableRowDTe>() { new BloqueTableRowDTe { Nombre = "Seleccione el fin", Id = 0, CantidadSuperficieHa = 0, CantidadSuperficieMl = 0 } };
            foreach (var t in PlantacionFacade.GetBloquesByPlantacionId(id))
                bloques.Add(new BloqueTableRowDTe { Nombre = t.Nombre, Id = t.Id, CantidadSuperficieHa = t.CantidadSuperficieHa, CantidadSuperficieMl = t.CantidadSuperficieMl });

            return Json(bloques);
        }



        [HttpPost]
        public JsonResult DeleteCoordenada(int id)
        {
            return Json(PlantacionFacade.DeleteCoordenadaById(id));

        }

        [HttpPost]
        public JsonResult DeleteArchivoCoordenada(int id, string ruta)
        {
            bool exito = false;

            if (AzureStorageHelper.DeleteFile(ruta, AzureStorageHelper.ContainerFlolder.Coordenadas))
            {
                exito = PlantacionFacade.UnsetBloqueUriFile(id);

            }
            return Json(exito);
        }

        [HttpPost]
        public JsonResult UploadCoordinateFile(int bloqueId, double latmin, double latmax, double lonmin, double lonmax)
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
                        //string fileName = fileContent.FileName;
                        string fileContentType = fileContent.ContentType;
                        //byte[] fileBytes = new byte[fileContent.ContentLength];
                        //var data = fileContent.InputStream.Read(fileBytes, 0, Convert.ToInt32(fileContent.ContentLength));
                        if (fileContentType == "application/vnd.ms-excel" || fileContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            var coordenadasList = new List<CoordenadaTableRowDTe>();
                            using (var package = new ExcelPackage(fileContent.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                var noOfRow = workSheet.Dimension.End.Row;
                                exito = true;
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    try
                                    {
                                        var coordenada = new CoordenadaTableRowDTe();
                                        if (int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString()) > 0)
                                            coordenada.NumeroSecuencia = int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                                        else
                                        {
                                            mensaje = "Numero de Secuencia Incorrecta!";
                                            exito = false;
                                            break;
                                        }
                                        coordenada.Bloque_Id = bloqueId;
                                        if (double.Parse(workSheet.Cells[rowIterator, 2].Value.ToString()) > lonmin && double.Parse(workSheet.Cells[rowIterator, 2].Value.ToString()) < lonmax)
                                            coordenada.CoordenadaEsteUTM = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        else
                                        {
                                            mensaje = "Coordenada Este fuera del rango!";
                                            exito = false;
                                            break;
                                        }
                                        if (double.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) > latmin && double.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) < latmax)
                                            coordenada.CoordenadaNorteUTM = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        else
                                        {
                                            mensaje = "Coordenada Norte fuera del rango!";
                                            exito = false;
                                            break;
                                        }
                                        coordenada.UsuarioCreacion = "";
                                        coordenadasList.Add(coordenada);

                                    }
                                    catch (Exception ex)
                                    {
                                        mensaje = "Valores incorrectos!";
                                        exito = false;
                                    }

                                }
                                if (exito)
                                {
                                    foreach (var coor in coordenadasList)
                                    {
                                        PlantacionFacade.InsertCoordenada(coor);
                                    }
                                }
                            }
                            /*var uri = AzureStorageHelper.UploadFile(fileContent.InputStream,
                                string.Format("blq-{0}-{1}{2}", bloqueId, Guid.NewGuid().ToString(), Path.GetExtension(fileContent.FileName)),
                                AzureStorageHelper.ContainerFlolder.Coordenadas);*/

                            //exito = true; // (!string.IsNullOrEmpty(uri) && PlantacionFacade.SetBloqueUriFile(bloqueId, uri));
                        }
                        else
                        {
                            mensaje = "Tipo de Archivo no valido!";
                            exito = false;
                        }

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

            return Json(new { success = exito, responseText = mensaje, innerId = bloqueId }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UploadAnexoFile(int plantacionId, short anexoId)
        {
            string mensaje = string.Empty;
            bool exito = false;
            decimal id = 0;
            try
            {
                if (Request.Files.Count > 0)
                {

                    var fileContent = Request.Files[0];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var docAnexo = new DocumentoAnexoTableRowDTe()
                        {
                            Anexo_Id = anexoId,
                            Plantacion_Id = plantacionId,
                            Secuencia = Convert.ToByte(anexoId),
                            NombreArchivo = string.Format("anx-{0}-{1}-{2}{3}", plantacionId, anexoId, Guid.NewGuid().ToString(), Path.GetExtension(fileContent.FileName)),
                            UsuarioCreacion = UserInfo.GetUserName(),
                            UsuarioModificacion = UserInfo.GetUserName(),
                            FechaCreacion = DateTime.Now,
                            FechaModificacion = DateTime.Now,
                            Tamanio = fileContent.ContentLength
                        };

                        var Uri = AzureStorageHelper.UploadFile(fileContent.InputStream,
                                        docAnexo.NombreArchivo,
                                        AzureStorageHelper.ContainerFlolder.Anexos);
                        docAnexo.Ruta = Uri;
                        id = AnexoFacade.Insert(docAnexo);

                        exito = (!string.IsNullOrEmpty(Uri) && (id > 0));

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

            return Json(new { success = exito, responseText = mensaje, innerId = id }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteDocumentoAnexo(decimal id, string ruta)
        {
            bool exito = false;

            if (AzureStorageHelper.DeleteFile(ruta, AzureStorageHelper.ContainerFlolder.Anexos))
            {
                exito = AnexoFacade.DeleteById(id);

            }
            return Json(exito);
        }
    }
    public class SelectListGeoItem : SelectListItem
    {

        public SelectListGeoItem() { }

        public decimal dataLatitudMin { get; set; }
        public decimal dataLatitudMax { get; set; }
        public decimal dataLongitudMin { get; set; }
        public decimal dataLongitudMax { get; set; }
    }
}
