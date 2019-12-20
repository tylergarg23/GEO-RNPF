using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class RevisionFacade
    {
        public static decimal Insert(RevisionPlantacionTableRowDTe revision, bool aprobadoATFFS, bool aprobadoDIR, bool aprobadoCatastro)
        {
            decimal id = 0;

            var revisionEF = new RevisionesRegistroPlantaciones()
            {
                Plantacion_Id = revision.Plantacion_Id,
                Descripcion = revision.Descripcion,
                EsAprobado = revision.EsAprobado,
                Rol = revision.Rol,
                UsuarioCreacion = revision.UsuarioCreacion,
                FechaCreacion = DateTime.Now
            };

            using (var dbContext = new PlantacionSchema())
            {
                var plantacionEF = dbContext.PlantacionSet.Find(revision.Plantacion_Id);
                if (!plantacionEF.AprobadoDIR) plantacionEF.AprobadoDIR = aprobadoDIR;
                if (!plantacionEF.AprobadoEspecialista) plantacionEF.AprobadoEspecialista = aprobadoATFFS;
                if (!plantacionEF.AprobadoCatastro) plantacionEF.AprobadoCatastro = aprobadoCatastro;
                
                if (aprobadoATFFS && string.IsNullOrEmpty(plantacionEF.NumeroCertificado))
                {

                }

                dbContext.RevisionesRegistroPlantacionesSet.Add(revisionEF);
                dbContext.SaveChanges();

                id = revisionEF.Id;
            }

            return id;
        }

        public static object DeleteById(int id)
        {

            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.RevisionesRegistroPlantacionesSet.Find(id);

                    if (efObject != null)
                    {

                        dbContext.RevisionesRegistroPlantacionesSet.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;

        }

        public static RevisionPlantacionTableRowDTe GetRowById(int id)
        {
            RevisionPlantacionTableRowDTe revision = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.RevisionesRegistroPlantacionesSet.Find(id);

                if (efObject != null)
                {
                    revision = new RevisionPlantacionTableRowDTe()
                    {
                        Id = efObject.Id,
                        Descripcion = efObject.Descripcion,
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        Rol = efObject.Rol,
                        FechaCreacion = efObject.FechaCreacion,
                        FechaEvento = efObject.FechaCreacion,
                        EsAprobado = efObject.EsAprobado,
                        Plantacion_Id = efObject.Plantacion_Id
                    };

                }
            }

            return revision;
        }

        public static IEnumerable<RevisionPlantacionTableRowDTe> GetByPlantacionId(int id)
        {

            var revisiones = new List<RevisionPlantacionTableRowDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var efObjects = dbContext.RevisionesRegistroPlantacionesSet.Where(r => r.Plantacion_Id == id);

                if (efObjects.Count() > 0)
                {
                    foreach (var revision in efObjects)
                    {
                        var newRevision = new RevisionPlantacionTableRowDTe()
                        {
                            Id = revision.Id,
                            Descripcion = revision.Descripcion,
                            UsuarioCreacion = revision.UsuarioCreacion,
                            Rol = revision.Rol,
                            FechaCreacion = revision.FechaCreacion,
                            FechaEvento = revision.FechaCreacion,
                            EsAprobado = revision.EsAprobado,
                            Plantacion_Id = id
                        };

                        revisiones.Add(newRevision);

                    }

                }
            }

            return revisiones;
        }

    }
}