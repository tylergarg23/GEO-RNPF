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
    public class ViveroItemListDTe : EntidadBaseDTe<int>
    {

        [DataMember]
        [StringLength(250)]
        public string Direccion { get; set; }

        [DataMember]
        public string Departamento { get; set; }

        [DataMember]
        public string Provincia { get; set; }

        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public string DatosCoordenada { get; set; }

        [DataMember]
        public decimal TotalAreaAlmacigo { get; set; }

        [DataMember]
        public decimal TotalAreaRepique { get; set; }

        [DataMember]
        public int TotalEspecies { get; set; }

    }
}

