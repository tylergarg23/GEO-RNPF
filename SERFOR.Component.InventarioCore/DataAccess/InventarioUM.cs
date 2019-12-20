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
    
    public partial class InventarioUM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InventarioUM()
        {
            this.InventarioEspecie = new HashSet<InventarioEspecie>();
        }
    
        public int Id { get; set; }
        public int IdInventario { get; set; }
        public int IdUnidadMuestreo { get; set; }
        public int IdTipoAccesibilidad { get; set; }
        public string OtraAccesibilidad { get; set; }
        public Nullable<int> IdConduccion { get; set; }
        public Nullable<System.DateTime> FechaInicioMediciones { get; set; }
        public Nullable<System.DateTime> FechaFinalMediciones { get; set; }
    
        public virtual Conduccion Conduccion { get; set; }
        public virtual Inventario Inventario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventarioEspecie> InventarioEspecie { get; set; }
        public virtual TipoAccesibilidad TipoAccesibilidad { get; set; }
        public virtual UnidadMuestreo UnidadMuestreo { get; set; }
    }
}
