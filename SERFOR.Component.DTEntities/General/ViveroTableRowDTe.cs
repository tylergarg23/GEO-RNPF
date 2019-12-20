using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class ViveroTableRowDTe : ViveroItemListDTe
    {
        public ViveroTableRowDTe()
        {
            Personas = new List<PersonaTableRowDTe>();
            Detalles = new List<EspecieViveroTableRowDTe>();
        }

        [DataMember]
        [StringLength(50)]
        public string Zona { get; set; }

        [DataMember]
        [StringLength(15)]
        public string CoordenadaNorteUTM { get; set; }

        [DataMember]
        [StringLength(15)]
        public string CoordenadaEsteUTM { get; set; }

        [DataMember]
        public short? Ubigeo_Id { get; set; }

        [DataMember]
        public string CodigoDepartamento { get; set; }

        [DataMember]
        public string CodigoProvincia { get; set; }

        [DataMember]
        public string FuenteAbastecimientoAgua { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public List<PersonaTableRowDTe> Personas { get; set; }

        [DataMember]
        public List<EspecieViveroTableRowDTe> Detalles { get; set; }

    }
}
