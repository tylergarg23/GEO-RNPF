using System;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class PersonaDetailDTe : PersonaItemListDTe
    {
        [DataMember]
        public DateTime FechaNacimiento { get; set; }
        [DataMember]
        public string RegistroLargo { get; set; }
        [DataMember]
        public string RegistroCorto { get; set; }
        [DataMember]
        public string ModificacionLargo { get; set; }
        [DataMember]
        public string ModificacionCorto { get; set; }
        [DataMember]
        public int ProgresoRegistro { get; set; }
        [DataMember]
        public string ProgresoNivel { get; set; }
    }
}
