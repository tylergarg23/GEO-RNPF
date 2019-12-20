using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class CoordinateDetailDTe 
    {
        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string NombreLocation { get; set; }

        [DataMember]
        public double Latitud { get; set; }

        [DataMember]
        public double Longitud { get; set; }

        [DataMember]
        public string Resumen { get; set; }

        [DataMember]
        public string HtmlInfo { get; set; }

    }
}
