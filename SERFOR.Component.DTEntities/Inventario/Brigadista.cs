using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Inventario
{
    [DataContract]
    public class Brigadista : Persona
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdInventario { get; set; }

        [DataMember]
        public int IdPersona { get; set; }

        [DataMember]
        public int? IdTipoRolBrigada { get; set; }

        [DataMember]
        public string TipoRolBrigada { get; set; }
    }
}
