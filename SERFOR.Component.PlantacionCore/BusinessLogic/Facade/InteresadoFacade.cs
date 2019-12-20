using EG = SERFOR.Component.DTEntities.General;
using SERFOR.Component.PlantacionCore.DataAccess;
using SERFOR.Component.DTEntities.Plantaciones;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class InteresadoFacade
    {
        public static IEnumerable<InteresadoItemListDTe> GeByFilter(short id, bool esPrincipal)
        {

            var interesados = new List<InteresadoItemListDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                InteresadoItemListDTe interesado;

                IQueryable<Interesado> query = null;

                if (id == 0)
                {
                    query = dbContext.Interesado;
                }
                else
                {
                    if (esPrincipal)
                    {
                        query = dbContext.Interesado.Where(p => p.SedePrincipal_Id == id);
                    }
                    else
                    {
                        query = dbContext.Interesado.Where(p => p.Sede_Id == id);
                    }
                }

                foreach (var efObject in query)
                {
                    interesado = new InteresadoItemListDTe()
                    {
                        Id = efObject.Id,
                        Departamento = efObject.Ubigeo.NombreDepartamento,
                        Provincia = efObject.Ubigeo.NombreProvincia,
                        Distrito = efObject.Ubigeo.NombreDistrito,
                        TieneInteres = efObject.TieneInteres.Value,
                        RequiereAsistenciaTecnica = efObject.RequiereAsistenciaTecnica.Value,
                        RecursosFinancieros = Convert.ToChar(efObject.RecursosFinancieros),
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaCreacion = efObject.FechaRegistro
                    };

                    if (efObject.PersonaInteresado.Where(p => p.Rol == "I").Count() > 0)
                    {
                        var involucrado = efObject.PersonaInteresado.Where(p => p.Rol == "I").First();

                        interesado.NombreCompleto = (involucrado.Persona.EsJuridica) ?
                                                        involucrado.Persona.Nombres :
                                                        involucrado.Persona.ApellidoPaterno + " " + involucrado.Persona.ApellidoMaterno + ", " + involucrado.Persona.Nombres;
                        interesado.Email = involucrado.Email;
                        interesado.Telefono = involucrado.Celular;

                        if (!string.IsNullOrEmpty(involucrado.Telefono)) { interesado.Telefono += "/" + involucrado.Telefono; }

                        interesado.Documento = involucrado.Persona.TipoDocumento.Acronimo + " " + involucrado.Persona.NumeroDocumento;

                    }

                    interesados.Add(interesado);
                }
            }

            return interesados;
        }

        public static bool DeleteById(int id)
        {

            bool exito = true;

            using (var dbContext = new PlantacionSchema())
            {
                try
                {
                    var efObject = dbContext.Interesado.Find(id);

                    if (efObject != null)
                    {

                        dbContext.Interesado.Remove(efObject);
                        dbContext.SaveChanges();

                    }
                }
                catch { exito = false; }
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
