using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class PersonaTableRowDTe : PersonaItemListDTe
    {
        [DataMember] 
        [StringLength(150)]
        public string ApellidoPaterno { get; set; }

        [DataMember]
        [StringLength(150)]
        public string ApellidoMaterno { get; set; }

        [DataMember]
        public DateTime? FechaNacimiento { get; set; }

        [DataMember]
        public short Ubigeo_Id { get; set; }

        [DataMember]
        public string CodigoDepartamento { get; set; }

        [DataMember]
        public string CodigoProvincia { get; set; }

        [DataMember]
        [Required]
        public byte TipoDocumento_Id { get; set; }

        [DataMember]
        public int PersonaPlantacion_Id { get; set; }
        [DataMember]
        public string Rol { get; set; }
    }
}
