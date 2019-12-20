using System.Collections.Generic;
using System.Linq;
using SERFOR.Component.GeneralCore.DataAccess;
using SERFOR.Component.DTEntities.General;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
{
    public static class UbigeoFacade
    {

        /// <summary>
        /// Retorna los departamentos de la tabla Ubigeo
        /// </summary>
        /// <returns>Lista de departamnentos</returns>
        public static IEnumerable<UbigeoDTe> GetDepartamentos(int sedeId)
        {
            var dbContext = new GeneralSchema();

            var at_id = 0;

            var lista = dbContext.SedeAutoridadForestal.Where(w => w.Id == sedeId).Select(g => g.AT_Id).ToList();
            foreach (int elemento in lista)
            {
                at_id = elemento;
                break;
            }

            var objData = dbContext.Ubigeo.Where(u => u.Activo && (u.ARFFS_Id== at_id || at_id == 0))
                           .GroupBy(u => u.CodigoDepartamento)
                           .Select(g => g.FirstOrDefault())
                           .OrderBy(u => u.NombreDepartamento);

            dbContext.Ubigeo.AsNoTracking();

            return objData.Select(u => new UbigeoDTe
            {
                Id = u.Id,
                Codigo = u.Codigo,
                CodigoDepartamento = u.CodigoDepartamento,
                NombreDepartamento = u.NombreDepartamento,
                CodigoProvincia = u.CodigoProvincia,
                NombreProvincia = u.NombreProvincia,
                CodigoDistrito = u.CodigoDistrito,
                NombreDistrito = u.NombreDistrito
            });
        }

        public static IEnumerable<UbigeoDTe> GetProvincias(string codigoDepartamento, int sedeId)
        {
            var dbContext = new GeneralSchema();

            var at_id = 0;

            var lista = dbContext.SedeAutoridadForestal.Where(w => w.Id == sedeId).Select(g => g.AT_Id).ToList();
            foreach (int elemento in lista)
            {
                at_id = elemento;
                break;
            }

            var objData = dbContext.Ubigeo.Where(u => u.Activo && u.CodigoDepartamento.Equals(codigoDepartamento) && (u.ARFFS_Id == at_id || at_id == 0))
                           .GroupBy(u => new { u.CodigoDepartamento, u.CodigoProvincia })
                           .Select(g => g.FirstOrDefault())
                           .OrderBy(u => u.NombreProvincia);

            dbContext.Ubigeo.AsNoTracking();

            return objData.Select(u => new UbigeoDTe
            {
                Id = u.Id,
                Codigo = u.Codigo,
                CodigoDepartamento = u.CodigoDepartamento,
                NombreDepartamento = u.NombreDepartamento,
                CodigoProvincia = u.CodigoProvincia,
                NombreProvincia = u.NombreProvincia,
                CodigoDistrito = u.CodigoDistrito,
                NombreDistrito = u.NombreDistrito
            });

        }

        public static IEnumerable<UbigeoDTe> GetDistritos(string codigoDepartamento, string codigoProvincia, int sedeId)
        {
            var dbContext = new GeneralSchema();

            var at_id = 0;

            var lista = dbContext.SedeAutoridadForestal.Where(w => w.Id == sedeId).Select(g => g.AT_Id).ToList();
            foreach (int elemento in lista)
            {
                at_id = elemento;
                break;
            }

            var objData = dbContext.Ubigeo.Where(u => u.Activo && u.CodigoDepartamento.Equals(codigoDepartamento) && u.CodigoProvincia.Equals(codigoProvincia) && (u.ARFFS_Id == at_id || at_id == 0))
                            .GroupBy(u => u.Codigo)
                            .Select(g => g.FirstOrDefault())
                            .OrderBy(u => u.NombreDistrito);

            dbContext.Ubigeo.AsNoTracking();

            return objData.Select(u => new UbigeoDTe
            {
                Id = u.Id,
                Codigo = u.Codigo,
                CodigoDepartamento = u.CodigoDepartamento,
                NombreDepartamento = u.NombreDepartamento,
                CodigoProvincia = u.CodigoProvincia,
                NombreProvincia = u.NombreProvincia,
                CodigoDistrito = u.CodigoDistrito,
                NombreDistrito = u.NombreDistrito
            });
        }


    }
}
