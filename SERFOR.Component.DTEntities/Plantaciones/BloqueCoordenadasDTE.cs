using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class BloqueCoordenadasDTE : BloqueTableRowDTe
    {
        [DataMember]
        public List<CoordenadaTableRowDTe> Coordenadas { get; set; }
        public int Zona { get; set; } //TYLER 12.12.19
    }
}
