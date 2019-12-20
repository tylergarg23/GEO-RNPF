using SERFOR.Component.DTEntities.General;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class RevisionPlantacionTableRowDTe : BitacoraDetailDTe
    {

        [DataMember]
        public int Plantacion_Id { get; set; }

        [DataMember]
        public bool EsAprobado { get; set; }

        [DataMember]
        public string Rol { get; set; }

       // [DataMember]
        //public IEnumerable<BloqueCoordenadasDTE> Poligonos { get; set; }
    }
}
