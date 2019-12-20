using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Seguridad
{
    [DataContract]
    public class UsuarioLoginDTe
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NombreUsuario { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string NombreCompleto { get; set; }        

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public DateTime? FechaInicio { get; set; }

        [DataMember]
        public DateTime? FechaFin { get; set; }

        [DataMember]
        public string Sede { get; set; }

        [DataMember]
        public string SedePrincipal { get; set; }

        [DataMember]
        public short Sede_Id { get; set; }
        [DataMember]
        public short SedePrincipal_Id { get; set; }

        [DataMember]
        public List<RolDTe> Roles { get; set; }

        [DataMember]
        public bool EsInvitado { get; set; }

        [DataMember]
        public bool EsSedePrincipal { get; set; }

        [DataMember]
        public string UsuarioModifica { get; set; }

    }
}
