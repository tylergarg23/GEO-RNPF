using SERFOR.Component.DTEntities.General;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Especie
{
    [DataContract]
    public class EspecieTableRowDTe : EntidadBaseDTe<int>
    {
        
        [DataMember]
        public string Nombre { get; set; }
        public string NombreConsultado { get; set; }
        public string NombreComun { get; set; }

    }
}