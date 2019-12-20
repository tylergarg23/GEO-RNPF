using OfficeOpenXml;
using SERFOR.Component.DTEntities.ListasExcel;
using SERFOR.Component.ListasExcelCore.BusinessLogic.Facade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text;

namespace Modulos_Core_MVC.Areas.ConsultasDIR.Controllers
{
    public class LicenciasController : Controller
    {
        [HttpPost]
        public JsonResult UploadFormato23()
        {
            bool exito;
            var mensaje =string.Empty;

            var attachedFile = System.Web.HttpContext.Current.Request.Files["ExcelFile"];
            if (attachedFile == null || attachedFile.ContentLength <= 0)
            {
                exito = false;
                mensaje = "El tamaño del archivo seleccionado es nulo o no contiene información válida.";
            }
            else
            {
                using (var package = new ExcelPackage(attachedFile.InputStream))
                {
                    var lista = new List<LicenciaCazaDTe>();
                    try
                    {
                        var workSheet = package.Workbook.Worksheets.FirstOrDefault();

                        // Fecha de actualizacion
                        // var updated = Convert.ToDateTime(workSheet.Cells[3, 1].Value);

                        for (int r = 4; r <= workSheet.Dimension.End.Row; r++)
                        {
                            lista.Add(new LicenciaCazaDTe
                            {
                                Id = Convert.ToInt16(workSheet.Cells[r, 1].Value),
                                AutoridadForestal = workSheet.Cells[r, 2].Value.ToString(),
                                Numero = workSheet.Cells[r, 3].Value.ToString(),
                                FechaEmision = DateTime.FromOADate(double.Parse(workSheet.Cells[r, 4].Value.ToString())),
                                FechaCaducidad = DateTime.FromOADate(double.Parse(workSheet.Cells[r, 5].Value.ToString())),
                                TipoDocumento = workSheet.Cells[r, 6].Value.ToString(),
                                NumeroDocumento = workSheet.Cells[r, 7].Value.ToString(),
                                ApellidoPaterno = workSheet.Cells[r, 8].Value.ToString(),
                                ApellidoMaterno = workSheet.Cells[r, 9].Value.ToString(),
                                Nombres = workSheet.Cells[r, 10].Value.ToString(),

                                //CAUSALES DE EXTINCION
                                //N° RESOLUCION
                                //FECHA DE LA RESOLUCION

                            });
                        }

                        exito = true;
                    }
                    catch (Exception e)
                    {
                        exito = false;
                        mensaje = "Ocurrió un error: " + e.Message;
                    }
                }
            }

            return Json(new { success = exito, responseText = mensaje }, JsonRequestBehavior.AllowGet);
          
        }

        public ActionResult IndexCazaDeportiva()
        {
            return View(LicenciasCaza.GetLicenciasCazaDeportiva());
        }
    }
}