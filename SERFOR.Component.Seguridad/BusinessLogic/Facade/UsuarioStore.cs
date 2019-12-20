using SERFOR.Component.DTEntities.Seguridad;
using SERFOR.Component.SeguridadCore.DataAccess;
using System;
using System.Linq;

namespace SERFOR.Component.SeguridadCore.BusinessLogic.Facade
{
    public class UsuarioStore
    {
        public UsuarioLoginDTe Usuario { get; private set; }

        /// <summary>
        /// Constructor que instancia el objeto UsuarioDTe con la información relevante para 
        /// autorización y validación de permisos.
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        public UsuarioStore(string nombre)
        {
            var dbContext = new SecuritySchema();

            var user = dbContext.Usuario.Where(u => u.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase) && u.Activo).FirstOrDefault();

            if (user != null)
            {
                // Carga la información personal del usuario
                Usuario = new UsuarioLoginDTe()
                {
                    Id = user.Usuario_Id,
                    NombreUsuario = nombre,
                    Password = user.Clave,
                    NombreCompleto = string.Format("{0} {1}, {2}", user.Persona.ApellidoPaterno, user.Persona.ApellidoMaterno, user.Persona.Nombres)
                };

                // Si ha sido registrado correctamente en el directorio, cargamos la información institucional      
                var directorio = user.Persona.Directorio.Where(d => d.Activo).FirstOrDefault();
                if (directorio != null)
                {
                    Usuario.EsInvitado = false;
                    Usuario.Email = directorio.Email;
                    Usuario.Sede = (directorio.SedeAutoridadForestal.TipoSede.EsPrincipal) ?
                          string.Format("{0} - {1}", directorio.SedeAutoridadForestal.Nombre, directorio.SedeAutoridadForestal.AutoridadForestal.Nombre) :
                          string.Format("{0} {1}", directorio.SedeAutoridadForestal.TipoSede.Nombre, directorio.SedeAutoridadForestal.Nombre);
                    Usuario.Sede_Id = directorio.SedeAutoridadForestal.Id;
                    Usuario.SedePrincipal_Id = (directorio.SedeAutoridadForestal.TipoSede.EsPrincipal) ? directorio.SedeAutoridadForestal.Id : user.SedePrincipal_Id;
                    Usuario.EsSedePrincipal = directorio.SedeAutoridadForestal.TipoSede.EsPrincipal;
                    Usuario.Roles = user.MiembroRol.Select(mr => new RolDTe
                    {
                        Codigo = mr.Rol.Codigo,
                        Id = mr.Rol.Id,
                        Nombre = mr.Rol.Nombre
                    }).ToList();
                }
                else
                {
                    Usuario.EsInvitado = true;
                    Usuario.Email = user.Persona.Email;
                    Usuario.Sede = string.Format("{0} - {1} - {2}", user.Persona.Ubigeo.NombreDepartamento, user.Persona.Ubigeo.NombreProvincia, user.Persona.Ubigeo.NombreDistrito);
                    Usuario.Sede_Id = Usuario.SedePrincipal_Id = 0;
                    Usuario.EsSedePrincipal = false;
                    Usuario.Roles.Add(new RolDTe
                    {
                        Codigo = "PUBLIC",
                        Id = 0,
                        Nombre = "Persona externa"
                    });
                }
            }

        }

        public UsuarioLoginDTe GetUsuarioDetalles()
        {
            return Usuario;
        }

    }

}
