using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class UbigeoDTe
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string CodigoDepartamento { get; set; }
        [DataMember]
        public string NombreDepartamento { get; set; }
        [DataMember]
        public string CodigoProvincia { get; set; }
        [DataMember]
        public string NombreProvincia { get; set; }
        [DataMember]
        public string CodigoDistrito { get; set; }
        [DataMember]
        public string NombreDistrito { get; set; }

    }
}
