using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class EntidadTipoDTe
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SufijoAcronimo { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
    }
}
