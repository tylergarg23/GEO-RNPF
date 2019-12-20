using SERFOR.Component.DTEntities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    public class InteresadoItemListDTe : EntidadBaseDTe<int>
    {
        [DataMember]
        public string NombreCompleto { get; set; }

        [DataMember]
        public string Documento { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string Departamento { get; set; }

        [DataMember]
        public string Provincia { get; set; }

        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public bool RequiereAsistenciaTecnica { get; set; }

        [DataMember]
        public bool TieneInteres { get; set; }

        [DataMember]
        public char RecursosFinancieros { get; set; }

        [DataMember]
        public DateTime? FechaInscripcion { get; set; }

    }
}
