//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SERFOR.Component.GeneralCore.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubigeo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ubigeo()
        {
            this.Persona = new HashSet<Persona>();
        }
    
        public short Id { get; set; }
        public string Codigo { get; set; }
        public string CodigoDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public string CodigoProvincia { get; set; }
        public string NombreProvincia { get; set; }
        public string CodigoDistrito { get; set; }
        public string NombreDistrito { get; set; }
        public string Region { get; set; }
        public Nullable<decimal> LatitudDec { get; set; }
        public Nullable<decimal> LongitudDec { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public Nullable<byte> ARFFS_Id { get; set; }
    
        public virtual AutoridadForestal AutoridadForestal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Persona> Persona { get; set; }
    }
}
