using SERFOR.Component.DTEntities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class InteresadoTableRowDTe : InteresadoItemListDTe
    {
        public InteresadoTableRowDTe()
        {
            Personas = new List<PersonaTableRowDTe>();
            Detalles = new List<BloqueTableRowDTe>();
        }

        [DataMember]
        [StringLength(50)]
        public string Zona { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Datum { get; set; }

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
        public byte? TipoZona_Id { get; set; }
             
        [DataMember]
        public short? Sede_Id { get; set; }

        [DataMember]
        public short? SedePrincipal_Id { get; set; }

        [DataMember]
        public List<PersonaTableRowDTe> Personas { get; set; }

        [DataMember]
        public List<BloqueTableRowDTe> Detalles { get; set; }

        [DataMember]
        public int CantidadPersonas { get; set; }

        [DataMember]
        public int CantidadBloques { get; set; }

      
    }
}
