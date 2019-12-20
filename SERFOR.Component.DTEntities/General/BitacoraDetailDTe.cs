using System;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{

    [DataContract]
    public class BitacoraDetailDTe : EntidadBaseDTe<int>
    {
        [DataMember]
        public DateTime FechaEvento { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string DireccionIP { get; set; }
    }
}
