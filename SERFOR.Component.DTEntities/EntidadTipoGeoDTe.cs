using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class EntidadTipoGeoDTe: EntidadTipoDTe
    {
        [DataMember]
        public decimal LatitudMin { get; set; }
        [DataMember]
        public decimal LatitudMax { get; set; }
        [DataMember]
        public decimal LongitudMin { get; set; }
        [DataMember]
        public decimal LongitudMax { get; set; }
    }
}
