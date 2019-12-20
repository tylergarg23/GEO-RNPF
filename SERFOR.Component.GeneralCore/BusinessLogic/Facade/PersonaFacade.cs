using SERFOR.Component.DTEntities.General;
using SERFOR.Component.GeneralCore.DataAccess;
using SERFOR.Component.Tools.DateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
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
                //    break
                default:
                    nombre = "Soltero";
                    break;
            }

            return nombre;
        }

        public static bool DeleteById(int id)
        {
            bool exito = true;

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.Persona.Find(id);

                    if (efObject != null)
                    {

                        dbContext.Persona.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
            }

            return exito;
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

            using (var dbContext = new GeneralSchema())
            {
                dbContext.Persona.Add(personaEF);
                dbContext.SaveChanges();

                id = personaEF.Id;
            }

            return id;
        }


        public static int Update(int id, PersonaTableRowDTe persona)
        {

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.Persona.Find(id);

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

        public static PersonaTableRowDTe GetRowById(int id)
        {
            PersonaTableRowDTe persona = null;

            using (var dbContext = new GeneralSchema())
            {
                var efObject = dbContext.Persona.Find(id);

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

        public static PersonaDetailDTe GetDetailById(int id)
        {
            PersonaDetailDTe persona = null;

            using (var dbContext = new GeneralSchema())
            {
                var efObject = dbContext.Persona.Find(id);

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
                        //gpaima - 6/11/19
                        //EstadoCivil = getEstadoCivil(Convert.ToChar(efObject.EstadoCivil)),                      
                        //Sexo = (efObject.Sexo.Equals("M")) ? "Masculino" : "Femenino",
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

        public static int GetTotal()
        {
            int total = 0;
            using (var dbContext = new GeneralSchema())
            {
                total = dbContext.Persona.Count();
            }
            return total;

            //return 150000;
        }

        /// <summary>
        /// Retorna registros de la tabla persona que se mostrarán en el listado principal
        /// </summary>
        /// <returns>Lista de personas naturales y jurídicas</returns>
        public static IEnumerable<PersonaItemListDTe> GetByParams(int offset, int pageSize, string sorParam, string searchText)
        {
            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new GeneralSchema();

            if (!String.IsNullOrEmpty(searchText))
            {
                query = dbContext.Persona.Where(p => p.Nombres.Contains(searchText)
                                              || p.ApellidoPaterno.Contains(searchText)
                                              || p.ApellidoMaterno.Contains(searchText)
                                              || p.Ubigeo.NombreDepartamento.Contains(searchText)
                                              || p.Ubigeo.NombreProvincia.Contains(searchText)
                                              || p.Ubigeo.NombreDistrito.Contains(searchText)
                                              //|| p.EsJuridica.Equals(searchText=="1")
                                              || p.Email.Contains(searchText)
                                              || p.NumeroDocumento.Contains(searchText));
            }
            else
            {
                query = dbContext.Persona;
            }

            switch (sorParam)
            {
                case "NombreCompleto_DESC":
                    query = query.OrderByDescending(p => p.ApellidoPaterno).ThenByDescending(p => p.ApellidoMaterno).ThenByDescending(p => p.Nombres);
                    break;
                case "NombreCompleto_ASC":
                    query = query.OrderBy(p => p.ApellidoPaterno).ThenBy(p => p.ApellidoMaterno).ThenBy(p => p.Nombres);
                    break;
                case "Departamento_DESC":
                    query = query.OrderByDescending(p => p.Ubigeo.NombreDepartamento);
                    break;
                case "Departamento_ASC":
                    query = query.OrderBy(p => p.Ubigeo.NombreDepartamento);
                    break;
                case "Provincia_DESC":
                    query = query.OrderByDescending(p => p.Ubigeo.NombreProvincia);
                    break;
                case "Provincia_ASC":
                    query = query.OrderBy(p => p.Ubigeo.NombreProvincia);
                    break;
                case "Distrito_DESC":
                    query = query.OrderByDescending(p => p.Ubigeo.NombreDistrito);
                    break;
                case "Distrito_ASC":
                    query = query.OrderBy(p => p.Ubigeo.NombreDistrito);
                    break;
                case "TipoPersona_DESC":
                    query = query.OrderByDescending(p => p.EsJuridica);
                    break;
                case "TipoPersona_ASC":
                    query = query.OrderBy(p => p.EsJuridica);
                    break;
                case "Id_DESC":
                    query = query.OrderByDescending(p => p.Id);
                    break;
                case "Id_ASC":
                    query = query.OrderBy(p => p.Id);
                    break;
                case "Documento_DESC":
                    query = query.OrderByDescending(p => p.NumeroDocumento);
                    break;
                case "Documento_ASC":
                    query = query.OrderBy(p => p.NumeroDocumento);
                    break;
                default:  // ID ascending 
                    query = query.OrderBy(p => p.Id);
                    break;

            }
            query = query.Skip(offset).Take(pageSize);

            personas = query.Select(p => new PersonaItemListDTe()
            {
                Id = p.Id,
                NombreCompleto = (p.EsJuridica) ?
                                                        p.Nombres :
                                                        p.ApellidoPaterno + " " + p.ApellidoMaterno + ", " + p.Nombres,
                Departamento = p.Ubigeo.NombreDepartamento,
                Provincia = p.Ubigeo.NombreProvincia,
                Distrito = p.Ubigeo.NombreDistrito,
                Email = p.Email,
                Celular = p.Celular,
                Telefono = p.Telefono,
                Documento = p.TipoDocumento.Acronimo + ": " + p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural"
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }

        public static IEnumerable<PersonaItemListDTe> GetBySearchText(string searchText, string cadena)
        {
            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new GeneralSchema();

            if (!String.IsNullOrEmpty(searchText))
            {
                query = dbContext.Persona.Where(p => p.Nombres.Contains(searchText)
                                              || p.ApellidoPaterno.Contains(searchText)
                                              || p.ApellidoMaterno.Contains(searchText)
                                              || p.NumeroDocumento.Contains(searchText));//.Where(p=> !(cadena.Contains(p.Id.ToString())));

            }
            else
            {
                query = dbContext.Persona;
            }

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
                Documento = p.TipoDocumento.Acronimo + ": " + p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural"                
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }
        public static IEnumerable<PersonaItemListDTe> GetByPersona_xTipoNumero(string tipo, string numero)
        {
            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new GeneralSchema();

            if (!String.IsNullOrEmpty(tipo) && !String.IsNullOrEmpty(numero))
            {
                int idtipo = int.Parse(tipo);
                query = dbContext.Persona.Where(p => p.TipoDocumento.Id== idtipo
                                              && p.NumeroDocumento.Contains(numero));
            }
            else
            {
                query = dbContext.Persona;
            }

            personas = query.Select(p => new PersonaItemListDTe()
            {
                Id = p.Id,
                EsJuridica = p.EsJuridica,
                NombreCompleto = (p.TipoDocumento.Id == 2) ?
                                                       p.Nombres :
                                                        p.ApellidoPaterno + " " + p.ApellidoMaterno + ", " + p.Nombres,
                Nombres = p.Nombres,
                ApellidoPaterno = p.ApellidoPaterno,
                ApellidoMaterno = p.ApellidoMaterno,
                Departamento = p.Ubigeo.CodigoDepartamento,
                Provincia = p.Ubigeo.CodigoProvincia,
                Distrito = p.Ubigeo.Id.ToString(),
                Email = p.Email,
                Celular = p.Celular,
                Telefono = p.Telefono,
                Documento = p.TipoDocumento.Acronimo + ": " + p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural",
                FechaNacimiento = p.FechaNacimiento,
                Direccion = p.Direccion
                //(!p.FechaNacimiento.HasValue)?"": string.Format("{0:yyyy-MM-dd}", p.FechaNacimiento)
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }

        public static IEnumerable<PersonaItemListDTe> GetAll()
        {
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new GeneralSchema();

            personas = dbContext.Persona.Select(p => new PersonaItemListDTe()
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
                Documento = p.TipoDocumento.Acronimo + ": " + p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural"
            });

           dbContext.Persona.AsNoTracking();

            return personas;
        }
        
        public static IEnumerable<PersonaItemListDTe> GetAll_wUsuario()
        {
            IQueryable<Persona> query = null;
            IEnumerable<PersonaItemListDTe> personas;

            var dbContext = new GeneralSchema();

            query = dbContext.Persona.Where(p => p.Directorio.Any());
            //query = dbContext.Persona;
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
                Documento = p.TipoDocumento.Acronimo + ": " + p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",
                TipoPersona = (p.EsJuridica) ? "Jurídica" : "Natural"
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }
    }
}
