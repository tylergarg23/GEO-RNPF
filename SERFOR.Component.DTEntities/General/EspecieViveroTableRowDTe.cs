using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class EspecieViveroTableRowDTe : EntidadBaseDTe<decimal>
    {
        [DataMember]
        public int Vivero_Id { get; set; }

        [DataMember]
        public short? Especie_Id { get; set; }

        [DataMember]
        public string NombreComun { get; set; }

        [DataMember]
        public string NombreCientifico { get; set; }

        [DataMember]
        public decimal? ProduccionEstimada { get; set; }

    }
}
