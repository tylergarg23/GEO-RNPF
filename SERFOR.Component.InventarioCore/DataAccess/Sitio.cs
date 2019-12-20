//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Sitio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sitio()
        {
            this.Inventario = new HashSet<Inventario>();
            this.Predio = new HashSet<Predio>();
            this.UnidadMuestreo = new HashSet<UnidadMuestreo>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public short IdUbigeo { get; set; }
        public string Comunidad { get; set; }
    
        public virtual Ubigeo Ubigeo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventario> Inventario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Predio> Predio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnidadMuestreo> UnidadMuestreo { get; set; }
    }
}
