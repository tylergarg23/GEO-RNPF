//TYLER 16.12.19 - INICIO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class PlantacionesFeatureLayer
    {
       
       [DataMember]
        public int ZONUTM { get; set; }
        [DataMember]
        public string NOMREL { get; set; }
        [DataMember]
        public string NOMTIT { get; set; }
        [DataMember]
        public string NOMCCN { get; set; }
        
        [DataMember]
        public string NUMRUC { get; set; }
        [DataMember]
        public byte TIPDOC { get; set; }
        [DataMember]
        public string NRODOC { get; set; }
        [DataMember]
        public string NOMDIS { get; set; }
        [DataMember]
        public string NOMPRO { get; set; }
        [DataMember]
        public string NOMDEP { get; set; }
        [DataMember]
        public int AUTFOR { get; set; }
        [DataMember]
        public decimal? FINALI { get; set; }
        [DataMember]
        public string ESPECI { get; set; }
        [DataMember]
        public decimal? SUPSIG { get; set; }
        [DataMember]
        public decimal? SUPAPR { get; set; }
        [DataMember]
        public double? COORES { get; set; }
        [DataMember]
        public double? COORNO { get; set; }
        [DataMember]
        public string FECPLA { get; set; }
        [DataMember]
        public string FUENTE { get; set; } //TYLER 19.12.19
        [DataMember]
        public string DOCREG { get; set; } //TYLER 19.12.19
        [DataMember]
        public string FECREG { get; set; } //TYLER 19.12.19
        [DataMember]
        public string OBSERV { get; set; } //TYLER 19.12.19
        [DataMember]
        //public string ORIGEN { get; set; } //TYLER 19.12.19
        //[DataMember]
        public string DOCASA { get; set; } //TYLER 19.12.19
        [DataMember]
        public string DOCTIT { get; set; } //TYLER 19.12.19
        [DataMember]
        public string DOCAUT { get; set; } //TYLER 19.12.19
        [DataMember]
        public string CONTRA { get; set; } //TYLER 19.12.19
        [DataMember]
        public string NUMREG { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        public byte? TIPCOM { get; set; }
    }
}
//TYLER 16.12.19 - FIN
