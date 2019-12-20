using SERFOR.Component.DTEntities.General;
using SERFOR.Component.DTEntities.Plantaciones;
using SERFOR.Component.PlantacionCore.DataAccess;
using SERFOR.Component.Tools.GeoConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.PlantacionCore.BusinessLogic.Facade
{
    public static class ReportesFacade
    {
        public static List<FactPlantacionTableDTe> GetResumenPlantacion()
        {
            var fact_plantaciones = new List<FactPlantacionTableDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var db_FactPlantaciones = dbContext.Fact_Plantaciones;

                if (db_FactPlantaciones.Count() > 0)
                {
                    FactPlantacionTableDTe fact_plantacion;
                    foreach (var db_FactPlantacion in db_FactPlantaciones)
                    {
                        fact_plantacion = new FactPlantacionTableDTe()
                        {
                            Ubigeo= db_FactPlantacion.Ubigeo,
                            Departamento=db_FactPlantacion.Departamento,
                            Provincia = db_FactPlantacion.Provincia,
                            Distrito = db_FactPlantacion.Distrito,
                            Zona = db_FactPlantacion.Zona,
                            Sede = db_FactPlantacion.Sede,
                            ARFFS = db_FactPlantacion.ARFFS,
                            NumeroDocumento = db_FactPlantacion.NumeroDocumento,
                            Titular = db_FactPlantacion.Titular,
                            Estado = db_FactPlantacion.Estado,
                            SistemaPlantacion = db_FactPlantacion.SistemaPlantacion,
                            Finalidad = db_FactPlantacion.Finalidad,
                            AnioPlantacion = db_FactPlantacion.AnioPlantacion,
                            MesPlantacion = db_FactPlantacion.MesPlantacion,
                            TipoPlantacion = db_FactPlantacion.Tipo_Plantacion,
                            UnidadMedida = db_FactPlantacion.UnidadMedida,
                            CodigoEspecie = db_FactPlantacion.CodigoEspecie,
                            NombreEspecie = db_FactPlantacion.NombreEspecie,
                            NombreComun = db_FactPlantacion.NombreComun,
                            Area_Ha = db_FactPlantacion.Area_Ha.Value,
                            Lineal_m = db_FactPlantacion.Lineal_m.Value,
                            NroPlantado = db_FactPlantacion.NroPlantado.Value,
                            ProduccionEstimada = db_FactPlantacion.ProduccionEstimada.Value
                        };
                        fact_plantaciones.Add(fact_plantacion);
                    }
                }
                dbContext.Fact_Plantaciones.AsNoTracking();                
            }            
            return fact_plantaciones;
        }
        public static List<FactPlantacionTableDTe> GetResumenPlantacionProduccion()
        {
            var fact_plantaciones = new List<FactPlantacionTableDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var db_FactPlantacionesProd = dbContext.Fact_PlantacionesProduccion;

                if (db_FactPlantacionesProd.Count() > 0)
                {
                    FactPlantacionTableDTe fact_plantacion;
                    foreach (var db_FactPlantacion in db_FactPlantacionesProd)
                    {
                        fact_plantacion = new FactPlantacionTableDTe()
                        {
                            Ubigeo = db_FactPlantacion.Ubigeo,
                            Departamento = db_FactPlantacion.Departamento,
                            Provincia = db_FactPlantacion.Provincia,
                            Distrito = db_FactPlantacion.Distrito,
                            Zona = db_FactPlantacion.Zona,
                            Sede = db_FactPlantacion.SEDE,
                            ARFFS = db_FactPlantacion.ARFFS,                                                        
                            Estado = db_FactPlantacion.Estado,
                            SistemaPlantacion = db_FactPlantacion.SistemaPlantacion,
                            Finalidad = db_FactPlantacion.Finalidad,
                            AnioPlantacion = db_FactPlantacion.AnioPlantacion,
                            MesPlantacion = db_FactPlantacion.MesPlantacion,
                            TipoPlantacion = db_FactPlantacion.Tipo_Plantacion,
                            UnidadMedida = db_FactPlantacion.UnidadMedida,                            
                            NombreEspecie = db_FactPlantacion.NombreEspecie,
                            NombreComun = db_FactPlantacion.NombreComun,                            
                            NroPlantado = db_FactPlantacion.NroPlantado.Value,
                            ProduccionEstimada = db_FactPlantacion.ProduccionEstimada.Value
                        };
                        fact_plantaciones.Add(fact_plantacion);
                    }
                }
                dbContext.Fact_Plantaciones.AsNoTracking();
            }
            return fact_plantaciones;
        }
        public static List<FactPlantacionEspacioTableDTe> GetResumenPlantacionEspacio()
        {
            var fact_plantacionesEspacio = new List<FactPlantacionEspacioTableDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var db_FactPlantacionesEspacio = dbContext.Fact_PlantacionesEspacio;

                if (db_FactPlantacionesEspacio.Count() > 0)
                {
                    FactPlantacionEspacioTableDTe fact_plantacionespacio;
                    foreach (var db_FactPlantacionespacio in db_FactPlantacionesEspacio)
                    {
                        fact_plantacionespacio = new FactPlantacionEspacioTableDTe()
                        {
                            Ubigeo = db_FactPlantacionespacio.Ubigeo,
                            Departamento = db_FactPlantacionespacio.Departamento,
                            Provincia = db_FactPlantacionespacio.Provincia,
                            Distrito = db_FactPlantacionespacio.Distrito,
                            Zona = db_FactPlantacionespacio.Zona,
                            Sede = db_FactPlantacionespacio.Sede,
                            ARFFS = db_FactPlantacionespacio.ARFFS,
                            Estado = db_FactPlantacionespacio.Estado,
                            SistemaPlantacion = db_FactPlantacionespacio.SistemaPlantacion,
                            Finalidad = db_FactPlantacionespacio.Finalidad,
                            AnioPlantacion = db_FactPlantacionespacio.AnioPlantacion,
                            MesPlantacion = db_FactPlantacionespacio.MesPlantacion,
                            Area_Ha = db_FactPlantacionespacio.Area_Ha.Value,
                            Lineal_m2 = db_FactPlantacionespacio.Lineal_m2.Value,
                        };
                        fact_plantacionesEspacio.Add(fact_plantacionespacio);
                    }
                }
                dbContext.Fact_PlantacionesEspacio.AsNoTracking();
            }
            return fact_plantacionesEspacio;
        }
        public static ReporteResumenDTe GetSummaryByFilter(bool soloAprobados, char condicion, string codigoDepartamento)
        {
            var dataReporte = new ReporteResumenDTe();

            // Consulta de plantaciones forestales
            var plantaciones = GetApprovedByDepartment(soloAprobados, codigoDepartamento.Trim());
            if (plantaciones != null && plantaciones.Count() > 0)
            {
                switch (condicion)
                {
                    case 'P':
                        dataReporte.PlantacionesDetalle = plantaciones.Where(p => p.EsPropietario);
                        dataReporte.TotalPlantaciones = plantaciones.Count(p => p.EsPropietario);
                        break;
                    case 'I':
                        dataReporte.PlantacionesDetalle = plantaciones.Where(p => p.EsInversionista);
                        dataReporte.TotalPlantaciones = plantaciones.Count(p => p.EsInversionista);
                        break;
                    case 'S':
                        dataReporte.PlantacionesDetalle = plantaciones.Where(p => p.EsPosesionario);
                        dataReporte.TotalPlantaciones = plantaciones.Count(p => p.EsPosesionario);
                        break;
                    case 'T':
                    default:
                        dataReporte.PlantacionesDetalle = plantaciones;
                        dataReporte.TotalPlantaciones = plantaciones.Count();
                        break;
                }
            }


            // Consulta de informacion de plantaciones por departamento
            if (dataReporte.PlantacionesDetalle != null && dataReporte.PlantacionesDetalle.Count() > 0)
            {
                var itemsPlantacion = dataReporte.PlantacionesDetalle.Where(p => p.Ubigeo_Id != null && p.Ubigeo_Id > 0);
                dataReporte.TotalPlantacionesUbigeo = itemsPlantacion.Count();
                dataReporte.TotalArea = itemsPlantacion.Sum(p => (p.CantidadTotalSuperficieHa == null?0: p.CantidadTotalSuperficieHa.Value) );
                
                using (var dbContext = new PlantacionSchema())
                {

                    var departamentos = dbContext.UbigeoSet.GroupBy(u => new { u.CodigoDepartamento, u.NombreDepartamento }).Select(u => u.FirstOrDefault());

                    var dataDepartamentos = new List<PlantacionDepartamentoDTe>();
                    var dataGraficoDepartamentos = new List<GraficoDTe>();
                    foreach (var departamento in departamentos)
                    {
                        var query = itemsPlantacion.Where(p => p.CodigoDepartamento == departamento.CodigoDepartamento);

                        // Solo se mostrara la informacion de los departamentos con plantaciones
                        if (query.Count() > 0)
                        {
                            var dataGraficoDepartamento = new GraficoDTe()
                            {
                                Etiqueta = departamento.NombreDepartamento,
                                Valor = query.Count(),
                                Porcentaje = query.Count() * 100 / dataReporte.TotalPlantacionesUbigeo
                            };

                            dataGraficoDepartamentos.Add(dataGraficoDepartamento);
                        }

                    }

                    dataReporte.GraficoDepartamentos = dataGraficoDepartamentos;
                }

                dataReporte.GraficoCondicion = new List<GraficoDTe>() {
                new GraficoDTe() { Etiqueta = "Propietarios", Valor = dataReporte.PlantacionesDetalle.Count(p=> p.EsPropietario), Porcentaje = itemsPlantacion.Count(p=> p.EsPropietario)*100/dataReporte.TotalPlantacionesUbigeo },
                new GraficoDTe() { Etiqueta = "Inversionistas", Valor = dataReporte.PlantacionesDetalle.Count(p=> p.EsInversionista), Porcentaje = itemsPlantacion.Count(p=> p.EsInversionista)*100/dataReporte.TotalPlantacionesUbigeo},
                new GraficoDTe() { Etiqueta = "Posesionarios", Valor = dataReporte.PlantacionesDetalle.Count(p=> p.EsPosesionario), Porcentaje = itemsPlantacion.Count(p=> p.EsPosesionario)*100/dataReporte.TotalPlantacionesUbigeo }
            };

                dataReporte.ListaKPIs = new List<GraficoDTe>() {
                    new GraficoDTe() { Etiqueta = string.Format("/{0} aprobadas", dataReporte.TotalPlantaciones), Valor = dataReporte.PlantacionesDetalle.Count(p => p.Estado!=""), Porcentaje = dataReporte.PlantacionesDetalle.Count(p=> p.Estado!="") * 100/dataReporte.TotalPlantacionesUbigeo },
                    new GraficoDTe() { Etiqueta = string.Format("/{0} pendientes aprob.", dataReporte.TotalPlantaciones), Valor = dataReporte.PlantacionesDetalle.Count(p=> p.Estado==""), Porcentaje = dataReporte.PlantacionesDetalle.Count(p=> p.Estado=="") * 100/dataReporte.TotalPlantacionesUbigeo },

                    new GraficoDTe() { Etiqueta = string.Format("/{0} por Esp. ATFFS", dataReporte.TotalPlantaciones), Valor = dataReporte.PlantacionesDetalle.Count(p=> p.AprobadoEspecialista), Porcentaje = dataReporte.PlantacionesDetalle.Count(p=> p.AprobadoEspecialista) * 100/dataReporte.TotalPlantacionesUbigeo },
                    new GraficoDTe() { Etiqueta = string.Format("/{0} por DIR", dataReporte.TotalPlantaciones), Valor = dataReporte.PlantacionesDetalle.Count(p=> p.AprobadoDIR ), Porcentaje = dataReporte.PlantacionesDetalle.Count(p=> p.AprobadoDIR) * 100/dataReporte.TotalPlantacionesUbigeo },
                    new GraficoDTe() { Etiqueta = string.Format("/{0} por Castro", dataReporte.TotalPlantaciones), Valor = dataReporte.PlantacionesDetalle.Count(p=> p.AprobadoCatastro), Porcentaje = dataReporte.PlantacionesDetalle.Count(p=> p.AprobadoCatastro) * 100/dataReporte.TotalPlantacionesUbigeo },

                    new GraficoDTe() { Etiqueta = string.Format("/{0} con autorización reg.", dataReporte.TotalPlantaciones), Valor = dataReporte.PlantacionesDetalle.Count(p=> p.TipoAutorizacion_Id != null && p.TipoAutorizacion_Id > 0), Porcentaje = dataReporte.PlantacionesDetalle.Count(p=> p.TipoAutorizacion_Id != null && p.TipoAutorizacion_Id > 0) * 100/dataReporte.TotalPlantacionesUbigeo },
            };
            }
            return dataReporte;
        }

        public static IEnumerable<PlantacionTableRowDTe> GetApprovedByDepartment(bool soloAprobados, string codigoDepartamento)
        {
            var plantaciones = new List<PlantacionTableRowDTe>();

            using (var dbContext = new PlantacionSchema())
            {

                IQueryable<Plantacion> query = dbContext.PlantacionSet;

                if (soloAprobados)
                {
                    query = query.Where(p => p.AprobadoCatastro);
                }

                if (!string.IsNullOrEmpty(codigoDepartamento))
                {
                    query = query.Where(p => p.Ubigeo.CodigoDepartamento == codigoDepartamento);
                }

                if (query.Count() > 0)
                {
                    int zona = 0;
                    var converter = new GeoUTMConverter();
                    PlantacionTableRowDTe plantacion;

                    foreach (var plantacionEF in query)
                    {

                        plantacion = new PlantacionTableRowDTe()
                        {
                            Id = plantacionEF.Id,
                            NombrePredio = plantacionEF.NombrePredio,
                            Area = (plantacionEF.Area.HasValue) ? plantacionEF.Area.Value : decimal.Zero,
                            NumeroCertificado = plantacionEF.NumeroCertificado,

                            TipoZona_Id = plantacionEF.TipoZona_Id,
                            NombreZona = plantacionEF.NombreZona,
                            EsPropietario = plantacionEF.EsPropietario,
                            EsInversionista = plantacionEF.EsInversionista,
                            EsPosesionario = plantacionEF.EsPosesionario,
                            Condicion = (plantacionEF.EsPropietario) ? "Propietario" : (plantacionEF.EsInversionista) ? "Inversionista": "Posesionario",

                            TipoAutorizacion_Id = plantacionEF.TipoAutorizacion_Id,
                            DocumentoCondicion = plantacionEF.DocumentoCondicion,
                            DocumentoContrato = plantacionEF.DocumentoContrato,
                            FechaRecepcionARFFS = plantacionEF.FechaRecepcionARFFS,
                            AprobadoEspecialista = plantacionEF.AprobadoEspecialista,
                            AprobadoDIR = plantacionEF.AprobadoDIR,
                            AprobadoCatastro = plantacionEF.AprobadoCatastro,
                            CantidadTotalSuperficieMl = plantacionEF.CantidadTotalSuperficieMl,
                            CantidadTotalSuperficieHa = plantacionEF.CantidadTotalSuperficieHa,
                            Observaciones = plantacionEF.Observaciones,
                            SeccionAvance = plantacionEF.SeccionAvance,
                            Sede_Id = plantacionEF.Sede_Id,
                            SedePrincipal_Id = plantacionEF.SedePrincipal_Id,
                            FechaRecepcion = plantacionEF.FechaRecepcion,
                            UsuarioCreacion = plantacionEF.UsuarioCreacion,
                            FechaCreacion = plantacionEF.FechaCreacion,
                            UsuarioModificacion = plantacionEF.UsuarioModificacion,
                            FechaModificacion = plantacionEF.FechaModificacion,
                            CantidadRevisiones = plantacionEF.RevisionesRegistroPlantaciones.Count,
                            CantidadHistoricos = plantacionEF.PlantacionHistorico.Count,
                            CantidadPersonas = plantacionEF.PersonaPlantacion.Count,
                            CantidadBloques = plantacionEF.PlantacionDetalle.Count,
                            CantidadAnexos = plantacionEF.AnexosPlantacion.Count,
                            Estado = plantacionEF.Estado
                        };

                        if (plantacionEF.TipoAutorizacion_Id != null && plantacionEF.TipoAutorizacion_Id > 0)
                        {
                            plantacion.Autorizacion = plantacionEF.TipoAutorizacion.Descripcion;
                        }

                        if (plantacionEF.Ubigeo != null && plantacionEF.Ubigeo_Id > 0)
                        {
                            plantacion.Ubigeo_Id = plantacionEF.Ubigeo_Id;
                            plantacion.Departamento = plantacionEF.Ubigeo.NombreDepartamento;
                            plantacion.Provincia = plantacionEF.Ubigeo.NombreProvincia;
                            plantacion.Distrito = plantacionEF.Ubigeo.NombreDistrito;
                            plantacion.CodigoDepartamento = plantacionEF.Ubigeo.CodigoDepartamento;
                            plantacion.CodigoProvincia = plantacionEF.Ubigeo.CodigoProvincia;
                        }

                        zona = (!string.IsNullOrEmpty(plantacionEF.Zona)) ? Convert.ToInt32(plantacionEF.Zona) : 0;

                        if (!string.IsNullOrEmpty(plantacionEF.CoordenadaEsteUTM) && !string.IsNullOrEmpty(plantacionEF.CoordenadaNorteUTM))
                        {
                            converter.ToLatLon(Convert.ToDouble(plantacionEF.CoordenadaEsteUTM), Convert.ToDouble(plantacionEF.CoordenadaNorteUTM), zona, GeoUTMConverter.Hemisphere.Southern);
                            plantacion.CoordenadaGeografica = new CoordinateDetailDTe()
                            {
                                Nombre = plantacionEF.NumeroCertificado,
                                NombreLocation = string.Format("Plantacion {0} - {1}", plantacionEF.Id, plantacionEF.NombrePredio),
                                Latitud = converter.Latitude,
                                Longitud = converter.Longitude
                            };
                        }

                        plantaciones.Add(plantacion);
                    }
                }

                dbContext.PlantacionSet.AsNoTracking();
            }

            return plantaciones;
        }

        public static IEnumerable<PlantacionDepartamentoDTe> GetDataDepartments()
        {
            var dataDepartamentos = new List<PlantacionDepartamentoDTe>();

            using (var dbContext = new PlantacionSchema())
            {
                var plantaciones = dbContext.PlantacionSet.Where(p => p.Ubigeo_Id != null && p.Ubigeo_Id > 0);
                var departamentos = dbContext.UbigeoSet.GroupBy(u => new { u.CodigoDepartamento, u.NombreDepartamento }).Select(u => u.FirstOrDefault());


                foreach (var departamento in departamentos)
                {
                    var query = plantaciones.Where(p => p.Ubigeo.CodigoDepartamento == departamento.CodigoDepartamento);
                    var dataDepartamento = new PlantacionDepartamentoDTe()
                    {
                        CodigoDepartamento = departamento.CodigoDepartamento,
                        NombreDepartamento = departamento.NombreDepartamento
                    };

                    if (query.Count() > 0)
                    {
                        dataDepartamento.CantidadRNPs = query.Where(p => p.Estado != null).Count();
                        dataDepartamento.CantidadVigentes = query.Where(p => p.Estado == "R").Count();
                        dataDepartamento.CantidadAnuladas = query.Where(p => p.Estado == "A").Count();

                        dataDepartamento.CantidadEsPropietario = query.Where(p => p.Estado == "R" && p.EsPropietario).Count();
                        dataDepartamento.CantidadEsInversionista = query.Where(p => p.Estado == "R" && p.EsInversionista).Count();
                        dataDepartamento.CantidadEsPosesionario = query.Where(p => p.Estado == "R" && p.EsPosesionario).Count();
                        if (dataDepartamento.CantidadVigentes > 0)
                        {
                            dataDepartamento.CantidadBloques = query.Where(n => n.Estado == "R").Sum(p => p.PlantacionDetalle.Count);
                            dataDepartamento.SuperficieTotalM2 = query.Where(n => n.Estado == "R").Sum(p => p.PlantacionDetalle.Sum(z => z.CantidadSuperficieMl) ?? 0);
                            dataDepartamento.SuperficieTotalHa = query.Where(n => n.Estado == "R").Sum(p => p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa) ?? 0);
                        }
                        dataDepartamento.CantidadAprobadosDIR = query.Where(p => p.Estado == "R" && p.AprobadoDIR).Count();
                        dataDepartamento.CantidadAprobadosCatastro = query.Where(p => p.Estado == "R" && p.AprobadoCatastro).Count();
                        


                        dataDepartamento.CantidadSolicitudes = query.Where(p => p.Estado == null).Count();
                        dataDepartamento.CantidadConNumero = query.Where(p => p.Estado == null && !string.IsNullOrEmpty(p.NumeroCertificado)).Count();
                        dataDepartamento.CantidadSinNumero = query.Where(p => p.Estado == null && string.IsNullOrEmpty(p.NumeroCertificado)).Count();
                        if (dataDepartamento.CantidadSolicitudes > 0)
                        {
                            dataDepartamento.AreaTotalM2 = query.Where(n => n.Estado == null).Sum(p => p.PlantacionDetalle.Sum(z => z.CantidadSuperficieMl) ?? 0);
                            dataDepartamento.AreaTotalHa = query.Where(n => n.Estado == null).Sum(p => p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa) ?? 0);
                        }
                        
                        //dataDepartamento.TotalArea = query.Sum(p =>(p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).HasValue) ? p.PlantacionDetalle.Sum(d => d.CantidadSuperficieHa).Value : 0);      


                    }


                    if (departamento.LatitudDec.HasValue && departamento.LongitudDec.HasValue)
                    {
                        dataDepartamento.CoordenadaGeografica = new CoordinateDetailDTe()
                        {
                            Nombre = departamento.NombreDepartamento,
                            NombreLocation = string.Format("Departamento: {0} - {1}", departamento.CodigoDepartamento, departamento.NombreDepartamento),
                            Latitud = Convert.ToDouble(departamento.LatitudDec.Value),
                            Longitud = Convert.ToDouble(departamento.LongitudDec.Value)
                        };
                    }

                    dataDepartamentos.Add(dataDepartamento);
                }

                dbContext.PlantacionSet.AsNoTracking();
            }

            return dataDepartamentos.OrderBy(d => d.CodigoDepartamento);
        }
    }
}
