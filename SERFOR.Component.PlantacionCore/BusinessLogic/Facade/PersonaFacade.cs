using SERFOR.Component.DTEntities.General;
using SERFOR.Component.PlantacionCore.DataAccess;
using SERFOR.Component.Tools.DateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class PersonaFacade
    {
        public static string GetRolDescripcion(string rol)
        {
            string nombre;
            switch (rol)
            {
                case "T":
                    nombre = "Titular";
                    break;
                case "R":
                    nombre = "Responsable legal";
                    break;
                case "B":
                    nombre = "Brigadista";
                    break;
                case "P":
                    nombre = "Propietario";
                    break;
                case "I":
                    nombre = "Interesado";
                    break;
                default:
                    nombre = "Rol no especificado";
                    break;
            }

            return nombre;
        }

        public static PersonaTableRowDTe InsertByPlantacionId(int idPlantacion, int idPersona, string rol, string usuario)
        {
            PersonaTableRowDTe persona = null;

            using (var dbContext = new PlantacionSchema())
            {

                if (idPlantacion.Equals(0))
                {
                    var plantacionEF = new Plantacion()
                    {
                        EsPropietario = false,
                        UsuarioCreacion = usuario,
                        FechaCreacion = DateTime.Now,
                        UsuarioModificacion = usuario,
                        FechaModificacion = DateTime.Now
                    };
                    dbContext.PlantacionSet.Add(plantacionEF);
                    dbContext.SaveChanges();

                    idPlantacion = plantacionEF.Id;

                }

                var personaEF = dbContext.PersonaSet.Find(idPersona);
                if(rol=="T") personaEF.EsAdministrado = true;

                if (personaEF != null && idPlantacion > 0)
                {
                    var personaPlantacionEF = new PersonaPlantacion()
                    {
                        Persona_Id = idPersona,
                        Plantacion_Id = idPlantacion,
                        Rol = rol,
                        Email = personaEF.Email,
                        Ubigeo_Id = personaEF.Ubigeo_Id,
                        Direccion = personaEF.Direccion,
                        Telefono = personaEF.Telefono,
                        Celular = personaEF.Celular,
                        UsuarioCreacion = usuario,
                        FechaCreacion = DateTime.Now,

                    };
                    dbContext.PersonaPlantacionSet.Add(personaPlantacionEF);
                    dbContext.SaveChanges();

                    if (personaPlantacionEF.Id > 0)
                    {
                        persona = new PersonaTableRowDTe()
                        {
                            Id = idPersona,
                            PersonaPlantacion_Id = personaPlantacionEF.Id,
                            Rol = GetRolDescripcion(personaPlantacionEF.Rol),
                            NombreCompleto = personaEF.ApellidoPaterno +" "+ personaEF.ApellidoMaterno + ", "+ personaEF.Nombres,
                            Departamento = personaEF.Ubigeo.NombreDepartamento,
                            Provincia = personaEF.Ubigeo.NombreProvincia,
                            Distrito = personaEF.Ubigeo.NombreDistrito,
                            Celular = personaPlantacionEF.Celular,
                            Telefono = personaPlantacionEF.Telefono,
                            Direccion = personaPlantacionEF.Direccion,
                            EsJuridica = personaEF.EsJuridica,
                            EstadoCivil = personaEF.EstadoCivil,
                            Sexo = personaEF.Sexo,
                            Documento = personaEF.NumeroDocumento,
                            Email = personaPlantacionEF.Email,
                            TipoPersona = (personaEF.EsJuridica) ? "Jurídica" : "Natural",
                            UsuarioCreacion = personaPlantacionEF.UsuarioCreacion,
                            FechaCreacion = personaPlantacionEF.FechaCreacion,
                            UsuarioModificacion = personaEF.UsuarioModificacion,
                            FechaModificacion = personaEF.FechaModificacion,
                            ApellidoMaterno = personaEF.ApellidoMaterno,
                            ApellidoPaterno = personaEF.ApellidoPaterno,
                            Ubigeo_Id = personaPlantacionEF.Ubigeo_Id,
                            CodigoDepartamento = personaEF.Ubigeo.CodigoDepartamento,
                            CodigoProvincia = personaEF.Ubigeo.CodigoProvincia,
                            FechaNacimiento = personaEF.FechaNacimiento,
                            TipoDocumento_Id = personaEF.TipoDocumento_Id,
                            Activo = true
                        };
                    }
                }

            }

            return persona;

        }

        public static bool DeleteById(int id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PersonaPlantacionSet.Find(id);

                    if (efObject != null)
                    {

                        dbContext.PersonaPlantacionSet.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;
        }
        public static bool DeleteInteresadoById(int id)
        {
            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PersonaSet.Find(id);

                    if (efObject != null)
                    {

                        dbContext.PersonaSet.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;
        }
        public static PersonaTableRowDTe GetRowById(int id)
        {
            PersonaTableRowDTe persona = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PersonaPlantacionSet.Find(id);

                if (efObject != null)
                {
                    persona = new PersonaTableRowDTe()
                    {
                        Id = efObject.Id,
                        NombreCompleto = (efObject.Persona.EsJuridica) ?
                                                        efObject.Persona.Nombres :
                                                        efObject.Persona.ApellidoPaterno + " " + efObject.Persona.ApellidoMaterno + ", " + efObject.Persona.Nombres,
                        Rol = GetRolDescripcion(efObject.Rol),
                        Departamento = efObject.Ubigeo.NombreDepartamento,
                        Provincia = efObject.Ubigeo.NombreProvincia,
                        Distrito = efObject.Ubigeo.NombreDistrito,
                        Celular = efObject.Celular,
                        Telefono = efObject.Telefono,
                        Direccion = efObject.Direccion,

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
                        FechaNacimiento = efObject.Persona.FechaNacimiento,
                        TipoDocumento_Id = efObject.Persona.TipoDocumento_Id,
                        Activo = true
                    };

                }
            }

            return persona;
        }
        public static PersonaTableRowDTe GetRowInteresadoById(int id)
        {
            PersonaTableRowDTe persona = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PersonaSet.Find(id);

                if (efObject != null)
                {
                    persona = new PersonaTableRowDTe()
                    {
                        Id = efObject.Id,
                        NombreCompleto = efObject.Nombres,
                        Departamento = efObject.Ubigeo.NombreDepartamento,
                        Provincia = efObject.Ubigeo.NombreProvincia,
                        Distrito = efObject.Ubigeo.NombreDistrito,
                        Celular = efObject.Celular,
                        Telefono = efObject.Telefono,
                        Direccion = efObject.Direccion,                       
                        EsJuridica = efObject.EsJuridica,
                        EstadoCivil = efObject.EstadoCivil,
                        Sexo = efObject.Sexo,
                        Documento = efObject.NumeroDocumento,
                        Email = efObject.Email,
                        TipoPersona = (efObject.EsJuridica) ? "Jurídica" : "Natural",
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaCreacion = efObject.FechaCreacion,
                        UsuarioModificacion = efObject.UsuarioModificacion,
                        FechaModificacion = efObject.FechaModificacion,
                        ApellidoMaterno = efObject.ApellidoMaterno,
                        ApellidoPaterno = efObject.ApellidoPaterno,
                        Ubigeo_Id = efObject.Ubigeo_Id,
                        CodigoDepartamento = efObject.Ubigeo.CodigoDepartamento,
                        CodigoProvincia = efObject.Ubigeo.CodigoProvincia,
                        FechaNacimiento = efObject.FechaNacimiento,
                        TipoDocumento_Id = efObject.TipoDocumento_Id,
                        Activo = efObject.Activo
                    };

                }
            }

            return persona;
        }

        public static IEnumerable<PersonaItemListDTe> GetInteresados()
        {

            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new PlantacionSchema();


            query = dbContext.PersonaSet.Where(p => p.EsAdministrado == true && p.Activo && !p.PersonaPlantacion.Any(x => x.Rol=="T"));


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

            dbContext.PersonaSet.AsNoTracking();

            return personas;
        }


        public static int Insert(PersonaTableRowDTe persona)
        {
            int id = 0;

            var personaEF = new Persona()
            {
                TipoDocumento_Id = persona.TipoDocumento_Id,
                NumeroDocumento = persona.Documento,
                Nombres = persona.NombreCompleto,
                ApellidoPaterno = persona.ApellidoPaterno,
                ApellidoMaterno = persona.ApellidoMaterno,
                EsJuridica = persona.EsJuridica,
                Sexo = persona.Sexo,
                FechaNacimiento = persona.FechaNacimiento,
                EstadoCivil = persona.EstadoCivil,
                Telefono = persona.Telefono,
                Celular = persona.Celular,
                Email = persona.Email,
                Ubigeo_Id = persona.Ubigeo_Id,
                Direccion = persona.Direccion,
                UsuarioCreacion = persona.UsuarioCreacion,
                FechaCreacion = persona.FechaCreacion = DateTime.Now,
                UsuarioModificacion = persona.UsuarioCreacion,
                FechaModificacion = persona.FechaCreacion = DateTime.Now,
                Activo = true,
                EsAdministrado = persona.EsAdministrado,
                EsTrabajador = persona.EsTrabajador
            };

            using (var dbContext = new PlantacionSchema())
            {
                dbContext.PersonaSet.Add(personaEF);
                dbContext.SaveChanges();

                id = personaEF.Id;
            }

            return id;
        }


        public static int Update(int id, PersonaTableRowDTe persona)
        {

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.PersonaSet.Find(id);

                    if (efObject != null)
                    {
                        efObject.TipoDocumento_Id = persona.TipoDocumento_Id;
                        efObject.NumeroDocumento = persona.Documento;
                        efObject.Nombres = persona.NombreCompleto;
                        efObject.ApellidoPaterno = persona.ApellidoPaterno;
                        efObject.ApellidoMaterno = persona.ApellidoMaterno;
                        efObject.EsJuridica = persona.EsJuridica;
                        efObject.Sexo = persona.Sexo;
                        efObject.FechaNacimiento = persona.FechaNacimiento;
                        efObject.EstadoCivil = persona.EstadoCivil;
                        efObject.Telefono = persona.Telefono;
                        efObject.Celular = persona.Celular;
                        efObject.Email = persona.Email;
                        efObject.Ubigeo_Id = persona.Ubigeo_Id;
                        efObject.Direccion = persona.Direccion;
                        efObject.UsuarioModificacion = persona.UsuarioModificacion;
                        efObject.FechaModificacion = DateTime.Now;
                        efObject.Activo = true;

                        dbContext.SaveChanges();


                    }
                }
                catch { id = 0; }

            }

            return id;
        }

        public static PersonaDetailDTe GetDetailById(int id)
        {
            PersonaDetailDTe persona = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PersonaSet.Find(id);

                if (efObject != null)
                {
                    persona = new PersonaDetailDTe()
                    {
                        NombreCompleto = (efObject.EsJuridica) ?
                                            efObject.Nombres :
                                            string.Format("{0} {1}, {2}", efObject.ApellidoPaterno, efObject.ApellidoMaterno, efObject.Nombres),
                        Departamento = efObject.Ubigeo.NombreDepartamento,
                        Provincia = efObject.Ubigeo.NombreProvincia,
                        Distrito = efObject.Ubigeo.NombreDistrito,
                        Celular = efObject.Celular,
                        Telefono = efObject.Telefono,
                        Direccion = efObject.Direccion,   
                        EsJuridica = efObject.EsJuridica,
                        EstadoCivil = getEstadoCivil(Convert.ToChar(efObject.EstadoCivil)),
                        Sexo = (efObject.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                        Documento = string.Format("{0}: {1}", efObject.TipoDocumento.Acronimo, efObject.NumeroDocumento),
                        Email = efObject.Email,
                        Id = efObject.Id,
                        TipoPersona = (efObject.EsJuridica) ? "Jurídica" : "Natural",
                        Activo = efObject.Activo,
                        FechaNacimiento = (efObject.FechaNacimiento.HasValue) ? efObject.FechaNacimiento.Value : DateTime.MinValue,
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaCreacion = efObject.FechaCreacion,
                        RegistroLargo = string.Format("Registrado el {0:0#} de {1} del {2}",
                                        efObject.FechaCreacion.Day,
                                        Month.GetMonthName(efObject.FechaCreacion.Month),
                                        efObject.FechaCreacion.Year),
                        RegistroCorto = string.Format("Registrado el {0:dd/MM/yyyy} ", efObject.FechaCreacion),
                        UsuarioModificacion = efObject.UsuarioModificacion,
                        FechaModificacion = efObject.FechaModificacion,
                        ModificacionLargo = string.Format("Modificado por última vez, el {0:0#} de {1} del {2}",
                                        efObject.FechaModificacion.Day,
                                        Month.GetMonthName(efObject.FechaModificacion.Month),
                                        efObject.FechaModificacion.Year),
                        ModificacionCorto = string.Format("Modificado el {0:dd/MM/yyyy} ", efObject.FechaModificacion)
                    };


                    int progress = 14;
                    if (string.IsNullOrEmpty(efObject.Celular)) progress--;
                    if (string.IsNullOrEmpty(efObject.Telefono)) progress--;
                    if (string.IsNullOrEmpty(efObject.Direccion)) progress--;
                    if (string.IsNullOrEmpty(efObject.Email)) progress--;
                    if (efObject.FechaNacimiento == null || efObject.FechaNacimiento.Equals(DateTime.MinValue)) progress--;

                    switch (progress)
                    {
                        case 8:
                        case 9:
                            persona.ProgresoNivel = "danger";
                            break;
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            persona.ProgresoNivel = "warning";
                            break;
                        case 14:
                            persona.ProgresoNivel = "success";
                            break;
                    }

                    persona.ProgresoRegistro = progress * 100 / 15;

                }
            }

            return persona;
        }
        private static string getEstadoCivil(char estado)
        {
            string nombre;
            switch (estado)
            {
                case 'S':
                    nombre = "Soltero";
                    break;
                case 'C':
                    nombre = "Casado";
                    break;
                case 'V':
                    nombre = "Viudo";
                    break;
                //default:
                //    nombre = "Feliz";
                //    break;
                default:
                    nombre = "Soltero";
                    break;
            }

            return nombre;
        }
    }
}
