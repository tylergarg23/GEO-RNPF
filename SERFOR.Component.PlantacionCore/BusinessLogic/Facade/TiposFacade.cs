using SERFOR.Component.DTEntities.General;
using SERFOR.Component.PlantacionCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class TiposFacade
    {
        public static IEnumerable<EntidadTipoDTe> GetFinalidadProducto()
        {
            var dbContext = new PlantacionSchema();

            var objData = dbContext.FinalidadProductoSet;

            dbContext.FinalidadProductoSet.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = string.Empty,
                Descripcion = t.Descripcion
            });
        }

        public static IEnumerable<EntidadTipoDTe> GetSistemasPlantacion()
        {
            var dbContext = new PlantacionSchema();

            var objData = dbContext.SistemaPlantacion;

            dbContext.SistemaPlantacion.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = t.Id,
                SufijoAcronimo = t.Tipo,
                Descripcion = t.Descripcion
            });
        }
        public static IEnumerable<EntidadTipoDTe> GetTiposPlantado()
        {
            var dbContext = new PlantacionSchema();

            var objData = dbContext.TipoPlantacion;

            dbContext.TipoPlantacion.AsNoTracking();

            return objData.Select(t => new EntidadTipoDTe
            {
                Id = 0,
                SufijoAcronimo = t.Id,
                Descripcion = t.Descripcion,
                
            });
        }
        public static IEnumerable<EntidadTipoGeoDTe> GetSistemaCoordenadas()
        {
            var dbContext = new PlantacionSchema();

            var objData = dbContext.SistemaCoordenadaSet;

            dbContext.SistemaCoordenadaSet.AsNoTracking();

            return objData.Select(t => new EntidadTipoGeoDTe
            {
                Id = t.Id,
                SufijoAcronimo = string.Empty,
                Descripcion = t.Datum+ " Zona "+t.Zona,
                LatitudMin = t.LatitudMin,
                LatitudMax = t.LatitudMax,
                LongitudMin = t.LongitudMin,
                LongitudMax = t.LongitudMax
            });
        }
    }
}
