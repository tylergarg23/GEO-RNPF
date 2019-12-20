﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SERFOR.Component.InventarioCore.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InventarioSchema : DbContext
    {
        public InventarioSchema()
            : base("name=InventarioSchema")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EspecieForestal> EspecieForestal { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<TipoZona> TipoZona { get; set; }
        public virtual DbSet<Ubigeo> Ubigeo { get; set; }
        public virtual DbSet<Brigadista> Brigadista { get; set; }
        public virtual DbSet<Conduccion> Conduccion { get; set; }
        public virtual DbSet<EstadoFitosanitario> EstadoFitosanitario { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<InventarioArbol> InventarioArbol { get; set; }
        public virtual DbSet<InventarioEspecie> InventarioEspecie { get; set; }
        public virtual DbSet<InventarioUM> InventarioUM { get; set; }
        public virtual DbSet<Predio> Predio { get; set; }
        public virtual DbSet<PuntoCentral> PuntoCentral { get; set; }
        public virtual DbSet<Sitio> Sitio { get; set; }
        public virtual DbSet<TipoAccesibilidad> TipoAccesibilidad { get; set; }
        public virtual DbSet<TipoConductor> TipoConductor { get; set; }
        public virtual DbSet<TipoRolBrigada> TipoRolBrigada { get; set; }
        public virtual DbSet<TipoUso> TipoUso { get; set; }
        public virtual DbSet<UnidadMuestreo> UnidadMuestreo { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }

        public System.Data.Entity.DbSet<SERFOR.Component.DTEntities.Inventario.Brigadista> Brigadistas { get; set; }
    }
}