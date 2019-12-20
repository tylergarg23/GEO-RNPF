using SERFOR.Component.DTEntities.General;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class PlantacionDepartamentoDTe : EntidadBaseDTe<byte>
    {
        [DataMember]
        public string CodigoDepartamento { get; set; }

        [DataMember]
        public string NombreDepartamento { get; set; }

        [DataMember]
        public int CantidadRNPs { get; set; }

        [DataMember]
        public int CantidadVigentes { get; set; }

        [DataMember]
        public int CantidadAnuladas { get; set; }

        [DataMember]
        public int CantidadSolicitudes { get; set; }

        [DataMember]
        public int CantidadConNumero { get; set; }

        [DataMember]
        public int CantidadSinNumero { get; set; }

        [DataMember]
        public int CantidadBloques { get; set; }

        [DataMember]
        public int CantidadAprobadosDIR { get; set; }

        [DataMember]
        public int CantidadAprobadosCatastro { get; set; }

        [DataMember]
        public int CantidadEsPropietario { get; set; }

        [DataMember]
        public int CantidadEsInversionista { get; set; }

        [DataMember]
        public int CantidadEsPosesionario { get; set; }        

        [DataMember]
        public decimal SuperficieTotalM2 { get; set; }

        [DataMember]
        public decimal SuperficieTotalHa { get; set; }

        [DataMember]
        public decimal AreaTotalM2 { get; set; }

        [DataMember]
        public decimal AreaTotalHa { get; set; }


    }
}
