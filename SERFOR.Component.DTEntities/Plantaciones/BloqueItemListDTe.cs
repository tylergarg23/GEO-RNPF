using SERFOR.Component.DTEntities.General;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class BloqueItemListDTe : EntidadBaseDTe<decimal>
    {
        [DataMember]
        public int Plantacion_Id { get; set; }

        [DataMember]
        public byte SistemaPlantacion_Id { get; set; }

        [DataMember]
        public string SistemaPlantacionDescripcion { get; set; }

        [DataMember]
        public string SistemaPlantacionTipo { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public decimal? AreaPredio { get; set; }

        [DataMember]
        public byte? Finalidad_Id { get; set; }

        [DataMember]
        public string FinalidadDescripcion { get; set; }

        [DataMember]
        public int? SistemaCoordenada_Id { get; set; }

        [DataMember]
        public decimal? LatitudMin { get; set; }

        [DataMember]
        public decimal? LatitudMax { get; set; }

        [DataMember]
        public decimal? LongitudMin { get; set; }

        [DataMember]
        public decimal? LongitudMax { get; set; }

        [DataMember]
        public string CoordenadaEsteUTM { get; set; }

        [DataMember]
        public string CoordenadaNorteUTM { get; set; }        

        [DataMember]
        public int CantidadCoordenadasRegistradas { get; set; }

        [DataMember]
        public List<EspecieItemListDTe> Especies { get; set; }


    }
}