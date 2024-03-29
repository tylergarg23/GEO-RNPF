//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class PlantacionHistorico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlantacionHistorico()
        {
            this.PersonaPlantacionHistorico = new HashSet<PersonaPlantacionHistorico>();
            this.RevisionesRegistroPlantacionesHistorico = new HashSet<RevisionesRegistroPlantacionesHistorico>();
            this.PlantacionDetalleHistorico = new HashSet<PlantacionDetalleHistorico>();
        }
    
        public int Id { get; set; }
        public int Plantacion_Id { get; set; }
        public string NumeroCertificado { get; set; }
        public string NombrePredio { get; set; }
        public Nullable<decimal> Area { get; set; }
        public string Zona { get; set; }
        public string Datum { get; set; }
        public string CoordenadaNorteUTM { get; set; }
        public string CoordenadaEsteUTM { get; set; }
        public Nullable<short> Ubigeo_Id { get; set; }
        public Nullable<byte> TipoZona_Id { get; set; }
        public string NombreZona { get; set; }
        public bool EsPropietario { get; set; }
        public string DocumentoCondicion { get; set; }
        public Nullable<byte> TipoAutorizacion_Id { get; set; }
        public string DocumentoContrato { get; set; }
        public Nullable<System.DateTime> FechaRecepcionARFFS { get; set; }
        public bool AprobadoEspecialista { get; set; }
        public bool AprobadoDIR { get; set; }
        public bool AprobadoCatastro { get; set; }
        public Nullable<decimal> CantidadTotalSuperficieMl { get; set; }
        public Nullable<decimal> CantidadTotalSuperficieHa { get; set; }
        public string Observaciones { get; set; }
        public Nullable<System.DateTime> FechaRecepcion { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioHistorico { get; set; }
        public System.DateTime FechaCreacionHistorica { get; set; }
        public bool EsPosesionario { get; set; }
        public bool EsInversionista { get; set; }
        public string Estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaPlantacionHistorico> PersonaPlantacionHistorico { get; set; }
        public virtual Plantacion Plantacion { get; set; }
        public virtual TipoAutorizacion TipoAutorizacion { get; set; }
        public virtual TipoZona TipoZona { get; set; }
        public virtual Ubigeo Ubigeo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RevisionesRegistroPlantacionesHistorico> RevisionesRegistroPlantacionesHistorico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlantacionDetalleHistorico> PlantacionDetalleHistorico { get; set; }
    }
}
