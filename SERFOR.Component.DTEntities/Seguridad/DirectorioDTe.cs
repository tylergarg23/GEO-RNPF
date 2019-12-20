using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Seguridad
{
    [DataContract]
    public class DirectorioDTe 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Persona_Id { get; set; }
        [DataMember]
        public short? Sede_Id { get; set; }
        [DataMember]
        public short Cargo_Id { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool EsResponsable { get; set; }
        [DataMember]
        public bool Activo { get; set; }
        [DataMember]
        public string UsuarioModifica { get; set; }

    }
}
