using SERFOR.Component.DTEntities.General;
using SERFOR.Component.GeneralCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
{
    public static class ViveroFacade
    {
        public static IEnumerable<ViveroItemListDTe> GetAll()
        {
            IEnumerable<ViveroItemListDTe> viveros;

            var dbContext = new GeneralSchema();

            viveros = dbContext.ViveroForestal.Select(p => new ViveroItemListDTe()
            {
                Id = p.Id,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                TotalEspecies = p.EspecieVivero.Count,
                Direccion = p.Direccion,
                DatosCoordenada = "Zona: " + p.Zona + ", " + p.CoordenadaEsteUTM + "E - " + p.CoordenadaNorteUTM + "N",
                TotalAreaAlmacigo = p.Almacigado.Value,
                TotalAreaRepique = p.Repique.Value,
                FechaCreacion = p.FechaCreacion,
                UsuarioCreacion = p.UsuarioCreacion,
                FechaModificacion = p.FechaModificacion,
                UsuarioModificacion = p.UsuarioModificacion
            });

            dbContext.ViveroForestal.AsNoTracking();

            return viveros;
        }

        public static ViveroTableRowDTe GetRowById(int id)
        {
            ViveroTableRowDTe vivero = null;

            using (var dbContext = new GeneralSchema())
            {
                var efObject = dbContext.ViveroForestal.Find(id);

                if (efObject != null)
                {
                    vivero = new ViveroTableRowDTe()
                    {
                        Id = efObject.Id,
                        TotalEspecies = efObject.EspecieVivero.Count,
                        Direccion = efObject.Direccion,
                        DatosCoordenada = "Zona: " + efObject.Zona + ", " + efObject.CoordenadaEsteUTM + "E - " + efObject.CoordenadaNorteUTM + "N",
                        TotalAreaAlmacigo = efObject.Almacigado.Value,
                        TotalAreaRepique = efObject.Repique.Value,
                        FechaCreacion = efObject.FechaCreacion,
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaModificacion = efObject.FechaModificacion,
                        UsuarioModificacion = efObject.UsuarioModificacion,
                        Zona = efObject.Zona,
                        CoordenadaEsteUTM = efObject.CoordenadaEsteUTM,
                        CoordenadaNorteUTM = efObject.CoordenadaNorteUTM,
                        Observaciones = efObject.Observaciones,
                        FuenteAbastecimientoAgua = efObject.FuenteAgua,


                    };


                    if (efObject.Ubigeo != null && efObject.Ubigeo_Id > 0)
                    {
                        vivero.Ubigeo_Id = efObject.Ubigeo_Id;
                        vivero.Departamento = efObject.Ubigeo.NombreDepartamento;
                        vivero.Provincia = efObject.Ubigeo.NombreProvincia;
                        vivero.Distrito = efObject.Ubigeo.NombreDistrito;
                        vivero.CodigoDepartamento = efObject.Ubigeo.CodigoDepartamento;
                        vivero.CodigoProvincia = efObject.Ubigeo.CodigoProvincia;
                    }

                    if (efObject.PersonaVivero.Count() > 0)
                    {
                        foreach (var persona in efObject.PersonaVivero)
                        {
                            vivero.Personas.Add(new PersonaTableRowDTe()
                            {
                                Id = persona.Persona.Id,
                                PersonaPlantacion_Id = persona.Id,
                                Rol = PersonaFacade.GetRolDescripcion(persona.Rol),
                                NombreCompleto = (persona.Persona.EsJuridica) ?
                                                        persona.Persona.Nombres :
                                                        persona.Persona.ApellidoPaterno + " " + persona.Persona.ApellidoMaterno + ", " + persona.Persona.Nombres,
                                Departamento = persona.Persona.Ubigeo.NombreDepartamento,
                                Provincia = persona.Persona.Ubigeo.NombreProvincia,
                                Distrito = persona.Persona.Ubigeo.NombreDistrito,
                                Celular = persona.Celular,
                                Telefono = persona.Telefono,
                                Direccion = persona.Direccion,
                                Zona = string.Format("{0} {1}", (persona.Persona.TipoZona_Id.HasValue) ? persona.Persona.TipoZona.Descripcion : string.Empty, persona.Persona.NombreZona),
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
                                CodigoDepartamento = persona.Persona.Ubigeo.CodigoDepartamento,
                                CodigoProvincia = persona.Persona.Ubigeo.CodigoProvincia,
                                TipoZona_Id = persona.Persona.TipoZona_Id,
                                FechaNacimiento = persona.Persona.FechaNacimiento,
                                TipoDocumento_Id = persona.Persona.TipoDocumento_Id,
                                Activo = true
                            });
                        }
                    }

                    if (efObject.EspecieVivero.Count() > 0)
                    {
                        foreach (var especie in efObject.EspecieVivero)
                        {
                            var newEspecie = new EspecieViveroTableRowDTe()
                            {
                                Id = especie.Id,
                                NombreComun = especie.NombreComun,
                                ProduccionEstimada = especie.ProduccionCampania,
                            };


                            if (especie.EspecieForestal != null && especie.Especie_Id > 0)
                            {
                                newEspecie.NombreCientifico = especie.EspecieForestal.NombreCientifico;
                                newEspecie.Especie_Id = especie.Especie_Id;
                            }

                            vivero.Detalles.Add(newEspecie);

                        }

                    }


                }
            }

            return vivero;
        }


        public static bool DeleteById(int id)
        {

            bool exito = true;

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.ViveroForestal.Find(id);

                    if (efObject != null)
                    {

                        dbContext.ViveroForestal.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;
        }

        public static int InsertSectionOne(ViveroTableRowDTe vivero)
        {
            int id = 0;

            var plantacionEF = new ViveroForestal()
            {
                Observaciones = vivero.Observaciones,
                Zona = string.Empty,
                Almacigado = decimal.Zero,
                Repique = decimal.Zero,
                UsuarioCreacion = vivero.UsuarioCreacion,
                FechaCreacion = vivero.FechaCreacion = DateTime.Now,
                UsuarioModificacion = vivero.UsuarioCreacion,
                FechaModificacion = vivero.FechaCreacion = DateTime.Now
            };

            using (var dbContext = new GeneralSchema())
            {
                dbContext.ViveroForestal.Add(plantacionEF);
                dbContext.SaveChanges();

                id = plantacionEF.Id;
            }

            return id;
        }

        public static int UpdateSectionOne(int id, ViveroTableRowDTe plantacion)
        {

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.ViveroForestal.Find(id);

                    if (efObject != null)
                    {

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

        public static PersonaTableRowDTe InsertPersonaByViveroId(int idVivero, int idPersona, string rol, string usuario)
        {
            PersonaTableRowDTe persona = null;

            using (var dbContext = new GeneralSchema())
            {

                if (idVivero.Equals(0))
                {
                    var viveroEF = new ViveroForestal()
                    {
                        UsuarioCreacion = usuario,
                        FechaCreacion = DateTime.Now,
                        UsuarioModificacion = usuario,
                        FechaModificacion = DateTime.Now
                    };
                    dbContext.ViveroForestal.Add(viveroEF);
                    dbContext.SaveChanges();

                    idVivero = viveroEF.Id;

                }

                var personaEF = dbContext.Persona.Find(idPersona);

                if (personaEF != null && idVivero > 0)
                {
                    var personaViveroEF = new PersonaVivero()
                    {
                        Persona_Id = idPersona,
                        Vivero_Id = idVivero,
                        Rol = rol,
                        Email = personaEF.Email,
                        Ubigeo_Id = personaEF.Ubigeo_Id,
                        Direccion = personaEF.Direccion,
                        Telefono = personaEF.Telefono,
                        Celular = personaEF.Celular,
                        UsuarioCreacion = usuario,
                        FechaCreacion = DateTime.Now,

                    };
                    dbContext.PersonaVivero.Add(personaViveroEF);
                    dbContext.SaveChanges();

                    if (personaViveroEF.Id > 0)
                    {
                        persona = new PersonaTableRowDTe()
                        {
                            Id = idPersona,
                            PersonaPlantacion_Id = personaViveroEF.Id,
                            Rol = PersonaFacade.GetRolDescripcion(personaViveroEF.Rol),
                            NombreCompleto = personaEF.Nombres,
                            Departamento = personaEF.Ubigeo.NombreDepartamento,
                            Provincia = personaEF.Ubigeo.NombreProvincia,
                            Distrito = personaEF.Ubigeo.NombreDistrito,
                            Celular = personaViveroEF.Celular,
                            Telefono = personaViveroEF.Telefono,
                            Direccion = personaViveroEF.Direccion,
                            Zona = personaEF.NombreZona,
                            EsJuridica = personaEF.EsJuridica,
                            EstadoCivil = personaEF.EstadoCivil,
                            Sexo = personaEF.Sexo,
                            Documento = personaEF.NumeroDocumento,
                            Email = personaViveroEF.Email,
                            TipoPersona = (personaEF.EsJuridica) ? "Jurídica" : "Natural",
                            UsuarioCreacion = personaViveroEF.UsuarioCreacion,
                            FechaCreacion = personaViveroEF.FechaCreacion,
                            UsuarioModificacion = personaEF.UsuarioModificacion,
                            FechaModificacion = personaEF.FechaModificacion,
                            ApellidoMaterno = personaEF.ApellidoMaterno,
                            ApellidoPaterno = personaEF.ApellidoPaterno,
                            Ubigeo_Id = personaViveroEF.Ubigeo_Id,
                            CodigoDepartamento = personaEF.Ubigeo.CodigoDepartamento,
                            CodigoProvincia = personaEF.Ubigeo.CodigoProvincia,
                            TipoZona_Id = personaEF.TipoZona_Id,
                            FechaNacimiento = personaEF.FechaNacimiento,
                            TipoDocumento_Id = personaEF.TipoDocumento_Id,
                            Activo = true
                        };
                    }
                }

            }

            return persona;

        }

        public static bool DeletePersonaById(int id)
        {
            bool exito = true;

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.PersonaVivero.Find(id);

                    if (efObject != null)
                    {

                        dbContext.PersonaVivero.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;
        }

        public static PersonaTableRowDTe GetPersonaRowById(int id)
        {
            PersonaTableRowDTe persona = null;

            using (var dbContext = new GeneralSchema())
            {
                var efObject = dbContext.PersonaVivero.Find(id);

                if (efObject != null)
                {
                    persona = new PersonaTableRowDTe()
                    {
                        Id = efObject.Id,
                        NombreCompleto = (efObject.Persona.EsJuridica) ?
                                                        efObject.Persona.Nombres :
                                                        efObject.Persona.ApellidoPaterno + " " + efObject.Persona.ApellidoMaterno + ", " + efObject.Persona.Nombres,
                        Rol = PersonaFacade.GetRolDescripcion(efObject.Rol),
                        Departamento = efObject.Ubigeo.NombreDepartamento,
                        Provincia = efObject.Ubigeo.NombreProvincia,
                        Distrito = efObject.Ubigeo.NombreDistrito,
                        Celular = efObject.Celular,
                        Telefono = efObject.Telefono,
                        Direccion = efObject.Direccion,
                        Zona = efObject.Persona.NombreZona,
                        EsJuridica = efObject.Persona.EsJuridica,
                        EstadoCivil = efObject.Persona.EstadoCivil,
                        Sexo = efObject.Persona.Sexo,
                        Documento = efObject.Persona.NumeroDocumento,
                        Email = efObject.Email,
                        TipoPersona = (efObject.Persona.EsJuridica) ? "Jurídica" : "Natural",
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaCreacion = efObject.FechaCreacion,
                        UsuarioModificacion = efObject.Persona.UsuarioModificacion,
                        FechaModificacion = efObject.Persona.FechaModificacion,
                        ApellidoMaterno = efObject.Persona.ApellidoMaterno,
                        ApellidoPaterno = efObject.Persona.ApellidoPaterno,
                        Ubigeo_Id = efObject.Ubigeo_Id,
                        CodigoDepartamento = efObject.Ubigeo.CodigoDepartamento,
                        CodigoProvincia = efObject.Ubigeo.CodigoProvincia,
                        TipoZona_Id = efObject.Persona.TipoZona_Id,
                        FechaNacimiento = efObject.Persona.FechaNacimiento,
                        TipoDocumento_Id = efObject.Persona.TipoDocumento_Id,
                        Activo = true
                    };

                }
            }

            return persona;
        }

        public static int UpdateSectionTwo(int id, ViveroTableRowDTe vivero)
        {

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.ViveroForestal.Find(id);

                    if (efObject != null)
                    {
                        efObject.Zona = vivero.Zona;
                        efObject.Direccion = vivero.Direccion;
                        efObject.CoordenadaNorteUTM = vivero.CoordenadaNorteUTM;
                        efObject.CoordenadaEsteUTM = vivero.CoordenadaEsteUTM;
                        efObject.Ubigeo_Id = vivero.Ubigeo_Id;

                        efObject.UsuarioModificacion = vivero.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        dbContext.SaveChanges();
                    }

                }
                catch { id = 0; }
            }

            return id;
        }

        public static int UpdateSectionThree(int id, ViveroTableRowDTe vivero)
        {
            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.ViveroForestal.Find(id);

                    if (efObject != null)
                    {
                        efObject.FuenteAgua = vivero.FuenteAbastecimientoAgua;
                        efObject.Almacigado = vivero.TotalAreaAlmacigo;
                        efObject.Repique = vivero.TotalAreaRepique;

                        efObject.UsuarioModificacion = vivero.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;

                        dbContext.SaveChanges();
                    }

                }
                catch { id = 0; }
            }

            return id;
        }


        public static decimal InsertEspecie(EspecieViveroTableRowDTe especie)
        {
            var detalleEF = new EspecieVivero()
            {
                Vivero_Id = especie.Vivero_Id,
                NombreComun = especie.NombreComun,
                ProduccionCampania = especie.ProduccionEstimada,
                UsuarioCreacion = especie.UsuarioCreacion,
                FechaCreacion = DateTime.Now,
                UsuarioModificacion = especie.UsuarioCreacion,
                FechaModificacion = DateTime.Now
            };

            if (especie.Especie_Id > 0) detalleEF.Especie_Id = especie.Especie_Id;

            decimal id = 0;
            using (var dbContext = new GeneralSchema())
            {

                dbContext.EspecieVivero.Add(detalleEF);
                dbContext.SaveChanges();

                id = detalleEF.Id;
            }

            return id;
        }

        public static object DeleteEspecieById(int id)
        {
            bool exito = true;

            using (var dbContext = new GeneralSchema())
            {

                var efObject = dbContext.EspecieVivero.Find(id);

                if (efObject != null)
                {
                    dbContext.EspecieVivero.Remove(efObject);

                    dbContext.SaveChanges();

                }

            }

            return exito;
        }
    }
}
