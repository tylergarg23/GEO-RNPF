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
    
    public partial class PlantacionDetalleHistorico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlantacionDetalleHistorico()
        {
            this.PlantacionDetalleVerticesHistorico = new HashSet<PlantacionDetalleVerticesHistorico>();
        }
    
        public int Id { get; set; }
        public int PlantacionHist_Id { get; set; }
        public byte SistemaPlantacion_Id { get; set; }
        public Nullable<short> Especie_Id { get; set; }
        public string NombreComun { get; set; }
        public Nullable<byte> Finalidad_Id { get; set; }
        public byte UnidadMedida_Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<decimal> AreaPredio { get; set; }
        public Nullable<byte> MesEstablecimiento { get; set; }
        public Nullable<short> AnnioEstablecimiento { get; set; }
        public Nullable<decimal> CantidadSuperficieHa { get; set; }
        public Nullable<decimal> CantidadSuperficieMl { get; set; }
        public Nullable<short> TotalPlantado { get; set; }
        public string TipoPlantado { get; set; }
        public Nullable<decimal> ProduccionEstimada { get; set; }
        public string CoordenadaEsteUTM { get; set; }
        public string CoordenadaNorteUTM { get; set; }
        public Nullable<decimal> AlturaPromedio { get; set; }
        public string UriStorageFileName { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string UsuarioHistorico { get; set; }
        public System.DateTime FechaCreacionHistorica { get; set; }
    
        public virtual EspecieForestal EspecieForestal { get; set; }
        public virtual FinalidadProducto FinalidadProducto { get; set; }
        public virtual PlantacionHistorico PlantacionHistorico { get; set; }
        public virtual SistemaPlantacion SistemaPlantacion { get; set; }
        public virtual UnidadMedida UnidadMedida { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlantacionDetalleVerticesHistorico> PlantacionDetalleVerticesHistorico { get; set; }
    }
}
