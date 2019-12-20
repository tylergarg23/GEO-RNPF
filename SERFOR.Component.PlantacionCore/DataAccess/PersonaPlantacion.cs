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
    
    public partial class PersonaPlantacion
    {
        public int Id { get; set; }
        public Nullable<int> Persona_Id { get; set; }
        public Nullable<int> Plantacion_Id { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public short Ubigeo_Id { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<byte> TipoZona_Id { get; set; }
        public string NombreZona { get; set; }
    
        public virtual Persona Persona { get; set; }
        public virtual Ubigeo Ubigeo { get; set; }
        public virtual Plantacion Plantacion { get; set; }
        public virtual TipoZona TipoZona { get; set; }
    }
}