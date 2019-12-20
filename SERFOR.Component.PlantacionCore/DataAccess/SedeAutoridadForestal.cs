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
    
    public partial class SedeAutoridadForestal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SedeAutoridadForestal()
        {
            this.Plantacion = new HashSet<Plantacion>();
        }
    
        public short Id { get; set; }
        public byte AT_Id { get; set; }
        public short TipoSede_Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public Nullable<decimal> LatitudDec { get; set; }
        public Nullable<decimal> LongitudDec { get; set; }
        public string CodigoUA { get; set; }
        public bool Activo { get; set; }
    
        public virtual AutoridadForestal AutoridadForestal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plantacion> Plantacion { get; set; }
    }
}
