using SERFOR.Component.DTEntities.General;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class ReporteResumenDTe
    {
        [DataMember]
        public IEnumerable<PlantacionTableRowDTe> PlantacionesDetalle { get; set; }

        [DataMember]
        public IEnumerable<GraficoDTe> GraficoDepartamentos { get; set; }

        [DataMember]
        public IEnumerable<GraficoDTe> GraficoCondicion { get; set; }

        [DataMember]
        public IEnumerable<GraficoDTe> ListaKPIs { get; set; }

        [DataMember]
        public int TotalPlantaciones { get; set; }

        [DataMember]
        public decimal TotalArea { get; set; }

        [DataMember]
        public int TotalPlantacionesUbigeo { get; set; }

    }
}
