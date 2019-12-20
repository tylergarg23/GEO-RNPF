using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Seguridad
{
    [DataContract]
    public class RolDTe 
    {
        [DataMember]
        public byte Id { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Activo { get; set; }

    }
}
