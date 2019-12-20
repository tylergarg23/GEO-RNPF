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
    
    public partial class Interesado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Interesado()
        {
            this.InteresadoDetalle = new HashSet<InteresadoDetalle>();
            this.PersonaInteresado = new HashSet<PersonaInteresado>();
        }
    
        public int Id { get; set; }
        public string NumPartidaRegistral { get; set; }
        public string Anexo { get; set; }
        public Nullable<byte> TipoZona_Id { get; set; }
        public string NombreZona { get; set; }
        public Nullable<short> Ubigeo_Id { get; set; }
        public string MedioComunicacion { get; set; }
        public Nullable<System.TimeSpan> TiempoViaje { get; set; }
        public Nullable<System.TimeSpan> TiempoAcceso { get; set; }
        public Nullable<decimal> AreaDisponible { get; set; }
        public Nullable<decimal> AreaPlantacion { get; set; }
        public Nullable<decimal> AreaLibre { get; set; }
        public Nullable<decimal> AreaBosques { get; set; }
        public string Zona { get; set; }
        public string CoordenadaEsteUTM { get; set; }
        public string CoordenadaNorteUTM { get; set; }
        public Nullable<bool> TieneInteres { get; set; }
        public Nullable<byte> TipoAdquisicion_Id { get; set; }
        public string AdquisicionDescripcion { get; set; }
        public Nullable<byte> TipoComercializacion_Id { get; set; }
        public string ComercializacionDescripcion { get; set; }
        public Nullable<bool> RequiereAsistenciaTecnica { get; set; }
        public string TemaInteresAsistenciaTecnica { get; set; }
        public string RecursosFinancieros { get; set; }
        public string RecursosFinancierosDescripcion { get; set; }
        public Nullable<System.DateTime> FechaInscripcion { get; set; }
        public Nullable<short> Sede_Id { get; set; }
        public Nullable<short> SedePrincipal_Id { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual TipoAdquisicion TipoAdquisicion { get; set; }
        public virtual TipoComercializacion TipoComercializacion { get; set; }
        public virtual Ubigeo Ubigeo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteresadoDetalle> InteresadoDetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaInteresado> PersonaInteresado { get; set; }
    }
}
