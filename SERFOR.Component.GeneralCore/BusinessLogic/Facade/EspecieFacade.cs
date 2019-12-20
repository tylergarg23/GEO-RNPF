using SERFOR.Component.DTEntities.Especie;
using SERFOR.Component.GeneralCore.DataAccess;
using SERFOR.Component.Tools.DateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
{

    public static class EspecieFacade
    {
      
        public static IEnumerable<EspecieTableRowDTe> GetBySearchText(string searchText)
        {
            IQueryable<Especie> query = null;
            IEnumerable<EspecieTableRowDTe> especies;
            IQueryable<Sinonimo> query2 = null;
            IEnumerable<EspecieTableRowDTe> sinonimos;

            var dbContext = new GeneralSchema();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = dbContext.Especie.Where(p => p.Nombre.Contains(searchText));
                query2 = dbContext.Sinonimo.Where(p => p.Nombre.Contains(searchText));                
            }

            especies = query.Select(e => new EspecieTableRowDTe()
            {
                Id = e.Id,
                Nombre = e.Nombre,
                NombreConsultado = e.Nombre,
                NombreComun = e.Sinonimo.Where(p => p.Tipo == "Común").Select(s => s.Nombre).FirstOrDefault().ToString()
            });            

            sinonimos = query2.Select(e => new EspecieTableRowDTe()
            {
                Id = e.Especie_Id,
                Nombre = e.Especie.Nombre,
                NombreConsultado = e.Nombre,
                NombreComun = (e.Tipo=="Común"?e.Nombre : dbContext.Sinonimo.Where(p => p.Especie_Id == e.Especie_Id && p.Tipo.Contains("Común")).Select(s => s.Nombre).FirstOrDefault().ToString())
             });

          
            especies = especies.Union(sinonimos).OrderBy(e=>e.NombreConsultado);

            dbContext.Especie.AsNoTracking();
            dbContext.Sinonimo.AsNoTracking();

            return especies;
        }

        /*public static IEnumerable<EspecieTableRowDTe> GetBySearchText(string searchText)
        {
            IQueryable<EspecieForestal> query = null;
            IEnumerable<EspecieTableRowDTe> especies;

            var dbContext = new GeneralSchema();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = dbContext.EspecieForestal.Where(p => p.NombreComun.Contains(searchText)
                                              || p.NombreCientifico.Contains(searchText)
                                              || p.NombreFamilia.Contains(searchText));
            }

            especies = query.Select(e => new EspecieTableRowDTe()
            {
                Id = e.Id,
                EsProductoMaderable = e.EsMaderable.Value,
                NombreComun = e.NombreComun,
                NombreCientifico = e.NombreCientifico,
                NombreFamilia = e.NombreFamilia
            });

            dbContext.EspecieForestal.AsNoTracking();

            return especies;
        }*/

    }
}
