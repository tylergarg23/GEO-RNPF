using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class AnexoFacade
    {
        public static decimal Insert(DocumentoAnexoTableRowDTe documento)
        {
            decimal id = 0;

            var anexoEF = new AnexosPlantacion()
            {
                Plantacion_Id = documento.Plantacion_Id,
                Anexo_Id = documento.Anexo_Id,
                Tamanio = documento.Tamanio,
                Secuencia = documento.Secuencia,
                UriStorageFileName = documento.Ruta,
                UsuarioCreacion = documento.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = documento.UsuarioCreacion,
                FechaModificacion = DateTime.Now
            };

            using (var dbContext = new PlantacionSchema())
            {
                dbContext.AnexosPlantacionSet.Add(anexoEF);
                dbContext.SaveChanges();

                id = anexoEF.Id;
            }

            return id;
        }

        public static List<DocumentoAnexoTableRowDTe> GetByPlantacionId(int id)
        {

            var anexos = new List<DocumentoAnexoTableRowDTe>();

            using (var dbContext = new PlantacionSchema())
            {

                anexos = (from a in dbContext.AnexosSet
                          join ap in dbContext.AnexosPlantacionSet.Where(p => p.Plantacion_Id.Equals(id)) on a.Id equals ap.Anexo_Id into DocumentosAnexos
                          from da in DocumentosAnexos.DefaultIfEmpty()
                          select new DocumentoAnexoTableRowDTe()
                          {
                              Id = (da.Id != null) ? da.Id : 0,
                              Descripcion = a.Descripcion,
                              Secuencia = a.Secuencia,
                              Plantacion_Id = (da.Plantacion_Id != null) ? da.Plantacion_Id : 0,
                              Anexo_Id = (da.Anexo_Id != null) ? da.Anexo_Id : a.Id,
                              Ruta = (!string.IsNullOrEmpty(da.UriStorageFileName)) ? da.UriStorageFileName : string.Empty,
                              Tamanio = (da.Tamanio != null && da.Tamanio.HasValue) ? da.Tamanio.Value : 0,
                              UsuarioCreacion = (!string.IsNullOrEmpty(da.UsuarioCreacion)) ? da.UsuarioCreacion : string.Empty,
                              FechaCreacion = (da.FechaCreacion != null && da.FechaCreacion > DateTime.MinValue) ? da.FechaCreacion : DateTime.MinValue
                          }).ToList();
            }

            return anexos;
        }

        public static List<DocumentoAnexoTableRowDTe> GetAll()
        {

            var anexos = new List<DocumentoAnexoTableRowDTe>();

            using (var dbContext = new PlantacionSchema())
            {

                anexos = (from a in dbContext.AnexosSet
                          where a.Activo && a.RegistroPlantacion.Value
                          select new DocumentoAnexoTableRowDTe()
                          {
                              Id = a.Id,
                              Descripcion = a.Descripcion,
                              Secuencia = a.Secuencia,
                              Plantacion_Id = 0,
                              Anexo_Id = a.Id,
                              Ruta = string.Empty,
                              Tamanio = 0,
                              UsuarioCreacion = string.Empty,
                              FechaCreacion = DateTime.MinValue
                          }).ToList();
            }

            return anexos;
        }

        public static bool DeleteById(decimal id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.AnexosPlantacionSet.Find(id);

                    if (efObject != null)
                    {
                        dbContext.AnexosPlantacionSet.Remove(efObject);

                        dbContext.SaveChanges();
                    }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;
        }


    }
}
