using SERFOR.Component.DTEntities.General;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class EspecieItemListDTe : EntidadBaseDTe<decimal>
    {        

        [DataMember]
        public byte? UnidadMedida_Id { get; set; }

        [DataMember]
        public string UnidadMedidaDescripcion { get; set; }

        [DataMember]
        public string UnidadMedidaSimbolo { get; set; }
        
        [DataMember]
        public int? Especie_Id { get; set; }

        [DataMember]
        public int EspecieCodigo { get; set; }

        [DataMember]
        public string EspecieNombre { get; set; }

        [DataMember]
        public string NombreComun { get; set; }

        [DataMember]
        public string TipoPlantado { get; set; }

        [DataMember]
        public string TipoPlantadoDesc { get; set; }

        [DataMember]
        public int? TotalPlantado { get; set; }

        [DataMember]
        public decimal? AlturaPromedio { get; set; }

        [DataMember]
        public decimal? ProduccionEstimada { get; set; }

        [DataMember]
        public int PlantacionDetalle_Id { get; set; }

        [DataMember]
        public BloqueTableRowDTe bloque { get; set; }
    }
}