using SERFOR.Component.DTEntities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class FactPlantacionEspacioTableDTe
    {
        public FactPlantacionEspacioTableDTe()
        {            
        }

        [DataMember]
        public string Ubigeo { get; set; }

        [DataMember]
        public string Departamento { get; set; }

        [DataMember]
        public string Provincia { get; set; }

        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public string Zona { get; set; }

        [DataMember]
        public string Sede { get; set; }

        [DataMember]
        public string ARFFS { get; set; }


        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string SistemaPlantacion { get; set; }

        [DataMember]
        public string Finalidad { get; set; }

        [DataMember]
        public short AnioPlantacion { get; set; }

        [DataMember]
        public byte MesPlantacion { get; set; }

        [DataMember]
        public decimal Area_Ha { get; set; }

        [DataMember]
        public decimal Lineal_m2 { get; set; }
        
    }
}
