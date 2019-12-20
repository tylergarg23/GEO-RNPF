using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
   public class BloqueTableRowDTe : BloqueItemListDTe
    {
        [DataMember]
        public byte? MesEstablecimiento { get; set; }

        [DataMember]
        public string MesDescripcion { get; set; }

        [DataMember]
        public short? AnnioEstablecimiento { get; set; }

        [DataMember]
        public decimal? CantidadSuperficieHa { get; set; }

        [DataMember]
        public decimal? CantidadSuperficieMl { get; set; }

        [DataMember]
        public string UriStorageFileName { get; set; }
    }
}
