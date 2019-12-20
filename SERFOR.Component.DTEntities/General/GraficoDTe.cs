using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class GraficoDTe
    {
        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public int Valor { get; set; }

        [DataMember]
        public int Porcentaje { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string Highlight { get; set; }
       
    }
}
