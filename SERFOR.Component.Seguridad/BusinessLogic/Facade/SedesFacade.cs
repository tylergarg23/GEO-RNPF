using SERFOR.Component.DTEntities.General;
using SERFOR.Component.SeguridadCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
{
    public static class SedesFacade
    {
        public static IEnumerable<EntidadTipoDTe> GetSedes()
        {
            var dbContext = new SecuritySchema();

            var objData = dbContext.SedeAutoridadForestal.Where(t => t.Activo);

            dbContext.SedeAutoridadForestal.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = "",
                Descripcion = t.Nombre
            });
        }


    }
}
