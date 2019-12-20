using System;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public abstract class EntidadBaseDTe<IdType>
    {
        [DataMember]
        public IdType Id { get; set; }
        [DataMember]
        public Nullable<bool> Activo { get; set; }
        [DataMember]
        public string UsuarioCreacion { get; set; }
        [DataMember]
        public DateTime FechaCreacion { get; set; }
        [DataMember]
        public string UsuarioModificacion { get; set; }
        [DataMember]
        public DateTime FechaModificacion { get; set; }

        [DataMember]
        public CoordinateDetailDTe CoordenadaGeografica { get; set; }


    }

}
