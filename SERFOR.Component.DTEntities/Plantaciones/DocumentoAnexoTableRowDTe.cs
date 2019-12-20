using SERFOR.Component.DTEntities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class DocumentoAnexoTableRowDTe : ArchivoDTe
    {
        [DataMember]
        public byte Secuencia { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public int Plantacion_Id { get; set; }

        [DataMember]
        public short Anexo_Id { get; set; }
    }
}
