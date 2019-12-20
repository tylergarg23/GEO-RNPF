using System;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Seguridad
{
    [DataContract]
    public class MiembroRolDTe 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Rol_Id { get; set; }
        [DataMember]
        public int Usuario_Id { get; set; }
        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public DateTime FechaCreacion { get; set; }

    }
}
