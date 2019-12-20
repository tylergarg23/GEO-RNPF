using SERFOR.Component.DTEntities.Inventario;
using SERFOR.Component.InventarioCore.DataAccess;
using SERFOR.Component.Tools.DateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.InventarioCore.BusinessLogic.Facade
{
    public static class BrigadistaFacade
    {
        public static DTEntities.Inventario.Brigadista GetRowById(int id)
        {
            DTEntities.Inventario.Brigadista brigadista = null;

            using (var dbContext = new InventarioSchema())
            {
                var efObject = dbContext.Brigadista.Find(id);

                if (efObject != null)
                {
                    brigadista = new DTEntities.Inventario.Brigadista()
                    {
                        Id = efObject.Id,
                        IdPersona = efObject.IdPersona,
                        IdTipoDocumento = efObject.Persona.TipoDocumento_Id,
                        TipoDocumento = efObject.Persona.TipoDocumento.Descripcion,
                        NumeroDocumento = efObject.Persona.NumeroDocumento,
                        Nombres = efObject.Persona.Nombres,
                        ApellidoPaterno = efObject.Persona.ApellidoPaterno,
                        ApellidoMaterno = efObject.Persona.ApellidoMaterno,
                        NombreCompleto = (efObject.Persona.EsJuridica) ?
                                                        efObject.Persona.Nombres :
                                                        efObject.Persona.ApellidoPaterno + " " + efObject.Persona.ApellidoMaterno + ", " + efObject.Persona.Nombres,
                        IdTipoRolBrigada = efObject.IdTipoRolBrigada,
                        TipoRolBrigada = efObject.TipoRolBrigada.Nombre,
                        IdUbigeo = efObject.Persona.Ubigeo_Id,
                        Departamento = efObject.Persona.Ubigeo.NombreDepartamento,
                        Provincia = efObject.Persona.Ubigeo.NombreProvincia,
                        Distrito = efObject.Persona.Ubigeo.NombreDistrito,
                        EstadoCivil = efObject.Persona.EstadoCivil,
                        Sexo = efObject.Persona.Sexo,
                        Celular = efObject.Persona.Celular,
                        Telefono = efObject.Persona.Telefono,
                        Direccion = efObject.Persona.Direccion,                                                                        
                        Email = efObject.Persona.Email                        
                    };

                }
            }

            return brigadista;
        }

        public static IEnumerable<DTEntities.Inventario.Brigadista> GetAllByIdInventario(int idInventario)
        {

            IQueryable<DataAccess.Brigadista> query = null;
            IEnumerable<DTEntities.Inventario.Brigadista> brigadistas;

            var dbContext = new InventarioSchema();


            query = dbContext.Brigadista.Where(p => p.IdInventario == idInventario);


            brigadistas = query.Select(p => new DTEntities.Inventario.Brigadista()
            {
                Id = p.Id,
                IdPersona = p.IdPersona,
                IdTipoDocumento = p.Persona.TipoDocumento_Id,
                TipoDocumento = p.Persona.TipoDocumento.Descripcion,
                NumeroDocumento = p.Persona.NumeroDocumento,
                Nombres = p.Persona.Nombres,
                ApellidoPaterno = p.Persona.ApellidoPaterno,
                ApellidoMaterno = p.Persona.ApellidoMaterno,
                NombreCompleto = (p.Persona.EsJuridica) ?
                                                        p.Persona.Nombres :
                                                        p.Persona.ApellidoPaterno + " " + p.Persona.ApellidoMaterno + ", " + p.Persona.Nombres,
                IdTipoRolBrigada = p.IdTipoRolBrigada,
                TipoRolBrigada = p.TipoRolBrigada.Nombre,
                IdUbigeo = p.Persona.Ubigeo_Id,
                Departamento = p.Persona.Ubigeo.NombreDepartamento,
                Provincia = p.Persona.Ubigeo.NombreProvincia,
                Distrito = p.Persona.Ubigeo.NombreDistrito,
                EstadoCivil = p.Persona.EstadoCivil,
                Sexo = p.Persona.Sexo,
                Celular = p.Persona.Celular,
                Telefono = p.Persona.Telefono,
                Direccion = p.Persona.Direccion,
                Email = p.Persona.Email
            });

            dbContext.Brigadista.AsNoTracking();

            return brigadistas;
        }      

        public static IEnumerable<DTEntities.Inventario.Persona> GetBySearchText(string searchText)
        {
            IQueryable<DataAccess.Persona> query = null;
            IEnumerable<DTEntities.Inventario.Persona> personas;

            var dbContext = new InventarioSchema();

            if (!String.IsNullOrEmpty(searchText))
            {
                query = dbContext.Persona.Where(p => p.Nombres.Contains(searchText)
                                              || p.ApellidoPaterno.Contains(searchText)
                                              || p.ApellidoMaterno.Contains(searchText)
                                              || p.NumeroDocumento.Contains(searchText));

            }
            else
            {
                query = dbContext.Persona;
            }

            personas = query.Select(p => new DTEntities.Inventario.Persona()
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
                NumeroDocumento = p.TipoDocumento.Acronimo + ": " + p.NumeroDocumento,
                EstadoCivil = p.EstadoCivil,
                Sexo = (p.Sexo.Equals("M")) ? "Masculino" : "Femenino",                
            });

            dbContext.Persona.AsNoTracking();

            return personas;
        }

    }
}
