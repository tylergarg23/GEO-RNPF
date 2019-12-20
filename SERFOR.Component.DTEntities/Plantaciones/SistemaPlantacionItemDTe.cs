using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class SistemaPlantacionItemDTe
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Tipo { get; set; }
    }
}
