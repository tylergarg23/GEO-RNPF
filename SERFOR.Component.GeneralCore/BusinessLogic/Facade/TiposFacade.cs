using SERFOR.Component.DTEntities.General;
using SERFOR.Component.GeneralCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
{
    public static class TiposFacade
    {
        public static IEnumerable<EntidadTipoDTe> GetTiposDocumento()
        {
            var dbContext = new GeneralSchema();

            var objData = dbContext.TipoDocumento.Where(t => t.Activo);

            dbContext.TipoDocumento.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = t.Acronimo,
                Descripcion = t.Descripcion
            });
        }

        public static IEnumerable<EntidadTipoDTe> GetTiposZona()
        {
            var dbContext = new GeneralSchema();

            var objData = dbContext.TipoZona.Where(t => t.Activo);

            dbContext.TipoZona.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = string.Empty,
                Descripcion = t.Descripcion
            });
        }
        public static IEnumerable<EntidadTipoDTe> GetTiposComunidades()
        {
            var dbContext = new GeneralSchema();

            var objData = dbContext.TipoComunidad.Where(t => t.Activo);

            dbContext.TipoComunidad.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = string.Empty,
                Descripcion = t.Descripcion
            });
        }

        public static IEnumerable<EntidadTipoDTe> GetTiposAutorizacion()
        {
            var dbContext = new GeneralSchema();

            var objData = dbContext.TipoAutorizacion;

            dbContext.TipoAutorizacion.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = string.Empty,
                Descripcion = t.Descripcion
            });

        }

        public static IEnumerable<EntidadTipoDTe> GetUnidadesMedida()
        {
            var dbContext = new GeneralSchema();

            var objData = dbContext.UnidadMedida;

            dbContext.UnidadMedida.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = t.Simbolo,
                Descripcion = t.Descripcion
            });

        }
    }
}
