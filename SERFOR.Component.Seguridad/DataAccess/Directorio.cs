//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SERFOR.Component.SeguridadCore.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Directorio
    {
        public int Id { get; set; }
        public Nullable<int> Persona_Id { get; set; }
        public Nullable<short> SedeATFFS_Id { get; set; }
        public short Cargo_Id { get; set; }
        public string Email { get; set; }
        public bool EsResponsable { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual Cargo Cargo { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual SedeAutoridadForestal SedeAutoridadForestal { get; set; }
    }
}
