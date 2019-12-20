using SERFOR.Component.DTEntities.General;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class PlantacionItemListDTe : EntidadBaseDTe<int>
    {

        [DataMember]
        public string NumeroCertificado { get; set; }

        [DataMember]
        public string NombreZona { get; set; }

        [DataMember]
        public string Condicion { get; set; }

        [DataMember]
        public string Departamento { get; set; }

        [DataMember]
        public string Provincia { get; set; }

        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public decimal Area { get; set; }

        [DataMember]
        public bool AprobadoEspecialista { get; set; }

        [DataMember]
        public bool AprobadoDIR { get; set; }

        [DataMember]
        public bool AprobadoCatastro { get; set; }

        [DataMember]
        public int Numero { get; set; }

        [DataMember]
        public string Titular { get; set; }

        [DataMember]
        public string Brigadista { get; set; }

        [DataMember]
        public string Representante { get; set; }

        [DataMember]
        public string Especie { get; set; }

        [DataMember]
        public string Estado { get; set; }

    }

    [DataContract]
    public class Resultado
    {
        [DataMember]
        public int total { get; set; }

        [DataMember]
        public IEnumerable<PlantacionItemListDTe> rows { get; set; }
    }
}
