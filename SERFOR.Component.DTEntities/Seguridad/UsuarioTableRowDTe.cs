using SERFOR.Component.DTEntities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Seguridad
{
    [DataContract]
    public class UsuarioTableRowDTe : UsuarioLoginDTe
    {
        public UsuarioTableRowDTe()
        {
            Persona = new PersonaTableRowDTe();
            Roles = new List<RolDTe>();
            Directorio = new DirectorioDTe();
        }

        [DataMember]
        public PersonaTableRowDTe Persona { get; set; }

        [DataMember]
        public short Cargo_Id { get; set; }

        [DataMember]
        public bool EsResponsable { get; set; }

        [DataMember]
        public DirectorioDTe Directorio { get; set; }

        [DataMember]
        public int Directorio_Id { get; set; }

        [DataMember]
        public string Roles_Id { get; set; }

        [DataMember]
        public int CantidadUsuarios { get; set; }        

      
    }
}
