using System;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.ListasExcel
{
    [DataContract]
   public class LicenciaCazaDTe
    {
        [DataMember]
        public short Id { get; set; }
        [DataMember]
        public string AutoridadForestal { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        public DateTime FechaCaducidad { get; set; }
        [DataMember]
        public string TipoDocumento { get; set; }
        [DataMember]
        public string NumeroDocumento { get; set; }
        [DataMember]
        public string ApellidoPaterno { get; set; }
        [DataMember]
        public string ApellidoMaterno { get; set; }
        [DataMember]
        public string Nombres { get; set; }
        [DataMember]
        public string CausalesExtinsion { get; set; }
        [DataMember]
        public string NumeroResolucion { get; set; }
        [DataMember]
        public DateTime FechaResolucion { get; set; }
    }
}
