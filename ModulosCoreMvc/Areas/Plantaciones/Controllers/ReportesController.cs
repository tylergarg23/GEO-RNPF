using SERFOR.Component.PlantacionCore.BusinessLogic.Facade;
using System.Web.Mvc;

namespace Modulos_Core_MVC.Areas.Plantaciones.Controllers
{
    [Authorize(Roles = "ADMINSIS,ADMINPLNT,ESPFORDIR,ESPCATAST,ESPATFFS,CONSULTOR")]
    public class ReportesController : Controller
    {
        // GET: Plantaciones/Reportes
        public ActionResult IndexMINAGRI()
        {            
            return View();
        }

        // GET: Plantaciones/Reportes
        public ActionResult IndexSummary()
        {
            return View();
        }


        public ActionResult ReportOficial()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }

        public JsonResult GetPlantaciones(string codigoDepartamento)
        {
            return Json(ReportesFacade.GetApprovedByDepartment(true, codigoDepartamento), JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult ReportDepartment()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }

        public JsonResult GetDataDepartamentos()
        {
            return Json(ReportesFacade.GetDataDepartments(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportSummary()
        {
            ViewBag.RoleName = GetRol();
            return View();

        }
        public ActionResult ReportResumen()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }
        public ActionResult ReportAreaPlantacion()
        {
            ViewBag.RoleName = GetRol();
            return View();
        }
        public JsonResult GetAllPlantaciones(bool soloAprobados, char condicion, string codigoDepartamento)
        {
            return Json(ReportesFacade.GetSummaryByFilter(soloAprobados, condicion, codigoDepartamento), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetResumenPlantaciones()
        {
            JsonResult result = Json(ReportesFacade.GetResumenPlantacion(), JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = 50000000;
            return result;            

        }
        public JsonResult GetResumenPlantacionEspacio()
        {
            JsonResult result = Json(ReportesFacade.GetResumenPlantacionEspacio(), JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = 50000000;
            return result;

        }
        public JsonResult GetResumenPlantacionProduccion()
        {
            JsonResult result = Json(ReportesFacade.GetResumenPlantacionProduccion(), JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = 50000000;
            return result;

        }
        private string GetRol()
        {
            string result = "";
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
