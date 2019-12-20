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
    
    public partial class PlantacionDetalleEspecieHistorico
    {
        public int Id { get; set; }
        public int PlantacionDetalleHist_Id { get; set; }
        public Nullable<int> Especie_Id { get; set; }
        public string NombreComun { get; set; }
        public Nullable<byte> UnidadMedida_Id { get; set; }
        public Nullable<int> TotalPlantado { get; set; }
        public string TipoPlantado { get; set; }
        public Nullable<decimal> ProduccionEstimada { get; set; }
        public Nullable<decimal> AlturaPromedio { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual Especie Especie { get; set; }
        public virtual PlantacionDetalleHistorico PlantacionDetalleHistorico { get; set; }
        public virtual UnidadMedida UnidadMedida { get; set; }
        public virtual TipoPlantacion TipoPlantacion { get; set; }
    }
}
