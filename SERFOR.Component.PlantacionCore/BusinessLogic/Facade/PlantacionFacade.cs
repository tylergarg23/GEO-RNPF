using EG = SERFOR.Component.DTEntities.General;
using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERFOR.Component.Tools.GeoConverter;
using QRCoder;
using SERFOR.Component.Tools.Cryptography;
using System.Data.Entity.Core.Objects;
using SERFOR.Component.DTEntities.General; //TYLER 17.12.19
using SERFOR.Component.DTEntities.Seguridad; //TYLER 17.12.19
using System.Collections; //TYLER 17.12.19
using System.Data.Entity;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class PlantacionFacade
    {
        public static object name { get; private set; }

        #region Metodos de la Plantacion

        public static bool DeleteById(int id)
        {

            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionSet.Find(id);

                    if (efObject != null)
                    {

                        dbContext.PlantacionSet.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;
        }
        public static bool GeneraLocalKey(int id)
        {
            bool result = false;
            using (var context = new PlantacionSchema())
            {
                context.sp_GeneraLocalKey(id);

                result = true;
            }

            return result;
        }
        public static bool AnularById(int id, string usuario)
        {

            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionSet.Find(id);

                    if (efObject != null)
                    {
                        efObject.Estado = "A";
                        efObject.UsuarioModificacion = usuario;
                        efObject.FechaModificacion = DateTime.Now;

                        dbContext.SaveChanges();
                    }
                }
                catch { exito = false; }
            }

            return exito;
        }

        public static int InsertSectionOne(PlantacionTableRowDTe plantacion)
        {
            int id = 0;

            var plantacionEF = new Plantacion()
            {
                NumeroCertificado = string.Empty,
                Zona = string.Empty,
                Datum = string.Empty,
                NombrePredio = string.Empty,
                Area = decimal.Zero,
                AprobadoCatastro = false,
                AprobadoEspecialista = false,
                AprobadoDIR = false,
                EsPropietario = false,
                SedePrincipal_Id = plantacion.SedePrincipal_Id,
                Sede_Id = plantacion.Sede_Id,
                FechaRecepcionARFFS = plantacion.FechaRecepcionARFFS,
                FechaRecepcion = plantacion.FechaRecepcion,
                UsuarioCreacion = plantacion.UsuarioCreacion,
                FechaCreacion = plantacion.FechaCreacion = DateTime.Now,
                UsuarioModificacion = plantacion.UsuarioCreacion,
                FechaModificacion = plantacion.FechaCreacion = DateTime.Now,
            };

            using (var dbContext = new PlantacionSchema())
            {
                dbContext.PlantacionSet.Add(plantacionEF);
                dbContext.SaveChanges();

                id = plantacionEF.Id;
            }

            return id;
        }

        public static int UpdateSectionOne(int id, PlantacionTableRowDTe plantacion)
        {

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    
                    if (!string.IsNullOrEmpty(plantacion.NumeroPredio))
                    {
                       var foundPredio = dbContext.PlantacionSet.Where(p => p.NumeroPredio == plantacion.NumeroPredio).First();

                        if (foundPredio != null && foundPredio.Id != id)
                        {
                            throw new Exception("Error Ya existe ese predio");
                        }
                    }

                    var efObject = dbContext.PlantacionSet.Find(id);
                    if (efObject != null)
                    {
                        efObject.FechaRecepcionARFFS = plantacion.FechaRecepcionARFFS;
                        efObject.FechaRecepcion = plantacion.FechaRecepcion;

                        if (!string.IsNullOrEmpty(plantacion.NumeroPredio))
                        {
                            efObject.NumeroPredio = plantacion.NumeroPredio;
                        }

                        if (efObject.SeccionAvance < 1)
                            efObject.SeccionAvance = 1;

                        efObject.UsuarioModificacion = plantacion.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        dbContext.SaveChanges();
                    }
                }
                catch { id = 0; }

            }

            return id;
        }

        public static int UpdateSectionTwo(int id, PlantacionTableRowDTe plantacion)
        {

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    if (!string.IsNullOrEmpty(plantacion.NumeroPredio))
                    {
                        var foundPredio = dbContext.PlantacionSet.Where(p => p.NumeroPredio == plantacion.NumeroPredio).FirstOrDefault();

                        if (foundPredio != null && foundPredio.Id != id)
                        {
                            throw new Exception("Error Ya existe ese predio");
                        }
                    }
                    var efObject = dbContext.PlantacionSet.Find(id);

                    if (efObject != null)
                    {
                        if(!string.IsNullOrEmpty(plantacion.NumeroPredio))
                        {
                            efObject.NumeroPredio = plantacion.NumeroPredio;
                        }

                        efObject.NombrePredio = plantacion.NombrePredio;
                        efObject.Area = plantacion.Area;
                        efObject.EsPropietario = plantacion.EsPropietario;
                        efObject.EsInversionista = plantacion.EsInversionista;
                        efObject.EsPosesionario = plantacion.EsPosesionario;
                        efObject.Ubigeo_Id = plantacion.Ubigeo_Id;
                        efObject.TipoZona_Id = plantacion.TipoZona_Id;
                        efObject.TipoComunidad_Id = plantacion.TipoComunidad_Id;
                        efObject.NombreZona = plantacion.NombreZona;
                        efObject.DocumentoCondicion = plantacion.DocumentoCondicion;
                        efObject.DocumentoContrato = plantacion.DocumentoContrato;
                        efObject.SistemaCoordenada_Id = plantacion.SistemaCoordenada_Id;
                        efObject.CoordenadaEsteUTM = plantacion.CoordenadaEsteUTM;
                        efObject.CoordenadaNorteUTM = plantacion.CoordenadaNorteUTM;

                        if (plantacion.TipoAutorizacion_Id > 0)
                            efObject.TipoAutorizacion_Id = plantacion.TipoAutorizacion_Id;
                        else
                            efObject.TipoAutorizacion_Id = null;

                        if (efObject.SeccionAvance < 2)
                            efObject.SeccionAvance = 2;

                        efObject.Observaciones = plantacion.Observaciones;

                        efObject.UsuarioModificacion = plantacion.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        dbContext.SaveChanges();
                    }

                }
                catch { id = 0; }
            }

            return id;
        }

        public static int UpdateStatusGeo(PlantacionTableRowDTe plantacion)
        {
            int id = plantacion.Id;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {                    
                    var efObject = dbContext.PlantacionSet.Find(plantacion.Id);

                    if (efObject != null)
                    {
                        efObject.EstadoGeo = plantacion.EstadoGeo;
                        efObject.UsuarioModificacion = plantacion.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        dbContext.SaveChanges();
                    }

                }
                catch { 
                    id = 0; }
            }

            return id;
        }

        public static int UpdateObjectId(PlantacionTableRowDTe plantacion)
        {
            int id = plantacion.Id;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionSet.Find(plantacion.Id);

                    if (efObject != null)
                    {
                      //  dbContext.Entry(efObject).State = EntityState.Modified;
                        efObject.ObjectId = plantacion.ObjectId;
                        efObject.UsuarioModificacion = plantacion.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        //dbContext.PlantacionSet.Update

                        //  dbContext.Update(efObject);
                       // dbContext.
                        dbContext.SaveChanges();
                    }

                }
                catch(Exception e)
                {
                    id = 0;
                }
            }

            return id;
        }

        public static IEnumerable<PlantacionItemListDTe> GetAll()
        {
            var dbContext = new PlantacionSchema();

            IQueryable<Plantacion> query = dbContext.PlantacionSet.Where(p => p.Estado == null);

            IEnumerable<PlantacionItemListDTe> plantaciones = query.Select(p => new PlantacionItemListDTe()
            {
                Id = p.Id,
                NumeroCertificado = p.NumeroCertificado,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                AprobadoEspecialista = p.AprobadoEspecialista,
                AprobadoDIR = p.AprobadoDIR,
                AprobadoCatastro = p.AprobadoCatastro,
                Condicion = (p.EsPropietario) ? "Propietario" : "Inversionista",
                //Area = (p.CantidadTotalSuperficieHa.HasValue) ? p.CantidadTotalSuperficieHa.Value : 0,
                Area = (p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0,
                //Area = p.PlantacionDetalle.Select(x => x.CantidadSuperficieHa).Sum(),
                NombreZona = (p.TipoZona_Id.HasValue) ? p.TipoZona.Descripcion + " " + p.NombreZona : string.Empty,
                Numero = p.PlantacionDetalle.Count,
                Titular =
                p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.Nombres.ToUpper(),
                Representante =
                p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.Nombres.ToUpper(),
                Brigadista =
                p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.Nombres.ToUpper(),
                Especie =
                p.PlantacionDetalle.FirstOrDefault().PlantacionDetalleEspecie.FirstOrDefault().Especie.Nombre
            });

            dbContext.PlantacionSet.AsNoTracking();

            return plantaciones;
        }

        public static Resultado GetRNP(string order, int offset, int limit, string search, string sort)
        {


            var dbContext = new PlantacionSchema();

            var query1 = dbContext.PlantacionSet
                                             .Where(p => p.Estado != null &&
                                                        (p.Id.ToString().Contains(search) || p.NumeroCertificado.Contains(search) || p.Ubigeo.NombreDepartamento.Contains(search) || /*p.Ubigeo.NombreProvincia.Contains(search) || */p.Ubigeo.NombreDistrito.Contains(search)//TYLER 17.12.19
                                                        || p.NombreZona.Contains(search) || ((p.EsPropietario) ? "Propietario" : ((p.EsInversionista) ? "Inversionista" : "Posesionario")).Contains(search)
                                                        || ((p.Estado == "A") ? "ANULADO" : "REGISTRADO").Contains(search)
                                                        )
                                                    );


            //if(order="asc")
            var total = query1.Count();
            var query2 = query1.OrderBy(s => s.Id);
            switch (sort)
            {
                case "Id":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Id);
                    else query2 = query1.OrderBy(s => s.Id); break;
                case "NumeroCertificado":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.NumeroCertificado);
                    else query2 = query1.OrderBy(s => s.NumeroCertificado); break;
                case "Departamento":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Ubigeo.NombreDepartamento);
                    else query2 = query1.OrderBy(s => s.Ubigeo.NombreDepartamento); break;
                case "Provincia":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Ubigeo.NombreProvincia);
                    else query2 = query1.OrderBy(s => s.Ubigeo.NombreProvincia); break;
                case "Distrito":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Ubigeo.NombreDistrito);
                    else query2 = query1.OrderBy(s => s.Ubigeo.NombreDistrito); break;
                case "NombreZona":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.NombreZona);
                    else query2 = query1.OrderBy(s => s.NombreZona); break;
                case "Condicion":
                    if (order == "desc") query2 = query1.OrderByDescending(s => ((s.EsPropietario) ? "Propietario" : ((s.EsInversionista) ? "Inversionista" : "Posesionario")));
                    else query2 = query1.OrderBy(s => ((s.EsPropietario) ? "Propietario" : ((s.EsInversionista) ? "Inversionista" : "Posesionario"))); break;
                case "Area":
                    if (order == "desc") query2 = query1.OrderByDescending(s => ((s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0));
                    else query2 = query1.OrderBy(s => ((s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0)); break;
                case "Estado":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Estado);
                    else query2 = query1.OrderBy(s => s.Estado); break;
            }


            IQueryable<Plantacion> query;
            if (offset != -1)

                query = query2.Skip(offset).Take(limit);
            else
                query = query2;

            IEnumerable<PlantacionItemListDTe> plantaciones = query.Select(p => new PlantacionItemListDTe()
            {
                Id = p.Id,
                NumeroCertificado = p.NumeroCertificado,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                AprobadoEspecialista = p.AprobadoEspecialista,
                AprobadoDIR = p.AprobadoDIR,
                AprobadoCatastro = p.AprobadoCatastro,
                Condicion = (p.EsPropietario) ? "Propietario" : ((p.EsInversionista) ? "Inversionista" : "Posesionario"),
                //Area = (p.CantidadTotalSuperficieHa.HasValue) ? p.CantidadTotalSuperficieHa.Value : 0,
                Area = (p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0,
                NombreZona = (p.TipoZona_Id.HasValue) ? p.TipoZona.Descripcion + " " + p.NombreZona : string.Empty,
                Numero = p.PlantacionDetalle.Count,
                Titular =
                p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.Nombres.ToUpper(),
                Representante =
                p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.Nombres.ToUpper(),
                Brigadista =
                p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.Nombres.ToUpper(),
                Estado = (p.Estado == "A") ? "ANULADO" : "REGISTRADO"

            });

            Resultado res = new Resultado();
            res.total = total;
            res.rows = plantaciones;

            dbContext.PlantacionSet.AsNoTracking();

            return res;
        }

        public static IEnumerable<PlantacionItemListDTe> GetBySedeId(short id, bool esPrincipal)
        {

            IQueryable<Plantacion> query = null;

            var dbContext = new PlantacionSchema();

            if (esPrincipal)
            {
                query = dbContext.PlantacionSet.Where(p => p.SedePrincipal_Id == id && p.Estado == null);
            }
            else
            {
                query = dbContext.PlantacionSet.Where(p => p.Sede_Id == id && p.Estado == null);
            }


            IEnumerable<PlantacionItemListDTe> plantaciones = query.Select(p => new PlantacionItemListDTe()
            {
                Id = p.Id,
                NumeroCertificado = p.NumeroCertificado,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                AprobadoEspecialista = p.AprobadoEspecialista,
                AprobadoDIR = p.AprobadoDIR,
                AprobadoCatastro = p.AprobadoCatastro,
                Condicion = (p.EsPropietario) ? "Propietario" : "Inversionista",
                //Area = (p.CantidadTotalSuperficieHa.HasValue) ? p.CantidadTotalSuperficieHa.Value : 0,
                Area = (p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0,
                NombreZona = (p.TipoZona_Id.HasValue) ? p.TipoZona.Descripcion + " " + p.NombreZona : "",
                Numero = p.PlantacionDetalle.Count,
                Titular =
                p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.Nombres.ToUpper(),
                Representante =
                p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.Nombres.ToUpper(),
                Brigadista =
                p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.Nombres.ToUpper(),
                Especie =
                p.PlantacionDetalle.FirstOrDefault().PlantacionDetalleEspecie.FirstOrDefault().Especie.Nombre
            });

            dbContext.PlantacionSet.AsNoTracking();

            return plantaciones;
        }
        public static Resultado GetRNPBySedeId(short id, bool esPrincipal, string order, int offset, int limit, string search, string sort)
        {

            var dbContext = new PlantacionSchema();
            IQueryable<Plantacion> query1;

            if (esPrincipal)
            {
                query1 = dbContext.PlantacionSet.Where(p => p.SedePrincipal_Id == id && p.Estado != null &&
                                                        (p.Id.ToString().Contains(search) || p.NumeroCertificado.Contains(search) || p.Ubigeo.NombreDepartamento.Contains(search) || p.Ubigeo.NombreProvincia.Contains(search) ||p.Ubigeo.NombreDistrito.Contains(search) //TYLER 17.12.19
                                                        || p.NombreZona.Contains(search) || ((p.EsPropietario) ? "Propietario" : ((p.EsInversionista) ? "Inversionista" : "Posesionario")).Contains(search)
                                                        || ((p.Estado == "A") ? "ANULADO" : "REGISTRADO").Contains(search)
                                                        )
                                                    );
            }
            else
            {
                query1 = dbContext.PlantacionSet.Where(p => p.Sede_Id == id && p.Estado != null &&
                                                        (p.Id.ToString().Contains(search) || p.NumeroCertificado.Contains(search) || p.Ubigeo.NombreDepartamento.Contains(search) || p.Ubigeo.NombreProvincia.Contains(search) || p.Ubigeo.NombreDistrito.Contains(search)
                                                        || p.NombreZona.Contains(search) || ((p.EsPropietario) ? "Propietario" : ((p.EsInversionista) ? "Inversionista" : "Posesionario")).Contains(search)
                                                        || ((p.Estado == "A") ? "ANULADO" : "REGISTRADO").Contains(search)
                                                        )
                                                    );
            }


            var total = query1.Count();
            var query2 = query1.OrderBy(s => s.Id);
            switch (sort)
            {
                case "Id":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Id);
                    else query2 = query1.OrderBy(s => s.Id); break;
                case "NumeroCertificado":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.NumeroCertificado);
                    else query2 = query1.OrderBy(s => s.NumeroCertificado); break;
                case "Departamento":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Ubigeo.NombreDepartamento);
                    else query2 = query1.OrderBy(s => s.Ubigeo.NombreDepartamento); break;
                case "Provincia":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Ubigeo.NombreProvincia);
                    else query2 = query1.OrderBy(s => s.Ubigeo.NombreProvincia); break;
                case "Distrito":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Ubigeo.NombreDistrito);
                    else query2 = query1.OrderBy(s => s.Ubigeo.NombreDistrito); break;
                case "NombreZona":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.NombreZona);
                    else query2 = query1.OrderBy(s => s.NombreZona); break;
                case "Condicion":
                    if (order == "desc") query2 = query1.OrderByDescending(s => ((s.EsPropietario) ? "Propietario" : ((s.EsInversionista) ? "Inversionista" : "Posesionario")));
                    else query2 = query1.OrderBy(s => ((s.EsPropietario) ? "Propietario" : ((s.EsInversionista) ? "Inversionista" : "Posesionario"))); break;
                case "Area":
                    if (order == "desc") query2 = query1.OrderByDescending(s => ((s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0));
                    else query2 = query1.OrderBy(s => ((s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? s.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0)); break;
                case "Estado":
                    if (order == "desc") query2 = query1.OrderByDescending(s => s.Estado);
                    else query2 = query1.OrderBy(s => s.Estado); break;
            }



            IQueryable<Plantacion> query;
            if (offset != -1)

                query = query2.Skip(offset).Take(limit);
            else
                query = query2;

            IEnumerable<PlantacionItemListDTe> plantaciones = query.Select(p => new PlantacionItemListDTe()
            {
                Id = p.Id,
                NumeroCertificado = p.NumeroCertificado,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                AprobadoEspecialista = p.AprobadoEspecialista,
                AprobadoDIR = p.AprobadoDIR,
                AprobadoCatastro = p.AprobadoCatastro,
                Condicion = (p.EsPropietario) ? "Propietario" : ((p.EsInversionista) ? "Inversionista" : "Posesionario"),
                //Area = (p.CantidadTotalSuperficieHa.HasValue) ? p.CantidadTotalSuperficieHa.Value : 0,
                Area = (p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0,
                NombreZona = (p.TipoZona_Id.HasValue) ? p.TipoZona.Descripcion + " " + p.NombreZona : string.Empty,
                Numero = p.PlantacionDetalle.Count,
                Titular =
               p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
               + " " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
               + ", " + p.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.Nombres.ToUpper(),
                Representante =
               p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
               + " " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
               + ", " + p.PersonaPlantacion.Where(x => x.Rol == "R").FirstOrDefault().Persona.Nombres.ToUpper(),
                Brigadista =
               p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
               + " " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
               + ", " + p.PersonaPlantacion.Where(x => x.Rol == "B").FirstOrDefault().Persona.Nombres.ToUpper(),
                Estado = (p.Estado == "A") ? "ANULADO" : "REGISTRADO"

            });

            Resultado res = new Resultado();
            res.total = total;
            res.rows = plantaciones;

            dbContext.PlantacionSet.AsNoTracking();

            return res;
        }

        private static byte tipoComunidad(byte? id, byte? zona)
        {
            var dbContext = new PlantacionSchema();
           var tipoZonaComunidad= dbContext.TipoZonaSet.Find(id);
            if(string.Equals("Predios Privados", tipoZonaComunidad.Descripcion.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            else
            {
                if (string.Equals("Comunidades", tipoZonaComunidad.Descripcion.Trim(), StringComparison.OrdinalIgnoreCase)) {
                    var comunidad = dbContext.TipoComunidadSet.Find(zona);
                    if(string.Equals("Nativas", comunidad.Descripcion.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return 3;
                    }
                    else
                    {
                        if (string.Equals("Campesinas", comunidad.Descripcion.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            return 2;
                        }
                    }
                }

            }
            return 0;
        }
        //Tyler 17-12-2019 - INICIO
        public static IList<ResponseFeature> GetAllRowById(int id)
        {
            var guion = "-"; //TYLER 19.12.19
            IList<ResponseFeature> listaResponse = new List<ResponseFeature>();
            ResponseFeature r = new ResponseFeature();
            var dbContext = new PlantacionSchema();
            PlantacionTableRowDTe plantacion = GetRowById(id);
          // plantacion.TipoComunidad_Id
            PlantacionesFeatureLayer layer = new PlantacionesFeatureLayer();
           
            layer.ObjectId = plantacion.ObjectId;
            layer.FUENTE = guion;
            layer.DOCREG = guion;
            layer.FECREG = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            layer.OBSERV = guion;
            // layer.ORIGEN = guion;
            layer.DOCASA = guion;
            layer.DOCTIT = guion;
            layer.DOCAUT = guion;
            layer.CONTRA = guion;

            layer.SUPSIG = plantacion.Area;
            layer.NUMREG = plantacion.NumeroCertificado;
            layer.NOMDEP = plantacion.CodigoDepartamento;
            layer.NOMPRO = plantacion.CodigoDepartamento + plantacion.CodigoProvincia;
            layer.NOMDIS = plantacion.CodigoDepartamento + plantacion.CodigoProvincia +
              plantacion.CodigoDistrito;

            byte tipoC = tipoComunidad(plantacion.TipoZona_Id, plantacion.TipoComunidad_Id);
            if(tipoC != 0)
            {
                layer.TIPCOM = tipoC;
            }
                
            //plantacion.

            decimal superficieInicial = 0;
            foreach(BloqueTableRowDTe d in plantacion.Detalles)
            {
                superficieInicial += d.CantidadSuperficieHa.HasValue? d.CantidadSuperficieHa.Value:0;
            }

            layer.SUPAPR = superficieInicial;
           
            
            layer.COORES = Convert.ToDouble(plantacion.CoordenadaEsteUTM);
            layer.COORNO = Convert.ToDouble(plantacion.CoordenadaNorteUTM);
           

            if (plantacion.Especies != null && plantacion.Especies.Count >0)
            {
                foreach(EspecieItemListDTe e in plantacion.Especies)
                {
                    layer.ESPECI += e.NombreComun+ ",";
                }
            }

            if(plantacion.Personas != null && plantacion.Personas.Count > 0)
            {
                var docAll = dbContext.TipoDocumento.ToList();

              
                foreach (PersonaTableRowDTe p in plantacion.Personas)
                {
                    if(string.Equals(p.Rol, "Responsable legal", StringComparison.OrdinalIgnoreCase))
                    {
                        var arr = p.Documento.Split(':');
                        if(arr.Length > 1)
                        {
                            layer.TIPDOC = p.TipoDocumento_Id;
                            layer.NUMRUC = arr[1];
                        }
                       
                        layer.NOMREL = p.NombreCompleto;
                    }

                    if (string.Equals(p.Rol, "Titular", StringComparison.OrdinalIgnoreCase))
                    {
                        var arr =  p.Documento.Split(':');
                        if (arr.Length > 1)
                        {
                            layer.TIPDOC = p.TipoDocumento_Id;
                            layer.NRODOC = arr[1];
                        }
                            
                        layer.NOMTIT = p.NombreCompleto;
                     

                    }
                }
            }

            if (plantacion.Detalles != null && plantacion.Detalles.Count > 0)
            {
                foreach(BloqueTableRowDTe bloque in plantacion.Detalles)
                {
                    layer.FINALI = bloque.Finalidad_Id;
                    break;
                }
            }

            if(plantacion.TipoComunidad_Id != null)
            {
               
                var tipoComunidad = dbContext.TipoComunidadSet.ToList();
                foreach(TipoComunidad com in tipoComunidad)
                {
                    if(com.Id == plantacion.TipoComunidad_Id)
                    {
                        layer.NOMCCN = com.Descripcion;
                    }
                         
                }
            }

            IEnumerable<BloqueCoordenadasDTE> listaBloques = GetCoordenadasByPlantacionId(id);

            if(listaBloques.Count() > 0)
            {
                layer.ZONUTM = listaBloques.FirstOrDefault().Zona;
                layer.FECPLA = listaBloques.FirstOrDefault().AnnioEstablecimiento.ToString()
                    + "-" + listaBloques.FirstOrDefault().MesEstablecimiento.ToString() + "-1";
               
                r.geometry = obtenerBloquesVertices(listaBloques);
            }

            layer.AUTFOR = obtenerAutoridadFor(plantacion.NombreAutoridad);
            r.attributes = layer;
            listaResponse.Add(r);
           return listaResponse;
        }

        private static int obtenerAutoridadFor(string nombreAutoridad)
        {
            int autoridad = 0;
            switch (nombreAutoridad.Trim())
            {
                case "SERFOR":
                            autoridad = 1;
                            break;
                case "GORE Loreto":
                    autoridad = 2;
                    break;
                case "GORE Ucayali":
                    autoridad = 3;
                    break;
                case "GORE Huánuco":
                    autoridad = 4;
                    break;
                case "GORE Amazonas":
                    autoridad = 5;
                    break;
                case "GORE San Martín":
                    autoridad = 6;
                    break;
                case "GORE Madre de Dios":
                    autoridad = 7;
                    break;
                case "GORE Ayacucho":
                    autoridad = 8;
                    break;
                case "GORE La Libertad":
                    autoridad = 9;
                    break;
                case "GORE Tumbes":
                    autoridad = 10;
                    break;
                case "ATFFS Áncash":
                    autoridad = 11;
                    break;
                case "ATFFS Apurímac":
                    autoridad = 12;
                    break;
                case "ATFFS Arequipa":
                    autoridad = 13;
                    break;
                case "ATFFS Cajamarca":
                    autoridad = 14;
                    break;
                case "ATFFS Cusco":
                    autoridad = 15;
                    break;
                case "ATFFS Ica":
                    autoridad = 16;
                    break;
                case "ATFFS Lambayeque":
                    autoridad = 17;
                    break;
                case "ATFFS Lima":
                    autoridad = 18;
                    break;
                case "ATFFS Moquegua-Tacna":
                    autoridad = 19;
                    break;
                case "ATFFS Selva Central":
                    autoridad = 20;
                    break;
                case "ATFFS Sierra Central":
                    autoridad = 21;
                    break;
                case "ATFFS Piura":
                    autoridad = 22;
                    break;
                case "ATFFS Puno":
                    autoridad = 23;
                    break;
            }

            return autoridad;
        }
        //TYLER 17.12.19 - FIN

        //TYLER 17.12.19 - INICIO
        private static GeometryFeature obtenerBloquesVertices(IEnumerable<BloqueCoordenadasDTE> listaBloques)
        {
            GeometryFeature geometryFeature = new GeometryFeature();
          
                SpatialReference spatialReference = new SpatialReference();
                geometryFeature.spatialReference = spatialReference;
                spatialReference.wkid = 4326;
                IList principal = new List<IList>(); //"[]"
                //principal.Add(new List<String>());
                foreach(BloqueCoordenadasDTE b in listaBloques)
                {
              
                    IList secundario = new List<IList>(); //[[],[]]
                    foreach(CoordenadaTableRowDTe c in b.Coordenadas)
                    {
                        IList tercero = new List<double>();
                        tercero.Add(c.CoordenadaGeografica.Longitud);
                        tercero.Add(c.CoordenadaGeografica.Latitud);
                        
                        secundario.Add(tercero);
                    }

                    principal.Add(secundario);
                }

                geometryFeature.rings = principal;

            return geometryFeature;
        }

        //TYLER 17.12.19 - FIN
        public static PlantacionTableRowDTe GetRowById(int id)
        {
            PlantacionTableRowDTe plantacion = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PlantacionSet.Find(id);

                if (efObject != null)
                {
                    //TYLER 20/11/19 FECHA FIRMA
                    if (efObject.FechaFirma == null)
                    {
                        efObject.FechaFirma = DateTime.Now;
                        dbContext.SaveChanges();
                    } //Fin del bloque
                    plantacion = new PlantacionTableRowDTe()
                    {
                        Id = efObject.Id,
                        NombrePredio = efObject.NombrePredio,
                        Area = (efObject.Area.HasValue) ? efObject.Area.Value : decimal.Zero,
                        NumeroCertificado = efObject.NumeroCertificado,
                        TipoZona_Id = efObject.TipoZona_Id,
                        TipoComunidad_Id = efObject.TipoComunidad_Id,
                        NombreZona = efObject.NombreZona,
                        EsPropietario = efObject.EsPropietario,
                        EsInversionista = efObject.EsInversionista,
                        EsPosesionario = efObject.EsPosesionario,
                        TipoAutorizacion_Id = efObject.TipoAutorizacion_Id,
                        DocumentoCondicion = efObject.DocumentoCondicion,
                        DocumentoContrato = efObject.DocumentoContrato,
                        FechaRecepcionARFFS = efObject.FechaRecepcionARFFS,
                        AprobadoEspecialista = efObject.AprobadoEspecialista,
                        AprobadoDIR = efObject.AprobadoDIR,
                        AprobadoCatastro = efObject.AprobadoCatastro,
                        CantidadTotalSuperficieMl = efObject.CantidadTotalSuperficieMl,
                        CantidadTotalSuperficieHa = efObject.CantidadTotalSuperficieHa,
                        Observaciones = efObject.Observaciones,
                        SeccionAvance = efObject.SeccionAvance,
                        Sede_Id = efObject.Sede_Id,
                        SedePrincipal_Id = efObject.SedePrincipal_Id,
                        Sede = efObject.SedeAutoridadForestal.Nombre,
                        LogoARFFS = efObject.SedeAutoridadForestal.AutoridadForestal.UriStorageLogo,
                        NombreAutoridad = efObject.SedeAutoridadForestal.AutoridadForestal.Nombre, //Tyler 17-12-2019
                        FechaRecepcion = efObject.FechaRecepcion,
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaCreacion = efObject.FechaCreacion,
                        UsuarioModificacion = efObject.UsuarioModificacion,
                        FechaModificacion = efObject.FechaModificacion,
                        CantidadRevisiones = efObject.RevisionesRegistroPlantaciones.Count,
                        CantidadHistoricos = efObject.PlantacionHistorico.Count,
                        SistemaCoordenada_Id = efObject.SistemaCoordenada_Id,
                        CoordenadaEsteUTM = efObject.CoordenadaEsteUTM,
                        CoordenadaNorteUTM = efObject.CoordenadaNorteUTM,
                        Titular = "",
                        LocalKey = efObject.LocalKey,
                        Estado = efObject.Estado,
                        FechaFirma = efObject.FechaFirma, //TYLER 20/11/19 FECHA FIRMA
                        EstadoGeo = efObject.EstadoGeo,  //TYLER 20/11/19 FECHA FIRMA
                        NumeroPredio = efObject.NumeroPredio,
                        ObjectId = efObject.ObjectId//TYLER 20/11/19 FECHA FIRMA
                    };

                    if (plantacion.LogoARFFS == "") plantacion.LogoARFFS = "/Content/Images/logo_documents.jpg";

                    if (efObject.PersonaPlantacion.Where(x => x.Rol == "T").Count() > 0)
                    {
                        plantacion.Titular = efObject.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoPaterno.ToUpper()
                + " " + efObject.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.ApellidoMaterno.ToUpper()
                + ", " + efObject.PersonaPlantacion.Where(x => x.Rol == "T").FirstOrDefault().Persona.Nombres.ToUpper();
                    }

                    if (efObject.TipoAutorizacion_Id != null && efObject.TipoAutorizacion_Id > 0)
                    {
                        plantacion.Autorizacion = efObject.TipoAutorizacion.Descripcion;
                    }

                    if (efObject.Ubigeo != null && efObject.Ubigeo_Id > 0)
                    {
                        plantacion.Ubigeo_Id = efObject.Ubigeo_Id;
                        plantacion.Departamento = efObject.Ubigeo.NombreDepartamento;
                        plantacion.Provincia = efObject.Ubigeo.NombreProvincia;
                        plantacion.Distrito = efObject.Ubigeo.NombreDistrito;
                        plantacion.CodigoDepartamento = efObject.Ubigeo.CodigoDepartamento;
                        plantacion.CodigoProvincia = efObject.Ubigeo.CodigoProvincia;
                        plantacion.CodigoDistrito = efObject.Ubigeo.CodigoDistrito; //TYLER 17.12.19
                    }

                    if (efObject.PersonaPlantacion.Count() > 0)
                    {
                        foreach (var persona in efObject.PersonaPlantacion)
                        {
                            plantacion.Personas.Add(new EG.PersonaTableRowDTe()
                            {
                                Id = persona.Persona.Id,
                                PersonaPlantacion_Id = persona.Id,
                                Rol = PersonaFacade.GetRolDescripcion(persona.Rol),
                                NombreCompleto = (persona.Persona.EsJuridica) ?
                                                        persona.Persona.Nombres :
                                                        persona.Persona.ApellidoPaterno + " " + persona.Persona.ApellidoMaterno + ", " + persona.Persona.Nombres,
                                Departamento = persona.Ubigeo.NombreDepartamento,
                                Provincia = persona.Ubigeo.NombreProvincia,
                                Distrito = persona.Ubigeo.NombreDistrito,
                                Celular = persona.Celular,
                                Telefono = persona.Telefono,
                                Direccion = persona.Direccion,
                                EsJuridica = persona.Persona.EsJuridica,
                                EstadoCivil = persona.Persona.EstadoCivil,
                                Sexo = persona.Persona.Sexo,
                                Documento = persona.Persona.TipoDocumento.Acronimo + ": " + persona.Persona.NumeroDocumento,
                                Email = persona.Email,
                                TipoPersona = (persona.Persona.EsJuridica) ? "Jurídica" : "Natural",
                                UsuarioCreacion = persona.UsuarioCreacion,
                                FechaCreacion = persona.FechaCreacion,
                                UsuarioModificacion = persona.Persona.UsuarioModificacion,
                                FechaModificacion = persona.Persona.FechaModificacion,
                                ApellidoMaterno = persona.Persona.ApellidoMaterno,
                                ApellidoPaterno = persona.Persona.ApellidoPaterno,
                                Ubigeo_Id = persona.Ubigeo_Id,
                                CodigoDepartamento = persona.Ubigeo.CodigoDepartamento,
                                CodigoProvincia = persona.Ubigeo.CodigoProvincia,
                                FechaNacimiento = persona.Persona.FechaNacimiento,
                                TipoDocumento_Id = persona.Persona.TipoDocumento_Id,
                                Activo = true
                                
                            });
                        }
                    }
                    plantacion.Especies = new List<EspecieItemListDTe>();
                    if (efObject.PlantacionDetalle.Count() > 0)
                    {

                        foreach (var bloque in efObject.PlantacionDetalle)
                        {
                            var newBloque = new BloqueTableRowDTe()
                            {
                                Id = bloque.Id,
                                Nombre = bloque.Nombre,
                                AnnioEstablecimiento = bloque.AnnioEstablecimiento,
                                MesEstablecimiento = bloque.MesEstablecimiento,
                                AreaPredio = bloque.AreaPredio,
                                SistemaCoordenada_Id = bloque.SistemaCoordenada_Id,
                                LatitudMin = bloque.SistemaCoordenada.LatitudMin,
                                LatitudMax = bloque.SistemaCoordenada.LatitudMax,
                                LongitudMin = bloque.SistemaCoordenada.LongitudMin,
                                LongitudMax = bloque.SistemaCoordenada.LongitudMax,
                                CoordenadaEsteUTM = bloque.CoordenadaEsteUTM,
                                CoordenadaNorteUTM = bloque.CoordenadaNorteUTM,

                                //ProduccionEstimada = bloque.ProduccionEstimada,
                                SistemaPlantacion_Id = bloque.SistemaPlantacion_Id,
                                SistemaPlantacionDescripcion = bloque.SistemaPlantacion.Descripcion,
                                SistemaPlantacionTipo = bloque.SistemaPlantacion.Tipo,
                                Plantacion_Id = bloque.Plantacion_Id,
                                //TotalPlantado = bloque.TotalPlantado,
                                //UnidadMedida_Id = bloque.UnidadMedida_Id,
                                //UnidadMedidaDescripcion = bloque.UnidadMedida.Descripcion,
                                //UnidadMedidaSimbolo = bloque.UnidadMedida.Simbolo,
                                CantidadCoordenadasRegistradas = bloque.PlantacionDetalleVertices.Count,
                                CantidadSuperficieHa = bloque.CantidadSuperficieHa,
                                CantidadSuperficieMl = bloque.CantidadSuperficieMl,
                                UriStorageFileName = bloque.UriStorageFileName
                            };

                            if (bloque.Finalidad_Id != null && bloque.Finalidad_Id > 0)
                            {
                                newBloque.Finalidad_Id = bloque.Finalidad_Id;
                                newBloque.FinalidadDescripcion = bloque.FinalidadProducto.Descripcion;
                            }
                            if (bloque.MesEstablecimiento != null)
                            {
                                newBloque.MesDescripcion = getMesDescripcion(bloque.MesEstablecimiento);
                            }
                            if (bloque.PlantacionDetalleEspecie.Count() > 0)
                            {
                                newBloque.Especies = new List<EspecieItemListDTe>();
                                foreach (var especie in bloque.PlantacionDetalleEspecie)
                                {
                                    var newEspecie = new EspecieItemListDTe()
                                    {
                                        Id = especie.Id,
                                        PlantacionDetalle_Id = Convert.ToInt16(especie.PlantacionDetalle_Id),
                                        ProduccionEstimada = especie.ProduccionEstimada,
                                        TotalPlantado = especie.TotalPlantado,
                                        TipoPlantado = especie.TipoPlantado,
                                        UnidadMedida_Id = especie.UnidadMedida_Id,
                                        UnidadMedidaDescripcion = especie.UnidadMedida.Descripcion,
                                        UnidadMedidaSimbolo = especie.UnidadMedida.Simbolo
                                    };
                                    if (especie.TipoPlantado != null) newEspecie.TipoPlantadoDesc = getTipoPlantado(especie.TipoPlantado);
                                    if (especie.Especie != null && especie.Especie_Id > 0)
                                    {

                                        newEspecie.EspecieCodigo = especie.Especie.Codigo;
                                        newEspecie.EspecieNombre = especie.Especie.Nombre;
                                        newEspecie.NombreComun = especie.NombreComun;
                                        newEspecie.Especie_Id = especie.Especie_Id;
                                    }
                                    newEspecie.bloque = newBloque;
                                    newBloque.Especies.Add(newEspecie);
                                    plantacion.Especies.Add(newEspecie);
                                }
                            }
                            plantacion.Detalles.Add(newBloque);

                        }

                    }

                    plantacion.Anexos = AnexoFacade.GetByPlantacionId(id);
                    //sdslfsd
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode("http:\\\\appweb.serfor.gob.pe\\validaRNP\\?id=" + plantacion.LocalKey, QRCodeGenerator.ECCLevel.Q);
                    Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                    plantacion.CodigoQR = qrCode.GetGraphic(20);

                }
            }

            return plantacion;
        }
        private static string getMesDescripcion(byte? num)
        {
            string s = "";
            switch (num)
            {
                case 1: s = "Enero"; break;
                case 2: s = "Febrero"; break;
                case 3: s = "Marzo"; break;
                case 4: s = "Abril"; break;
                case 5: s = "Mayo"; break;
                case 6: s = "Junio"; break;
                case 7: s = "Julio"; break;
                case 8: s = "Agosto"; break;
                case 9: s = "Septiembre"; break;
                case 10: s = "Octubre"; break;
                case 11: s = "Noviembre"; break;
                case 12: s = "Diciembre"; break;
                default: s = ""; break;
            }
            return s;
        }
        private static string getTipoPlantado(string cod)
        {
            string s = "";
            switch (cod)
            {
                case "A": s = "Árboles"; break;
                case "C": s = "Cepas"; break;
                case "M": s = "Matas"; break;
                default: s = ""; break;
            }
            return s;
        }
        public static IEnumerable<EG.BitacoraDetailDTe> GetDatesByPlantacionId(int id)
        {

            var fechas = new List<EG.BitacoraDetailDTe>();

            using (var dbContext = new PlantacionSchema())
            {

                var efObject = dbContext.PlantacionSet.Find(id);
                if (efObject != null)
                {
                    fechas.Add(new EG.BitacoraDetailDTe()
                    {
                        FechaEvento = efObject.FechaCreacion,
                        Descripcion = string.Format("Registrado en el sistema por el usuario {0}", efObject.UsuarioCreacion),
                        Id = 3
                    });

                    fechas.Add(new EG.BitacoraDetailDTe()
                    {
                        FechaEvento = (efObject.FechaRecepcion.HasValue) ? efObject.FechaRecepcion.Value : DateTime.MinValue,
                        Descripcion = "Fecha de solicitud",
                        Id = 1
                    });

                    fechas.Add(new EG.BitacoraDetailDTe()
                    {
                        FechaEvento = (efObject.FechaRecepcionARFFS.HasValue) ? efObject.FechaRecepcionARFFS.Value : DateTime.MinValue,
                        Descripcion = "Fecha de recepción en la ARFFS",
                        Id = 2

                    });
                }

            }

            return fechas;
        }

        public static bool SetNumeroCertificado(int id, string codigoGenerado)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionSet.Find(id);

                    if (efObject != null)
                    {
                        efObject.NumeroCertificado = codigoGenerado;
                        efObject.Estado = "R";
                        dbContext.SaveChanges();
                    }
                    else
                    { exito = false; }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;

        }

        public static bool SetEstado(int id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionSet.Find(id);

                    if (efObject != null)
                    {
                        efObject.Estado = "R";
                        dbContext.SaveChanges();
                    }
                    else
                    { exito = false; }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;

        }

        #endregion

        #region Metodos del(los) Bloques

        public static decimal InsertBloque(BloqueTableRowDTe bloque)
        {
            var detalleEF = new PlantacionDetalle()
            {
                Nombre = bloque.Nombre,
                Plantacion_Id = bloque.Plantacion_Id,
                SistemaPlantacion_Id = bloque.SistemaPlantacion_Id,
                //NombreComun = bloque.EspecieNombreComun,
                AreaPredio = bloque.AreaPredio,
                AnnioEstablecimiento = bloque.AnnioEstablecimiento,
                MesEstablecimiento = bloque.MesEstablecimiento,
                //AlturaPromedio = bloque.AlturaPromedio,
                CantidadSuperficieHa = bloque.CantidadSuperficieHa,
                CantidadSuperficieMl = bloque.CantidadSuperficieMl,
                SistemaCoordenada_Id = bloque.SistemaCoordenada_Id,
                CoordenadaEsteUTM = bloque.CoordenadaEsteUTM,
                CoordenadaNorteUTM = bloque.CoordenadaNorteUTM,
                //ProduccionEstimada = bloque.ProduccionEstimada,                
                //TotalPlantado = bloque.TotalPlantado,
                UsuarioCreacion = bloque.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = bloque.UsuarioCreacion,
                FechaModificacion = DateTime.Now
            };

            //if (bloque.UnidadMedida_Id > 0) detalleEF.UnidadMedida_Id = bloque.UnidadMedida_Id;
            //if (bloque.Especie_Id > 0) detalleEF.Especie_Id = bloque.Especie_Id;
            if (bloque.Finalidad_Id > 0) detalleEF.Finalidad_Id = bloque.Finalidad_Id;
            //if (bloque.TipoPlantado != null) detalleEF.TipoPlantado = bloque.TipoPlantado.ToString();

            decimal id = 0;
            using (var dbContext = new PlantacionSchema())
            {
                var plantacion = dbContext.PlantacionSet.Find(bloque.Plantacion_Id);
                IList<PlantacionDetalle> lista =  dbContext.PlantacionDetalle.Where(pla => pla.Plantacion_Id == bloque.Plantacion_Id).ToList();
                if(lista!= null)
                {
                    decimal areaDetalle = 0;
                    foreach(PlantacionDetalle p in lista)
                    {
                        areaDetalle += (p.CantidadSuperficieHa.HasValue? p.CantidadSuperficieHa.Value:0);
                    }

                    if(areaDetalle > plantacion.Area)
                    {
                        throw new Exception("No se pudo registrar el bloque o predio. Por favor corrija los datos ingresados e intente nuevamente.");
                    }
                }

                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.PlantacionDetalle.Add(detalleEF);

                        
                        var totalHa = (plantacion.CantidadTotalSuperficieHa == null) ? bloque.CantidadSuperficieHa : bloque.CantidadSuperficieHa + plantacion.CantidadTotalSuperficieHa;
                        var totalMl = (plantacion.CantidadTotalSuperficieMl == null) ? bloque.CantidadSuperficieMl : bloque.CantidadSuperficieMl + plantacion.CantidadTotalSuperficieMl;

                        plantacion.CantidadTotalSuperficieHa = totalHa;
                        plantacion.CantidadTotalSuperficieMl = totalMl;
                        plantacion.EstadoGeo = "REGISTERED";

                        dbContext.SaveChanges();

                        id = detalleEF.Id;

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }

            }

            return id;
        }

        public static decimal InsertEspecie(EspecieItemListDTe especie)
        {
            var detalleEF = new PlantacionDetalleEspecie()
            {
                PlantacionDetalle_Id = especie.PlantacionDetalle_Id,
                NombreComun = especie.NombreComun,
                AlturaPromedio = especie.AlturaPromedio,
                ProduccionEstimada = especie.ProduccionEstimada,
                TotalPlantado = especie.TotalPlantado,
                UsuarioCreacion = especie.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = especie.UsuarioCreacion,
                FechaModificacion = DateTime.Now
            };

            if (especie.UnidadMedida_Id > 0) detalleEF.UnidadMedida_Id = especie.UnidadMedida_Id;
            if (especie.Especie_Id > 0) detalleEF.Especie_Id = especie.Especie_Id;
            if (especie.TipoPlantado != null) detalleEF.TipoPlantado = especie.TipoPlantado.ToString();

            decimal id = 0;
            using (var dbContext = new PlantacionSchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.PlantacionDetalleEspecie.Add(detalleEF);

                        dbContext.SaveChanges();

                        id = detalleEF.Id;

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }

            }

            return id;
        }

        public static decimal UpdateBloque(BloqueTableRowDTe bloque)
        {
            decimal blid = 0;
            using (var dbContext = new PlantacionSchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var efObject = dbContext.PlantacionDetalle.Find(bloque.Id);
                        efObject.Nombre = bloque.Nombre;
                        efObject.Plantacion_Id = bloque.Plantacion_Id;
                        efObject.SistemaPlantacion_Id = bloque.SistemaPlantacion_Id;
                        //efObject.UnidadMedida_Id = bloque.UnidadMedida_Id;
                        //efObject.NombreComun = bloque.EspecieNombreComun;
                        efObject.AreaPredio = bloque.AreaPredio;
                        efObject.AnnioEstablecimiento = bloque.AnnioEstablecimiento;
                        efObject.MesEstablecimiento = bloque.MesEstablecimiento;
                        //efObject.AlturaPromedio = bloque.AlturaPromedio;
                        efObject.CantidadSuperficieHa = bloque.CantidadSuperficieHa;
                        efObject.CantidadSuperficieMl = bloque.CantidadSuperficieMl;
                        efObject.CoordenadaEsteUTM = bloque.CoordenadaEsteUTM;
                        efObject.CoordenadaNorteUTM = bloque.CoordenadaNorteUTM;
                        efObject.SistemaCoordenada_Id = bloque.SistemaCoordenada_Id;
                        //efObject.ProduccionEstimada = bloque.ProduccionEstimada;                        
                        //efObject.TotalPlantado = bloque.TotalPlantado;
                        efObject.UsuarioModificacion = bloque.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        //if (bloque.Especie_Id > 0) efObject.Especie_Id = bloque.Especie_Id;
                        if (bloque.Finalidad_Id > 0) efObject.Finalidad_Id = bloque.Finalidad_Id;
                        //if (bloque.TipoPlantado!=null) efObject.TipoPlantado = bloque.TipoPlantado.ToString();


                        dbContext.SaveChanges();

                        var plantacion = dbContext.PlantacionSet.Find(bloque.Plantacion_Id);
                        var totalHa = (plantacion.CantidadTotalSuperficieHa == null) ? bloque.CantidadSuperficieHa : bloque.CantidadSuperficieHa + plantacion.CantidadTotalSuperficieHa;
                        var totalMl = (plantacion.CantidadTotalSuperficieMl == null) ? bloque.CantidadSuperficieMl : bloque.CantidadSuperficieMl + plantacion.CantidadTotalSuperficieMl;

                        plantacion.CantidadTotalSuperficieHa = totalHa;
                        plantacion.CantidadTotalSuperficieMl = totalMl;
                        plantacion.EstadoGeo = "REGISTERED";


                        dbContext.SaveChanges();

                        trans.Commit();
                        blid = bloque.Id;
                    }
                    catch
                    {
                        blid = 0;
                        trans.Rollback();
                    }
                }

            }

            return blid;
        }

        public static decimal UpdateEspecie(EspecieItemListDTe especie)
        {
            decimal espid = 0;
            using (var dbContext = new PlantacionSchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var efObject = dbContext.PlantacionDetalleEspecie.Find(especie.Id);
                        efObject.AlturaPromedio = especie.AlturaPromedio;
                        efObject.Especie_Id = especie.Especie_Id;
                        efObject.Id = especie.Id;
                        efObject.NombreComun = especie.NombreComun;
                        efObject.PlantacionDetalle_Id = especie.PlantacionDetalle_Id;
                        efObject.ProduccionEstimada = especie.ProduccionEstimada;
                        efObject.TipoPlantado = especie.TipoPlantado;
                        efObject.TotalPlantado = especie.TotalPlantado;
                        efObject.UnidadMedida_Id = especie.UnidadMedida_Id;
                        efObject.PlantacionDetalle_Id = especie.PlantacionDetalle_Id;
                        efObject.UsuarioModificacion = especie.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;


                        dbContext.SaveChanges();

                        trans.Commit();
                        espid = especie.Id;
                    }
                    catch
                    {
                        espid = 0;
                        trans.Rollback();
                    }
                }

            }

            return espid;
        }

        public static object DeleteBloqueById(int id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var efObject = dbContext.PlantacionDetalle.Find(id);

                        if (efObject != null)
                        {
                            dbContext.PlantacionDetalle.Remove(efObject);

                            var plantacion = dbContext.PlantacionSet.Find(efObject.Plantacion_Id);
                            plantacion.CantidadTotalSuperficieHa -= efObject.CantidadSuperficieHa;
                            plantacion.CantidadTotalSuperficieMl -= efObject.CantidadSuperficieMl;

                            if (plantacion.EstadoGeo == "FAIL")
                            {
                                plantacion.EstadoGeo = "REGISTERED";
                            }

                            dbContext.SaveChanges();

                        }
                        trans.Commit();
                    }
                    catch
                    {
                        exito = false;
                        trans.Rollback();
                    }
                }

            }
            return exito;
        }

        public static object DeleteEspecieById(int id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var efObject = dbContext.PlantacionDetalleEspecie.Find(id);

                        if (efObject != null)
                        {
                            dbContext.PlantacionDetalleEspecie.Remove(efObject);

                            /* var plantacion = dbContext.PlantacionSet.Find(efObject.Plantacion_Id);
                            plantacion.CantidadTotalSuperficieHa -= efObject.CantidadSuperficieHa;
                            plantacion.CantidadTotalSuperficieMl -= efObject.CantidadSuperficieMl;
                            */
                            dbContext.SaveChanges();

                        }
                        trans.Commit();
                    }
                    catch
                    {
                        exito = false;
                        trans.Rollback();
                    }
                }

            }
            return exito;
        }

        public static BloqueTableRowDTe GetBloqueById(int id)
        {
            BloqueTableRowDTe bloque = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PlantacionDetalle.Find(id);

                if (efObject != null)
                {
                    try
                    {
                        bloque = new BloqueTableRowDTe()
                        {
                            Id = efObject.Id,
                            Nombre = efObject.Nombre,
                            AnnioEstablecimiento = efObject.AnnioEstablecimiento,
                            MesEstablecimiento = efObject.MesEstablecimiento,
                            AreaPredio = efObject.AreaPredio,
                            SistemaCoordenada_Id = efObject.SistemaCoordenada_Id,
                            CoordenadaEsteUTM = efObject.CoordenadaEsteUTM,
                            CoordenadaNorteUTM = efObject.CoordenadaNorteUTM,
                            SistemaPlantacion_Id = efObject.SistemaPlantacion_Id,
                            SistemaPlantacionDescripcion = efObject.SistemaPlantacion.Descripcion,
                            Plantacion_Id = efObject.Plantacion_Id,
                            CantidadCoordenadasRegistradas = efObject.PlantacionDetalleVertices.Count,
                            CantidadSuperficieHa = efObject.CantidadSuperficieHa,
                            CantidadSuperficieMl = efObject.CantidadSuperficieMl,
                            FechaCreacion = efObject.FechaCreacion.Value,
                            UsuarioCreacion = efObject.UsuarioCreacion
                        };
                    }
                    catch (Exception e)
                    {

                    }



                    if (!string.IsNullOrEmpty(efObject.UriStorageFileName))
                        bloque.UriStorageFileName = efObject.UriStorageFileName;

                    if (efObject.Finalidad_Id != null && efObject.Finalidad_Id > 0)
                    {
                        bloque.Finalidad_Id = efObject.Finalidad_Id;
                        bloque.FinalidadDescripcion = efObject.FinalidadProducto.Descripcion;
                    }

                    if (efObject.PlantacionDetalleEspecie.Count() > 0)
                    {
                        bloque.Especies = new List<EspecieItemListDTe>();
                        foreach (var especie in efObject.PlantacionDetalleEspecie)
                        {
                            var newEspecie = new EspecieItemListDTe()
                            {
                                Id = especie.Id,
                                PlantacionDetalle_Id = Convert.ToInt16(especie.PlantacionDetalle_Id),
                                ProduccionEstimada = especie.ProduccionEstimada,
                                TotalPlantado = especie.TotalPlantado,
                                TipoPlantado = especie.TipoPlantado,
                                UnidadMedida_Id = especie.UnidadMedida_Id,
                                UnidadMedidaDescripcion = especie.UnidadMedida.Descripcion,
                                UnidadMedidaSimbolo = especie.UnidadMedida.Simbolo
                            };
                            if (especie.TipoPlantado != null) newEspecie.TipoPlantadoDesc = getTipoPlantado(especie.TipoPlantado);
                            if (especie.Especie != null && especie.Especie_Id > 0)
                            {

                                newEspecie.EspecieCodigo = especie.Especie.Codigo;
                                newEspecie.EspecieNombre = especie.Especie.Nombre;
                                newEspecie.NombreComun = especie.NombreComun;
                                newEspecie.Especie_Id = especie.Especie_Id;
                            }
                            //newEspecie.bloque = bloque;                            
                            bloque.Especies.Add(newEspecie);
                        }
                    }

                }
            }

            return bloque;
        }

        public static EspecieItemListDTe GetEspecieById(int id)
        {
            EspecieItemListDTe especie = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PlantacionDetalleEspecie.Find(id);

                if (efObject != null)
                {
                    especie = new EspecieItemListDTe()
                    {
                        Id = efObject.Id,
                        Especie_Id = efObject.Especie_Id,
                        EspecieNombre = efObject.Especie.Nombre,
                        NombreComun = efObject.NombreComun,
                        TotalPlantado = efObject.TotalPlantado,
                        TipoPlantado = efObject.TipoPlantado,
                        TipoPlantadoDesc = efObject.TipoPlantacion.Descripcion,
                        AlturaPromedio = efObject.AlturaPromedio,
                        ProduccionEstimada = efObject.ProduccionEstimada,
                        UnidadMedida_Id = efObject.UnidadMedida_Id,
                        UnidadMedidaDescripcion = efObject.UnidadMedida.Descripcion,
                        UnidadMedidaSimbolo = efObject.UnidadMedida.Simbolo,
                        PlantacionDetalle_Id = Convert.ToInt32(efObject.PlantacionDetalle_Id),
                        FechaCreacion = efObject.FechaCreacion.Value,
                        UsuarioCreacion = efObject.UsuarioCreacion
                    };

                    //if (!string.IsNullOrEmpty(efObject.TipoPlantado))
                    //    bloque.TipoPlantado = Convert.ToChar(efObject.TipoPlantado);

                    /*if (!string.IsNullOrEmpty(efObject.UriStorageFileName))
                        bloque.UriStorageFileName = efObject.UriStorageFileName;

                    if (efObject.Finalidad_Id != null && efObject.Finalidad_Id > 0)
                    {
                        bloque.Finalidad_Id = efObject.Finalidad_Id;
                        bloque.FinalidadDescripcion = efObject.FinalidadProducto.Descripcion;
                    }*/

                    /*if (efObject.EspecieForestal != null && efObject.Especie_Id > 0)
                    {
                        bloque.EspecieNombreCientifico = efObject.EspecieForestal.NombreCientifico;
                        bloque.EspecieNombreComun = efObject.NombreComun;
                        bloque.Especie_Id = efObject.Especie_Id;
                    }*/

                }
            }

            return especie;
        }

        public static IEnumerable<BloqueTableRowDTe> GetBloquesByPlantacionId(int id)
        {
             
            var detalles = new List<BloqueTableRowDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var efObjects = dbContext.PlantacionDetalle.Where(b => b.Plantacion_Id == id);

                if (efObjects.Count() > 0)
                {
                    foreach (var bloque in efObjects)
                    {
                        var newBloque = new BloqueTableRowDTe()
                        {
                            Id = bloque.Id,
                            Nombre = bloque.Nombre,
                            AnnioEstablecimiento = bloque.AnnioEstablecimiento,
                            MesEstablecimiento = bloque.MesEstablecimiento,
                            AreaPredio = bloque.AreaPredio,
                            SistemaCoordenada_Id = bloque.SistemaCoordenada_Id,
                            CoordenadaEsteUTM = bloque.CoordenadaEsteUTM,
                            CoordenadaNorteUTM = bloque.CoordenadaNorteUTM,
                            Finalidad_Id = bloque.Finalidad_Id,
                            FinalidadDescripcion = bloque.FinalidadProducto.Descripcion,
                            SistemaPlantacion_Id = bloque.SistemaPlantacion_Id,
                            SistemaPlantacionDescripcion = bloque.SistemaPlantacion.Descripcion,
                            Plantacion_Id = bloque.Plantacion_Id,
                            CantidadCoordenadasRegistradas = bloque.PlantacionDetalleVertices.Count,
                            CantidadSuperficieHa = bloque.CantidadSuperficieHa,
                            CantidadSuperficieMl = bloque.CantidadSuperficieMl,
                            UriStorageFileName = bloque.UriStorageFileName
                        };
                        if (bloque.MesEstablecimiento != null)
                        {
                            newBloque.MesDescripcion = getMesDescripcion(bloque.MesEstablecimiento);
                        }
                        newBloque.Especies = new List<EspecieItemListDTe>();
                        if (bloque.PlantacionDetalleEspecie.Count() > 0)
                        {
                            foreach (var especie in bloque.PlantacionDetalleEspecie)
                            {
                                var newEspecie = new EspecieItemListDTe()
                                {
                                    Id = especie.Id,
                                    PlantacionDetalle_Id = Convert.ToInt16(especie.PlantacionDetalle_Id),
                                    ProduccionEstimada = especie.ProduccionEstimada,
                                    TotalPlantado = especie.TotalPlantado,
                                    TipoPlantado = especie.TipoPlantado,
                                    UnidadMedida_Id = especie.UnidadMedida_Id,
                                    UnidadMedidaDescripcion = especie.UnidadMedida.Descripcion,
                                    UnidadMedidaSimbolo = especie.UnidadMedida.Simbolo,

                                };
                                if (especie.TipoPlantado != null) newEspecie.TipoPlantadoDesc = getTipoPlantado(especie.TipoPlantado);
                                if (especie.Especie != null && especie.Especie_Id > 0)
                                {
                                    newEspecie.EspecieCodigo = especie.Especie.Codigo;
                                    newEspecie.EspecieNombre = especie.Especie.Nombre;
                                    newEspecie.NombreComun = especie.NombreComun;
                                    newEspecie.Especie_Id = especie.Especie_Id;
                                }
                                newEspecie.bloque = newBloque;
                                newBloque.Especies.Add(newEspecie);
                            }
                        }
                        detalles.Add(newBloque);
                    }

                }
            }

            return detalles;
        }

        public static bool SetBloqueUriFile(int bloqueId, string uri)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionDetalle.Find(bloqueId);

                    if (efObject != null)
                    {
                        efObject.UriStorageFileName = uri;
                        dbContext.SaveChanges();
                    }
                    else
                    { exito = false; }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;

        }

        public static bool UnsetBloqueUriFile(int bloqueId)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionDetalle.Find(bloqueId);

                    if (efObject != null)
                    {
                        efObject.UriStorageFileName = string.Empty;
                        dbContext.SaveChanges();
                    }
                    else
                    { exito = false; }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;

        }

        #endregion

        #region Metodos de los vértices o coordenadas de los bloques

        public static CoordenadaTableRowDTe GetCoordenadaPredio(int plantacionId)
        {
            var coordenada = new CoordenadaTableRowDTe();
            using (var dbContext = new PlantacionSchema())
            {
                var efObjects = dbContext.PlantacionSet.Where(c => c.Id == plantacionId).FirstOrDefault();
                if (efObjects != null)
                {
                    int zona = 0;
                    var converter = new GeoUTMConverter();

                    zona = Convert.ToInt32(efObjects.SistemaCoordenada_Id);
                    coordenada = new CoordenadaTableRowDTe()
                    {
                        Id = efObjects.Id,
                        CoordenadaEsteUTM = efObjects.CoordenadaEsteUTM,
                        CoordenadaNorteUTM = efObjects.CoordenadaNorteUTM,
                        UsuarioCreacion = efObjects.UsuarioCreacion,
                        UsuarioModificacion = efObjects.UsuarioModificacion,
                    };

                    if (!string.IsNullOrEmpty(efObjects.CoordenadaEsteUTM) && !string.IsNullOrEmpty(efObjects.CoordenadaNorteUTM))
                    {
                        converter.ToLatLon(Convert.ToDouble(efObjects.CoordenadaEsteUTM), Convert.ToDouble(efObjects.CoordenadaNorteUTM), zona, GeoUTMConverter.Hemisphere.Southern);
                        //var LatLng = converter.convertUtmToLatLng(Convert.ToDouble(vertice.CoordenadaEsteUTM), Convert.ToDouble(vertice.CoordenadaNorteUTM), zona, "S");
                        coordenada.CoordenadaGeografica = new EG.CoordinateDetailDTe()
                        {
                            Nombre = efObjects.NombrePredio,
                            NombreLocation = "Centroide",
                            Latitud = converter.Latitude,//LatLng.Lat,
                            Longitud = converter.Longitude// LatLng.Lng
                        };
                    }


                }
            }
            return coordenada;
        }

        public static CapaArcGIS GetCapaArcGISPlantacionesBySede(int idSede)
        {
            var capa = new CapaArcGIS();
            using (var dbContext = new PlantacionSchema())
            {
                var efBloques = dbContext.PlantacionDetalle.Where(p => p.Plantacion.SedePrincipal_Id == idSede && p.Plantacion.Estado == "R");
                if (efBloques.Count() > 0)
                {
                    capa.displayFieldName = "PLANID";
                    //ALIAS
                    capa.fieldAliases = new AliasCampo();
                    capa.fieldAliases.OBJECTID = "OBJECTID";
                    capa.fieldAliases.PLANID = "Plantacion ID";
                    capa.fieldAliases.NUMERO = "Numero de Certificado";
                    capa.fieldAliases.NOMPRE = "Nombre del Predio";
                    capa.fieldAliases.DEPART = "Departamento";
                    capa.fieldAliases.PROVIN = "Provincia";
                    capa.fieldAliases.DISTRI = "Distrito";
                    capa.fieldAliases.NOMZON = "Nombre de Zona";
                    capa.fieldAliases.TITULA = "Titulares";
                    capa.fieldAliases.PROPIE = "Propietarios";

                    //tipo de geometría
                    capa.geometryType = "esriGeometryPolygon";
                    capa.spatialReference = new ReferenciaEspacial();
                    capa.spatialReference.wkid = 102100;
                    capa.spatialReference.latestWkid = 3857;
                    //CAMPOS
                    capa.fields = new List<Campo>();
                    capa.fields.Add(new Campo() { name = "OBJECTID", type = "esriFieldTypeOID", alias = "OBJECTID" });
                    capa.fields.Add(new Campo() { name = "PLANID", type = "esriFieldTypeSmallInteger", alias = "Plantacion ID" });
                    capa.fields.Add(new Campo() { name = "NUMERO", type = "esriFieldTypeString", alias = "Numero de Certificado", length = 60 });
                    capa.fields.Add(new Campo() { name = "NOMPRE", type = "esriFieldTypeString", alias = "Nombre del Predio", length = 100 });
                    capa.fields.Add(new Campo() { name = "DEPART", type = "esriFieldTypeString", alias = "Departamento", length = 150 });
                    capa.fields.Add(new Campo() { name = "PROVIN", type = "esriFieldTypeString", alias = "Provincia", length = 150 });
                    capa.fields.Add(new Campo() { name = "DISTRI", type = "esriFieldTypeString", alias = "Distrito", length = 150 });
                    capa.fields.Add(new Campo() { name = "NOMZON", type = "esriFieldTypeString", alias = "Nombre de Zona", length = 150 });
                    capa.fields.Add(new Campo() { name = "TITULA", type = "esriFieldTypeString", alias = "Titulares", length = 250 });
                    capa.fields.Add(new Campo() { name = "PROPIE", type = "esriFieldTypeString", alias = "Propietarios", length = 250 });

                    Caracteristica registro = null;
                    //Anillo anillo = null;

                    foreach (var bloque in efBloques)
                    {
                        registro = new Caracteristica();
                        registro.attributes.OBJECTID = (int)bloque.Id;
                        registro.attributes.PLANID = bloque.Plantacion_Id;
                        registro.attributes.NUMERO = bloque.Plantacion.NumeroCertificado;
                        registro.attributes.NOMPRE = bloque.Plantacion.NombrePredio;
                        registro.attributes.DEPART = bloque.Plantacion.Ubigeo.NombreDepartamento;
                        registro.attributes.PROVIN = bloque.Plantacion.Ubigeo.NombreProvincia;
                        registro.attributes.DISTRI = bloque.Plantacion.Ubigeo.NombreDistrito;
                        registro.attributes.NOMZON = bloque.Plantacion.NombreZona;
                        //var personas = bloque.Plantacion.PersonaPlantacion;
                        string titulares = "";
                        string propietarios = "";
                        foreach (var persona in bloque.Plantacion.PersonaPlantacion)
                        {
                            if (persona.Rol == "T")
                            {
                                if (titulares != "") titulares += ", ";
                                if (persona.Persona.EsJuridica)
                                {
                                    titulares += persona.Persona.Nombres;
                                }
                                else
                                {
                                    titulares += persona.Persona.Nombres + " " + persona.Persona.ApellidoPaterno + " " + persona.Persona.ApellidoMaterno;
                                }
                            }
                            if (persona.Rol == "P")
                            {
                                if (propietarios != "") propietarios += ", ";
                                if (persona.Persona.EsJuridica)
                                {
                                    propietarios += persona.Persona.Nombres;
                                }
                                else
                                {
                                    propietarios += persona.Persona.Nombres + " " + persona.Persona.ApellidoPaterno + " " + persona.Persona.ApellidoMaterno;
                                }
                            }
                        }
                        registro.attributes.TITULA = titulares;
                        registro.attributes.PROPIE = propietarios;

                        string zonaChar = bloque.SistemaCoordenada_Id.ToString();
                        int zona = (!string.IsNullOrEmpty(zonaChar)) ? Convert.ToInt32(zonaChar) : 17;
                        var converter = new GeoUTMConverter();

                        double[][] poligono = new double[bloque.PlantacionDetalleVertices.Count][];

                        int num = 0;
                        foreach (var vertice in bloque.PlantacionDetalleVertices)
                        {

                            if (!string.IsNullOrEmpty(vertice.CoordenadaEsteUTM) && !string.IsNullOrEmpty(vertice.CoordenadaNorteUTM))
                            {
                                poligono[num] = new double[2];

                                converter.ToLatLon(Convert.ToDouble(vertice.CoordenadaEsteUTM), Convert.ToDouble(vertice.CoordenadaNorteUTM), zona, GeoUTMConverter.Hemisphere.Southern);
                                poligono[num][0] = converter.Latitude;
                                poligono[num][1] = converter.Longitude;
                                num++;
                            }
                        }
                        registro.geometry.rings.Add(poligono);
                        capa.features.Add(registro);
                    }
                }
            }
            return capa;
        }

        public static IEnumerable<BloqueCoordenadasDTE> GetCoordenadasByPlantacionId(int id)
        {
            //var name = UserInfo.GetUserName();    TYLER 17.12.19

            var bloques = new List<BloqueCoordenadasDTE>();

            var reduccion = 0; //TYLER 17.12.19
            var reducir = 0; //TYLER 17.12.19

            using (var dbContext = new PlantacionSchema())
            {

                var efBloques = dbContext.PlantacionDetalle.Where(b => b.Plantacion_Id == id);

                if (efBloques.Count() > 0)
                {
                    //int zona = Convert.ToInt32(dbContext.PlantacionDetalle.SistemaCoordenada_Id);
                    string zonaChar = dbContext.PlantacionSet.FirstOrDefault(p => p.Id == id).PlantacionDetalle.ToArray()[0].SistemaCoordenada_Id.ToString();

                    int zona = (!string.IsNullOrEmpty(zonaChar)) ? Convert.ToInt32(zonaChar) : 17;

                    var converter = new GeoUTMConverter();

                    foreach (var bloque in efBloques)
                    {

                        var newBloque = new BloqueCoordenadasDTE()
                        {
                            Id = bloque.Id,
                            Zona = zona, //TYLER 17.12.19
                            Nombre = bloque.Nombre,
                            AnnioEstablecimiento = bloque.AnnioEstablecimiento,
                            MesEstablecimiento = bloque.MesEstablecimiento,
                            AreaPredio = bloque.AreaPredio,
                            CoordenadaEsteUTM = bloque.CoordenadaEsteUTM,
                            CoordenadaNorteUTM = bloque.CoordenadaNorteUTM,
                            SistemaPlantacion_Id = bloque.SistemaPlantacion_Id,
                            SistemaPlantacionDescripcion = bloque.SistemaPlantacion.Descripcion,
                            Plantacion_Id = bloque.Plantacion_Id,
                            CantidadCoordenadasRegistradas = bloque.PlantacionDetalleVertices.Count,
                            CantidadSuperficieHa = bloque.CantidadSuperficieHa,
                            CantidadSuperficieMl = bloque.CantidadSuperficieMl,
                            FechaCreacion = bloque.FechaCreacion.Value,
                            UsuarioCreacion = bloque.UsuarioCreacion
                        };

                        if (!string.IsNullOrEmpty(bloque.UriStorageFileName))
                            newBloque.UriStorageFileName = bloque.UriStorageFileName;

                        if (bloque.Finalidad_Id != null && bloque.Finalidad_Id > 0)
                        {
                            newBloque.Finalidad_Id = bloque.Finalidad_Id;
                            newBloque.FinalidadDescripcion = bloque.FinalidadProducto.Descripcion;
                        }

                        if (bloque.PlantacionDetalleEspecie.Count() > 0)
                        {
                            foreach (var especie in bloque.PlantacionDetalleEspecie)
                            {
                                var newEspecie = new EspecieItemListDTe()
                                {
                                    Id = especie.Id,
                                    ProduccionEstimada = especie.ProduccionEstimada,
                                    TotalPlantado = especie.TotalPlantado,
                                    UnidadMedida_Id = especie.UnidadMedida_Id,
                                    UnidadMedidaDescripcion = especie.UnidadMedida.Descripcion,
                                    UnidadMedidaSimbolo = especie.UnidadMedida.Simbolo
                                };
                                if (especie.Especie != null && especie.Especie_Id > 0)
                                {
                                    newEspecie.EspecieCodigo = especie.Especie.Codigo;
                                    newEspecie.EspecieNombre = especie.Especie.Nombre;
                                    newEspecie.Especie_Id = especie.Especie_Id;
                                }
                                newBloque.Especies = new List<EspecieItemListDTe>();
                                newBloque.Especies.Add(newEspecie);
                            }
                        }

                        newBloque.Coordenadas = new List<CoordenadaTableRowDTe>();

                        foreach (var vertice in bloque.PlantacionDetalleVertices)
                        {

                            var newVertice = new CoordenadaTableRowDTe()
                            {
                                Id = vertice.Id,
                                CoordenadaEsteUTM = (Convert.ToDecimal(vertice.CoordenadaEsteUTM) - reducir).ToString(), //TYLER 17.12.19
                                CoordenadaNorteUTM = (Convert.ToDecimal(vertice.CoordenadaNorteUTM) - reducir).ToString(), //TYLER 17.12.19
                                NumeroSecuencia = vertice.NumeroSecuencia,
                                UsuarioCreacion = vertice.UsuarioCreacion,
                                FechaCreacion = vertice.FechaCreacion.Value,
                                UsuarioModificacion = vertice.UsuarioModificacion,
                                FechaModificacion = vertice.FechaModificacion.Value
                            };

                            if (!string.IsNullOrEmpty(vertice.CoordenadaEsteUTM) && !string.IsNullOrEmpty(vertice.CoordenadaNorteUTM))
                            {
                                converter.ToLatLon(Convert.ToDouble(vertice.CoordenadaEsteUTM) - reducir, Convert.ToDouble(vertice.CoordenadaNorteUTM) - reducir, zona, GeoUTMConverter.Hemisphere.Southern); //TYLER 17.12.19
                                newVertice.CoordenadaGeografica = new EG.CoordinateDetailDTe()
                                {
                                    Nombre = vertice.PlantacionDetalle.Nombre,
                                    NombreLocation = "Punto " + vertice.NumeroSecuencia.ToString(),
                                    Latitud = converter.Latitude - reduccion, //TYLER 17.12.19
                                    Longitud = converter.Longitude - reduccion //TYLER 17.12.19
                                };
                            }

                            newBloque.Coordenadas.Add(newVertice);
                        }

                        bloques.Add(newBloque);

                    }
                }
            }

            return bloques;
        }

        public static IEnumerable<CoordenadaTableRowDTe> GetCoordenadasByBloqueId(int id)
        {

            var coordenadas = new List<CoordenadaTableRowDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var efObjects = dbContext.PlantacionDetalleVertices.Where(c => c.PlantacionDetalle_Id == id);

                if (efObjects.Count() > 0)
                {
                    int zona = 0;
                    var converter = new GeoUTMConverter();
                    //var converter = new LatLngUTMConverter("WGS 84");
                    foreach (var vertice in efObjects)
                    {
                        //zona = (!string.IsNullOrEmpty(vertice.PlantacionDetalle.SistemaCoordenada_Id)) ? Convert.ToInt32(vertice.PlantacionDetalle.SistemaCoordenada_Id) : 0;
                        zona = Convert.ToInt32(vertice.PlantacionDetalle.SistemaCoordenada_Id);
                        var newVertice = new CoordenadaTableRowDTe()
                        {
                            Id = vertice.Id,
                            CoordenadaEsteUTM = vertice.CoordenadaEsteUTM,
                            CoordenadaNorteUTM = vertice.CoordenadaNorteUTM,
                            NumeroSecuencia = vertice.NumeroSecuencia,
                            UsuarioCreacion = vertice.UsuarioCreacion,
                            FechaCreacion = vertice.FechaCreacion.Value,
                            UsuarioModificacion = vertice.UsuarioModificacion,
                            FechaModificacion = vertice.FechaModificacion.Value
                        };

                        if (!string.IsNullOrEmpty(vertice.CoordenadaEsteUTM) && !string.IsNullOrEmpty(vertice.CoordenadaNorteUTM))
                        {
                            converter.ToLatLon(Convert.ToDouble(vertice.CoordenadaEsteUTM), Convert.ToDouble(vertice.CoordenadaNorteUTM), zona, GeoUTMConverter.Hemisphere.Southern);
                            //var LatLng = converter.convertUtmToLatLng(Convert.ToDouble(vertice.CoordenadaEsteUTM), Convert.ToDouble(vertice.CoordenadaNorteUTM), zona, "S");
                            newVertice.CoordenadaGeografica = new EG.CoordinateDetailDTe()
                            {
                                Nombre = vertice.PlantacionDetalle.Nombre,
                                NombreLocation = "Punto " + vertice.NumeroSecuencia.ToString(),
                                Latitud = converter.Latitude,//LatLng.Lat,
                                Longitud = converter.Longitude// LatLng.Lng
                            };
                        }

                        coordenadas.Add(newVertice);
                    }
                }
            }

            return coordenadas;
        }

        public static decimal InsertCoordenada(CoordenadaTableRowDTe vertice)
        {
            var verticeEF = new PlantacionDetalleVertices()
            {
                NumeroSecuencia = vertice.NumeroSecuencia,
                PlantacionDetalle_Id = vertice.Bloque_Id,
                CoordenadaEsteUTM = vertice.CoordenadaEsteUTM,
                CoordenadaNorteUTM = vertice.CoordenadaNorteUTM,
                UsuarioCreacion = vertice.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = vertice.UsuarioCreacion,
                FechaModificacion = DateTime.Now
            };


            decimal id = 0;
            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    dbContext.PlantacionDetalleVertices.Add(verticeEF);
                    var bloque = dbContext.PlantacionDetalle.Find(vertice.Bloque_Id);
                    var plantacion = dbContext.PlantacionSet.Find(bloque.Plantacion_Id);
                    plantacion.EstadoGeo = "REGISTERED";
                    dbContext.SaveChanges();
                    id = verticeEF.Id;
                }
                catch
                {

                }
            }

            return id;
        }

        public static bool DeleteCoordenadaById(int id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PlantacionDetalleVertices.Find(id);

                    if (efObject != null)
                    {
                        dbContext.PlantacionDetalleVertices.Remove(efObject);
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

        #endregion

    }
}
