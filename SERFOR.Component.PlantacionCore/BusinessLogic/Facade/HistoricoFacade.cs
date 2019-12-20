using EG = SERFOR.Component.DTEntities.General;
using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class HistoricoFacade
    {
        public static bool InsertByPlantacionId(int id, string usuario)
        {
            bool result = false;
            using (var context = new PlantacionSchema())
            {
                var output = new ObjectParameter("Result", typeof(bool));
                context.sp_ReplicarRegistroPlantacion(id, usuario, output);

                result = Convert.ToBoolean(output.Value);
            }

            return result;
        }

        public static IEnumerable<PlantacionItemListDTe> GetByPlantacionId(int id)
        {

            var plantaciones = new List<PlantacionItemListDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var efObjects = dbContext.PlantacionHistoricoSet.Where(r => r.Plantacion_Id == id);

                if (efObjects.Count() > 0)
                {
                    foreach (var p in efObjects)
                    {
                        var newPlantacion = new PlantacionItemListDTe()
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
                            Area = (p.Area.HasValue) ? p.Area.Value : decimal.MinValue,
                            NombreZona = (p.TipoZona_Id.HasValue) ? p.TipoZona.Descripcion + " " + p.NombreZona : "",
                            Estado = (p.Estado=="A")?"ANULACIÓN":((p.Estado == "I")?"INSCRIPCIÓN":"MODIFICACIÓN")
                        };

                        plantaciones.Add(newPlantacion);

                    }

                }
            }

            return plantaciones;
        }

        public static PlantacionTableRowDTe GetRowById(int id)
        {
            PlantacionTableRowDTe plantacion = null;

            using (var dbContext = new PlantacionSchema())
            {
                var efObject = dbContext.PlantacionHistoricoSet.Find(id);

                if (efObject != null)
                {
                    plantacion = new PlantacionTableRowDTe()
                    {
                        Id = efObject.Id,
                        NombrePredio = efObject.NombrePredio,                        
                        Area = (efObject.Area.HasValue) ? efObject.Area.Value : decimal.Zero,
                        NumeroCertificado = efObject.NumeroCertificado,

                        TipoZona_Id = efObject.TipoZona_Id,
                        NombreZona = efObject.NombreZona,
                        EsPropietario = efObject.EsPropietario,
                        EsPosesionario = efObject.EsPosesionario,                       
                        EsInversionista = efObject.EsInversionista,
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
                        FechaRecepcion = efObject.FechaRecepcion,
                        UsuarioCreacion = efObject.UsuarioCreacion,
                        FechaCreacion = efObject.FechaCreacion,
                        CantidadRevisiones = efObject.RevisionesRegistroPlantacionesHistorico.Count
                    };

                    if (efObject.Ubigeo != null && efObject.Ubigeo_Id > 0)
                    {
                        plantacion.Ubigeo_Id = efObject.Ubigeo_Id;
                        plantacion.Departamento = efObject.Ubigeo.NombreDepartamento;
                        plantacion.Provincia = efObject.Ubigeo.NombreProvincia;
                        plantacion.Distrito = efObject.Ubigeo.NombreDistrito;
                        plantacion.CodigoDepartamento = efObject.Ubigeo.CodigoDepartamento;
                        plantacion.CodigoProvincia = efObject.Ubigeo.CodigoProvincia;
                    }

                    if (efObject.PersonaPlantacionHistorico.Count() > 0)
                    {
                        foreach (var persona in efObject.PersonaPlantacionHistorico)
                        {
                            plantacion.Personas.Add(new EG.PersonaTableRowDTe()
                            {
                                Id = persona.Persona.Id,
                                PersonaPlantacion_Id = persona.Id,
                                Rol = PersonaFacade.GetRolDescripcion(persona.Rol),
                                NombreCompleto = persona.Persona.Nombres,
                                Departamento = persona.Ubigeo.NombreDepartamento,
                                Provincia = persona.Ubigeo.NombreProvincia,
                                Distrito = persona.Ubigeo.NombreDistrito,
                                Celular = persona.Celular,
                                Telefono = persona.Telefono,
                                Direccion = persona.Direccion,           
                                EsJuridica = persona.Persona.EsJuridica,
                                EstadoCivil = persona.Persona.EstadoCivil,
                                Sexo = persona.Persona.Sexo,
                                Documento = persona.Persona.NumeroDocumento,
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

                    if (efObject.PlantacionDetalleHistorico.Count() > 0)
                    {
                        foreach (var bloque in efObject.PlantacionDetalleHistorico)
                        {
                            var newBloque = new BloqueTableRowDTe()
                            {
                                Id = bloque.Id,
                                Nombre = bloque.Nombre,
                                AnnioEstablecimiento = bloque.AnnioEstablecimiento,
                                MesEstablecimiento = bloque.MesEstablecimiento,
                                AreaPredio = bloque.AreaPredio,
                                CoordenadaEsteUTM = bloque.CoordenadaEsteUTM,
                                CoordenadaNorteUTM = bloque.CoordenadaNorteUTM,
                                Finalidad_Id = bloque.Finalidad_Id,
                                FinalidadDescripcion = bloque.FinalidadProducto.Descripcion,                                
                                SistemaPlantacion_Id = bloque.SistemaPlantacion_Id,
                                SistemaPlantacionDescripcion = bloque.SistemaPlantacion.Descripcion,
                                Plantacion_Id = bloque.PlantacionHist_Id,
                                CantidadCoordenadasRegistradas = bloque.PlantacionDetalleVerticesHistorico.Count,
                                CantidadSuperficieHa = bloque.CantidadSuperficieHa,
                                CantidadSuperficieMl = bloque.CantidadSuperficieMl
                            };
                            if (bloque.PlantacionDetalleEspecieHistorico.Count() > 0)
                            {
                                foreach (var especie in bloque.PlantacionDetalleEspecieHistorico)
                                {
                                    var newEspecie = new EspecieItemListDTe()
                                    {
                                        Id = bloque.Id,
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
                                    newBloque.Especies.Add(newEspecie);
                                }
                            }                             

                            plantacion.Detalles.Add(newBloque);

                        }

                    }
                }
            }

            return plantacion;
        }


    }
}
