using System;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.General
{
    [DataContract]
    public class ArchivoDTe : EntidadBaseDTe<decimal>
    {
        [DataMember]
        public string Ruta { get; set; }

        [DataMember]
        public string NombreArchivo { get; set; }

        [DataMember]
        public byte[] BytesArchivo { get; set; }

        [DataMember]
        public Int64 Tamanio { get; set; }
    }

}
