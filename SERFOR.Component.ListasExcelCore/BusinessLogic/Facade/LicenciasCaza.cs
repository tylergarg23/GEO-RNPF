using SERFOR.Component.DTEntities.ListasExcel;
using SERFOR.Component.ListasExcelCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.ListasExcelCore.BusinessLogic.Facade
{
    public static class LicenciasCaza
    {
        public static IEnumerable<LicenciaCazaDTe> GetLicenciasCazaDeportiva()
        {
            var dbContext = new ExcelsSchema();

            var licencias = dbContext.Excel_LicenciasCazaDeportiva.Select(lc =>
                         new LicenciaCazaDTe()
                         {
                             Id = lc.Id,
                             Nombres = lc.Nombres,
                             Numero = lc.Numero,
                             ApellidoMaterno = lc.ApellidoMaterno,
                             ApellidoPaterno = lc.ApellidoPaterno,
                             AutoridadForestal = lc.AutoridadForestal,
                             CausalesExtinsion = lc.CausalesExtinsion,
                             FechaCaducidad = lc.FechaCaducidad,
                             FechaEmision = lc.FechaEmision,
                             FechaResolucion = (lc.FechaResolucion.HasValue) ? lc.FechaResolucion.Value : DateTime.MinValue,
                             TipoDocumento = lc.TipoDocumento,
                             NumeroDocumento = lc.NumeroDocumento,
                             NumeroResolucion = lc.NumeroResolucion
                         });
            dbContext.Excel_LicenciasCazaDeportiva.AsNoTracking();


            return licencias;
        }
    }
}
