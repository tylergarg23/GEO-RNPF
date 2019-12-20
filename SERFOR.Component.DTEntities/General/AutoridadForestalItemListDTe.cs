using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class AutoridadForestalItemListDTe : EntidadBaseDTe<short>
    {

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public bool EsGobiernoRegional { get; set; }

        [DataMember]
        public string RutaLogo { get; set; }
        [DataMember]
        public string NombreContacto { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public string Telefono { get; set; }
    }
}
