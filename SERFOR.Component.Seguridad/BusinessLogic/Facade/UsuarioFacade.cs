using EG = SERFOR.Component.DTEntities.Seguridad;
using SERFOR.Component.SeguridadCore.DataAccess;
using SERFOR.Component.DTEntities.Seguridad;
using System.Collections.Generic;
using System.Linq;
using System;
using SERFOR.Component.DTEntities.General;

namespace SERFOR.Component.SeguridadCore.BusinessLogic.Facade
{
    public static class UsuarioFacade
    {

        public static IEnumerable<UsuarioLoginDTe> GetAll()
        {
            IEnumerable<UsuarioLoginDTe> usuarios;

            var dbContext = new SecuritySchema();

            usuarios = dbContext.Usuario.Select(p => new UsuarioLoginDTe()
            {
                Id = p.Usuario_Id,
                NombreUsuario = p.Nombre,
                Password = p.Clave,
                NombreCompleto = (p.Persona.EsJuridica) ? p.Persona.Nombres : p.Persona.ApellidoPaterno + " " + p.Persona.ApellidoMaterno + ", " + p.Persona.Nombres,
                FechaInicio = p.FechaInicio,
                FechaFin= p.FechaFin,
                Sede = p.SedeAutoridadForestal.Nombre,
                SedePrincipal = p.SedeAutoridadForestal1.Nombre,
                EsSedePrincipal = p.EsSedePrincipal
            });

            dbContext.Usuario.AsNoTracking();

            return usuarios;
        }
        public static IEnumerable<PersonaItemListDTe> GetTrabajadores()
        {

            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new SecuritySchema();


            query = dbContext.Persona.Where(p => p.EsTrabajador == true && p.Activo);


            personas = query.Select(p => new PersonaItemListDTe()
            {
                Id = p.Id,
                EsJuridica = p.EsJuridica,
                NombreCompleto = (p.EsJuridica) ?
                                                      p.Nombres :
                                                      p.ApellidoPaterno + " " + p.ApellidoMaterno + ", " + p.Nombres,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                Email = p.Email,
                Celular = p.Celular,
                Telefono = p.Telefono,
                Documento = p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural"
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }
        public static IEnumerable<PersonaItemListDTe> GetTrabajadores_nU()
        {

            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new SecuritySchema();


            query = dbContext.Persona.Where(p => p.EsTrabajador == true && p.Activo && p.Usuario == null);


            personas = query.Select(p => new PersonaItemListDTe()
            {
                Id = p.Id,
                EsJuridica = p.EsJuridica,
                NombreCompleto = (p.EsJuridica) ?
                                                      p.Nombres :
                                                      p.ApellidoPaterno + " " + p.ApellidoMaterno + ", " + p.Nombres,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                Email = p.Email,
                Celular = p.Celular,
                Telefono = p.Telefono,
                Documento = p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural"
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }
        public static IEnumerable<RolDTe> GetRoles()
        {
            IEnumerable<RolDTe> roles;

            var dbContext = new SecuritySchema();

            roles = dbContext.Rol.Select(p => new RolDTe()
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Activo = p.Activo
            });

            dbContext.Rol.AsNoTracking();

            return roles;
        }
        public static IEnumerable<CargoDTe> GetCargos()
        {
            IEnumerable<CargoDTe> cargos;

            var dbContext = new SecuritySchema();

                cargos = dbContext.Cargo.Select(p => new CargoDTe()
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Activo = p.Activo
                });
            dbContext.Cargo.AsNoTracking();

            return cargos;
        }
        public static IEnumerable<UsuarioLoginDTe> GeByFilter(short id, bool esPrincipal)
        {

            var usuarios = new List<UsuarioLoginDTe>();

            using (var dbContext = new SecuritySchema())
            {
                UsuarioLoginDTe usuario;

                IQueryable<Usuario> query = null;

                if (id == 0)
                {
                    query = dbContext.Usuario;
                }
                else
                {
                    if (esPrincipal)
                    {
                        query = dbContext.Usuario.Where(p => p.SedePrincipal_Id == id);
                    }
                    else
                    {
                        query = dbContext.Usuario.Where(p => p.Sede_Id == id);
                    }
                }

                foreach (var efObject in query)
                {
                    usuario = new UsuarioLoginDTe()
                    {
                        Id = efObject.Usuario_Id,
                        NombreUsuario = efObject.Nombre,
                        Password = efObject.Clave,                        
                        Sede_Id = efObject.Sede_Id,
                        SedePrincipal_Id = efObject.SedePrincipal_Id,
                        EsSedePrincipal = efObject.EsSedePrincipal
                    };

                    usuarios.Add(usuario);
                }
            }

            return usuarios;
        }
        public static UsuarioTableRowDTe GetRowById(int id)
        {
            UsuarioTableRowDTe usuario = null; 

            using (var dbContext = new SecuritySchema())
            {
                DirectorioDTe directorio = new DirectorioDTe();
                IQueryable<Directorio> query = dbContext.Directorio.Where(p => p.Persona_Id == id);
                foreach (var efDirectorio in query)
                {
                    directorio = new DirectorioDTe()
                    {
                        Id = efDirectorio.Id,
                        Cargo_Id = efDirectorio.Cargo_Id,
                        Email = efDirectorio.Email,
                        EsResponsable = efDirectorio.EsResponsable                        
                    };
                }
                string roles = "";
                IQueryable<MiembroRol> query2 = dbContext.MiembroRol.Where(p => p.Usuario_Id == id);
                foreach (var efRol in query2)
                {
                    if (roles == "") roles = roles + efRol.Rol_Id;
                    else roles = roles + "," + efRol.Rol_Id;
                }

                var efObject = dbContext.Usuario.Find(id);

                if (efObject != null)
                {
                    if (efObject != null)
                    {
                        usuario = new UsuarioTableRowDTe()
                        {
                            Id = efObject.Usuario_Id,
                            NombreUsuario = efObject.Nombre,
                            Password = "",
                            FechaInicio = efObject.FechaInicio,
                            FechaFin = efObject.FechaFin,
                            Sede_Id = efObject.Sede_Id,
                            SedePrincipal_Id = efObject.SedePrincipal_Id,
                            EsSedePrincipal = efObject.EsSedePrincipal,
                            Directorio_Id = directorio.Id,
                            Cargo_Id = directorio.Cargo_Id,
                            Email =directorio.Email,
                            EsResponsable=directorio.EsResponsable,
                            Roles_Id=roles
                        };

                    }
                }

            }
            return usuario;
        }
        
        public static bool Insert(UsuarioTableRowDTe usuario)
        {
            var usuarioEF = new Usuario()
            {
                Usuario_Id = usuario.Id,
                Nombre = usuario.NombreUsuario,
                Clave = usuario.Password,
                FechaInicio = usuario.FechaInicio,
                FechaFin = usuario.FechaFin,
                Sede_Id = usuario.Sede_Id,
                SedePrincipal_Id = usuario.SedePrincipal_Id,
                EsSedePrincipal = usuario.EsSedePrincipal,
                UsuarioCreacion = usuario.UsuarioModifica,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = usuario.UsuarioModifica,
                FechaModificacion = DateTime.Now,
                Activo = true
            };

            var directorioEF = new Directorio()
            {
                Cargo_Id = usuario.Directorio.Cargo_Id,
                Persona_Id = usuario.Directorio.Persona_Id,
                SedeATFFS_Id = usuario.Directorio.Sede_Id,
                Email = usuario.Directorio.Email,
                EsResponsable = usuario.Directorio.EsResponsable,
                Activo = true,
                UsuarioCreacion = usuario.UsuarioModifica,
                FechaRegistro = DateTime.Now,
                UsuarioModificacion = usuario.UsuarioModifica,
                FechaModificacion = DateTime.Now
            };
            using (var dbContext = new SecuritySchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.Usuario.Add(usuarioEF);
                        dbContext.Directorio.Add(directorioEF);

                        foreach (RolDTe rol in usuario.Roles) {
                            var rolesEF = new MiembroRol()
                            {
                                Rol_Id = rol.Id,
                                Usuario_Id = usuario.Id,
                                UsuarioCreacion = usuario.UsuarioModifica,
                                FechaCreacion = DateTime.Now
                            };
                            dbContext.MiembroRol.Add(rolesEF);
                        }

                       
                        dbContext.SaveChanges();                                                

                        trans.Commit();
                    }
                    catch(Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }                   

            return true;
        }
        public static bool Update(int idDirectorio, string idRoles, UsuarioTableRowDTe usuario)
        {
            using (var dbContext = new SecuritySchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var efObject = dbContext.Usuario.Find(usuario.Id);
                        var efDirectorio = dbContext.Directorio.Find(idDirectorio);

                        if (efObject != null)
                        {
                            efObject.Usuario_Id = usuario.Id;
                            efObject.Nombre = usuario.NombreUsuario;
                            if (usuario.Password!=null) efObject.Clave = usuario.Password;
                            efObject.FechaInicio = usuario.FechaInicio;
                            efObject.FechaFin = usuario.FechaFin;
                            efObject.Sede_Id = usuario.Sede_Id;
                            efObject.SedePrincipal_Id = usuario.SedePrincipal_Id;
                            efObject.EsSedePrincipal = usuario.EsSedePrincipal;
                            efObject.UsuarioModificacion = usuario.UsuarioModifica;
                            efObject.FechaModificacion = DateTime.Now;

                            efDirectorio.Cargo_Id = usuario.Directorio.Cargo_Id;
                            efDirectorio.Persona_Id = usuario.Directorio.Persona_Id;
                            efDirectorio.SedeATFFS_Id = usuario.Directorio.Sede_Id;
                            efDirectorio.Email = usuario.Directorio.Email;
                            efDirectorio.EsResponsable = usuario.Directorio.EsResponsable;
                            efDirectorio.UsuarioModificacion = usuario.UsuarioModifica;
                            efDirectorio.FechaModificacion = DateTime.Now;

                            string[] arr_roles = idRoles.Split(',');
                            IQueryable<MiembroRol> query2 = dbContext.MiembroRol.Where(p => p.Usuario_Id == usuario.Id);
                            foreach (var efRol in query2)
                            {
                                bool existe = false;
                                foreach(var rol in arr_roles)
                                {
                                    if (efRol.Rol_Id == byte.Parse(rol)) existe = true;
                                }
                                if(existe==false) dbContext.MiembroRol.Remove(efRol);                               
                            }
                            foreach (var rol in arr_roles)
                            {
                                bool existe = false;
                                IQueryable<MiembroRol> query3 = dbContext.MiembroRol.Where(p => p.Usuario_Id == usuario.Id);
                                foreach (var efRol2 in query3)
                                {
                                    if (efRol2.Rol_Id == byte.Parse(rol)) existe = true;

                                }
                                if (existe == false)
                                {
                                    var rolesEF = new MiembroRol()
                                    {
                                        Rol_Id = byte.Parse(rol),
                                        Usuario_Id = usuario.Id,
                                        UsuarioCreacion = usuario.UsuarioModifica,
                                        FechaCreacion = DateTime.Now
                                    };
                                    dbContext.MiembroRol.Add(rolesEF);
                                }
                            }

                            dbContext.SaveChanges();
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }
        public static bool DeleteById(int id)
        {
            bool exito = true;

            using (var dbContext = new SecuritySchema())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var efDirectorios = dbContext.Directorio.Where(p => p.Persona_Id == id).ToList();
                        if (efDirectorios != null)
                        {
                            foreach (Directorio ef in efDirectorios)
                            {
                                dbContext.Directorio.Remove(ef);
                            }                            
                        }
                        var efMiembros = dbContext.MiembroRol.Where(p => p.Usuario_Id == id).ToList();
                        if (efMiembros != null)
                        {
                            foreach (MiembroRol ef in efMiembros)
                            {
                                dbContext.MiembroRol.Remove(ef);
                            }
                        }

                        Usuario efObject = dbContext.Usuario.Find(id);
                        if (efObject != null)
                        {
                            dbContext.Usuario.Remove(efObject);
                            dbContext.SaveChanges();
                        }
                        
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        exito = false;
                    }
                }
            }

            return exito;
        }

        //public static InteresadoTableRowDTe GetRowById(int id)
        //{
        //    InteresadoTableRowDTe interesado = null;

        //    using (var dbContext = new PlantacionSchema())
        //    {
        //        var efObject = dbContext.Interesado.Find(id);

        //        if (efObject != null)
        //        {
        //            interesado = new InteresadoTableRowDTe()
        //            {
        //                Id = efObject.Id,
        //                NombrePredio = efObject.NombrePredio,
        //                Zona = efObject.Zona,
        //                Datum = efObject.Datum,
        //                Area = (efObject.Area.HasValue) ? efObject.Area.Value : decimal.Zero,
        //                NumeroCertificado = efObject.NumeroCertificado,

        //                CoordenadaEsteUTM = efObject.CoordenadaEsteUTM,
        //                CoordenadaNorteUTM = efObject.CoordenadaNorteUTM,
        //                TipoZona_Id = efObject.TipoZona_Id,
        //                NombreZona = efObject.NombreZona,
        //                EsPropietario = efObject.EsPropietario,

        //                TipoAutorizacion_Id = efObject.TipoAutorizacion_Id,
        //                DocumentoCondicion = efObject.DocumentoCondicion,
        //                DocumentoContrato = efObject.DocumentoContrato,
        //                FechaRecepcionARFFS = efObject.FechaRecepcionARFFS,
        //                AprobadoEspecialista = efObject.AprobadoEspecialista,
        //                AprobadoDIR = efObject.AprobadoDIR,
        //                AprobadoCatastro = efObject.AprobadoCatastro,
        //                CantidadTotalSuperficieMl = efObject.CantidadTotalSuperficieMl,
        //                CantidadTotalSuperficieHa = efObject.CantidadTotalSuperficieHa,
        //                Observaciones = efObject.Observaciones,
        //                SeccionAvance = efObject.SeccionAvance,
        //                Sede_Id = efObject.Sede_Id,
        //                SedePrincipal_Id = efObject.SedePrincipal_Id,
        //                FechaRecepcion = efObject.FechaRecepcion,
        //                UsuarioCreacion = efObject.UsuarioCreacion,
        //                FechaCreacion = efObject.FechaCreacion,
        //                UsuarioModificacion = efObject.UsuarioModificacion,
        //                FechaModificacion = efObject.FechaModificacion,
        //                CantidadRevisiones = efObject.RevisionesRegistroPlantaciones.Count,
        //                CantidadHistoricos = efObject.PlantacionHistorico.Count
        //            };

        //            if (efObject.TipoAutorizacion_Id != null && efObject.TipoAutorizacion_Id > 0)
        //            {
        //                interesado.Autorizacion = efObject.TipoAutorizacion.Descripcion;
        //            }

        //            if (efObject.Ubigeo != null && efObject.Ubigeo_Id > 0)
        //            {
        //                interesado.Ubigeo_Id = efObject.Ubigeo_Id;
        //                interesado.Departamento = efObject.Ubigeo.NombreDepartamento;
        //                interesado.Provincia = efObject.Ubigeo.NombreProvincia;
        //                interesado.Distrito = efObject.Ubigeo.NombreDistrito;
        //                interesado.CodigoDepartamento = efObject.Ubigeo.CodigoDepartamento;
        //                interesado.CodigoProvincia = efObject.Ubigeo.CodigoProvincia;
        //            }

        //            if (efObject.PersonaInteresado.Count() > 0)
        //            {
        //                foreach (var persona in efObject.PersonaInteresado)
        //                {
        //                    interesado.Personas.Add(new EG.PersonaTableRowDTe()
        //                    {
        //                        Id = persona.Persona.Id,
        //                        PersonaPlantacion_Id = persona.Id,
        //                        Rol = PersonaFacade.GetRolDescripcion(persona.Rol),
        //                        NombreCompleto = (persona.Persona.EsJuridica) ?
        //                                                persona.Persona.Nombres :
        //                                                persona.Persona.ApellidoPaterno + " " + persona.Persona.ApellidoMaterno + ", " + persona.Persona.Nombres,
        //                        Departamento = persona.Ubigeo.NombreDepartamento,
        //                        Provincia = persona.Ubigeo.NombreProvincia,
        //                        Distrito = persona.Ubigeo.NombreDistrito,
        //                        Celular = persona.Celular,
        //                        Telefono = persona.Telefono,
        //                        Direccion = persona.Direccion,
        //                        Zona = string.Format("{0} {1}", (persona.Persona.TipoZona_Id.HasValue) ? persona.Persona.TipoZona.Descripcion : string.Empty, persona.Persona.NombreZona),
        //                        EsJuridica = persona.Persona.EsJuridica,
        //                        EstadoCivil = persona.Persona.EstadoCivil,
        //                        Sexo = persona.Persona.Sexo,
        //                        Documento = persona.Persona.TipoDocumento.Acronimo + ": " + persona.Persona.NumeroDocumento,
        //                        Email = persona.Email,
        //                        TipoPersona = (persona.Persona.EsJuridica) ? "Jurídica" : "Natural",
        //                        UsuarioCreacion = persona.UsuarioCreacion,
        //                        FechaCreacion = persona.FechaCreacion,
        //                        UsuarioModificacion = persona.Persona.UsuarioModificacion,
        //                        FechaModificacion = persona.Persona.FechaModificacion,
        //                        ApellidoMaterno = persona.Persona.ApellidoMaterno,
        //                        ApellidoPaterno = persona.Persona.ApellidoPaterno,
        //                        Ubigeo_Id = persona.Ubigeo_Id,
        //                        CodigoDepartamento = persona.Ubigeo.CodigoDepartamento,
        //                        CodigoProvincia = persona.Ubigeo.CodigoProvincia,
        //                        TipoZona_Id = persona.Persona.TipoZona_Id,
        //                        FechaNacimiento = persona.Persona.FechaNacimiento,
        //                        TipoDocumento_Id = persona.Persona.TipoDocumento_Id,
        //                        Activo = true
        //                    });
        //                }
        //            }

        //            if (efObject.InteresadoDetalle.Count() > 0)
        //            {
        //                foreach (var bloque in efObject.InteresadoDetalle)
        //                {
        //                    var newBloque = new BloqueTableRowDTe()
        //                    {
        //                        Id = bloque.Id,
        //                        Nombre = bloque.Nombre,
        //                        AnnioEstablecimiento = bloque.AnnioEstablecimiento,
        //                        MesEstablecimiento = bloque.MesEstablecimiento,
        //                        AreaPredio = bloque.AreaPredio,
        //                        CoordenadaEsteUTM = bloque.CoordenadaEsteUTM,
        //                        CoordenadaNorteUTM = bloque.CoordenadaNorteUTM,

        //                        ProduccionEstimada = bloque.ProduccionEstimada,
        //                        SistemaPlantacion_Id = bloque.SistemaPlantacion_Id,
        //                        SistemaPlantacionDescripcion = bloque.SistemaPlantacion.Descripcion,
        //                        Plantacion_Id = bloque.Plantacion_Id,
        //                        TotalPlantado = bloque.TotalPlantado,
        //                        UnidadMedida_Id = bloque.UnidadMedida_Id,
        //                        UnidadMedidaDescripcion = bloque.UnidadMedida.Descripcion,
        //                        UnidadMedidaSimbolo = bloque.UnidadMedida.Simbolo,
        //                        CantidadCoordenadasRegistradas = bloque.PlantacionDetalleVertices.Count,
        //                        CantidadSuperficieHa = bloque.CantidadSuperficieHa,
        //                        CantidadSuperficieMl = bloque.CantidadSuperficieMl,
        //                        UriStorageFileName = bloque.UriStorageFileName
        //                    };

        //                    if (bloque.Finalidad_Id != null && bloque.Finalidad_Id > 0)
        //                    {
        //                        newBloque.Finalidad_Id = bloque.Finalidad_Id;
        //                        newBloque.FinalidadDescripcion = bloque.FinalidadProducto.Descripcion;
        //                    }

        //                    if (bloque.EspecieForestal != null && bloque.Especie_Id > 0)
        //                    {

        //                        newBloque.EspecieNombreCientifico = bloque.EspecieForestal.NombreCientifico;
        //                        newBloque.EspecieNombreComun = bloque.NombreComun;
        //                        newBloque.Especie_Id = bloque.Especie_Id;
        //                    }

        //                    interesado.Detalles.Add(newBloque);

        //                }

        //            }

        //        }
        //    }

        //    return interesado;
        //}

    }
}
