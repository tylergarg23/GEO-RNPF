﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SERFOR.Component.PlantacionCore.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PlantacionSchema : DbContext
    {
        public PlantacionSchema()
            : base("name=PlantacionSchema")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FinalidadProducto> FinalidadProducto { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Plantacion> Plantacion { get; set; }
        public virtual DbSet<PlantacionDetalleVertices> PlantacionDetalleVertices { get; set; }
        public virtual DbSet<SistemaPlantacion> SistemaPlantacion { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<Ubigeo> Ubigeo { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
        public virtual DbSet<TipoZona> TipoZona { get; set; }
        public virtual DbSet<PersonaPlantacion> PersonaPlantacion { get; set; }
        public virtual DbSet<PlantacionHistorico> PlantacionHistorico { get; set; }
        public virtual DbSet<RevisionesRegistroPlantaciones> RevisionesRegistroPlantaciones { get; set; }
        public virtual DbSet<RevisionesRegistroPlantacionesHistorico> RevisionesRegistroPlantacionesHistorico { get; set; }
        public virtual DbSet<PersonaPlantacionHistorico> PersonaPlantacionHistorico { get; set; }
        public virtual DbSet<FamiliaEspecie> FamiliaEspecie { get; set; }
        public virtual DbSet<PlantacionDetalleVerticesHistorico> PlantacionDetalleVerticesHistorico { get; set; }
        public virtual DbSet<TipoAutorizacion> TipoAutorizacion { get; set; }
        public virtual DbSet<EspecieForestal> EspecieForestal { get; set; }
        public virtual DbSet<PlantacionDetalle> PlantacionDetalle { get; set; }
        public virtual DbSet<PlantacionDetalleHistorico> PlantacionDetalleHistorico { get; set; }
        public virtual DbSet<Anexos> Anexos { get; set; }
        public virtual DbSet<AnexosPlantacion> AnexosPlantacion { get; set; }
        public virtual DbSet<Interesado> Interesado { get; set; }
        public virtual DbSet<InteresadoDetalle> InteresadoDetalle { get; set; }
        public virtual DbSet<PersonaInteresado> PersonaInteresado { get; set; }
        public virtual DbSet<TipoAdquisicion> TipoAdquisicion { get; set; }
        public virtual DbSet<TipoComercializacion> TipoComercializacion { get; set; }
        public virtual DbSet<SistemaCoordenada> SistemaCoordenada { get; set; }
        public virtual DbSet<TipoComunidad> TipoComunidad { get; set; }
    
        public virtual int ReplicarRegistroPlantacion(Nullable<int> plantacion_Id, string usuario, ObjectParameter result)
        {
            var plantacion_IdParameter = plantacion_Id.HasValue ?
                new ObjectParameter("Plantacion_Id", plantacion_Id) :
                new ObjectParameter("Plantacion_Id", typeof(int));
    
            var usuarioParameter = usuario != null ?
                new ObjectParameter("Usuario", usuario) :
                new ObjectParameter("Usuario", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReplicarRegistroPlantacion", plantacion_IdParameter, usuarioParameter, result);
        }
    }
}