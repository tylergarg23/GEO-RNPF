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
    public class PlantacionTableRowDTe : PlantacionItemListDTe
    {
        public PlantacionTableRowDTe()
        {
            Personas = new List<PersonaTableRowDTe>();
            Detalles = new List<BloqueTableRowDTe>();            
        }

        [DataMember]
        public string NombrePredio { get; set; }

         [DataMember]        
         public int? SistemaCoordenada_Id { get; set; }

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
        public byte? TipoComunidad_Id { get; set; }

        [DataMember]
        public bool EsPropietario { get; set; }

        [DataMember]
        public bool EsInversionista { get; set; }

        [DataMember]
        public bool EsPosesionario { get; set; }

        [DataMember]
        [StringLength(50)]
        public string DocumentoCondicion { get; set; }

        [DataMember]
        public byte? TipoAutorizacion_Id { get; set; }

        [DataMember]
        [StringLength(50)]
        public string DocumentoContrato { get; set; }

        [DataMember]
        public DateTime? FechaRecepcionARFFS { get; set; }

        [DataMember]
        public decimal? CantidadTotalSuperficieMl { get; set; }

        [DataMember]
        public decimal? CantidadTotalSuperficieHa { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public byte SeccionAvance { get; set; }

        [DataMember]
        public short? Sede_Id { get; set; }

        [DataMember]
        public string Sede { get; set; }

        [DataMember]
        public short? SedePrincipal_Id { get; set; }

        [DataMember]
        public string LogoARFFS { get; set; }

        [DataMember]
        public DateTime? FechaRecepcion { get; set; }


        [DataMember]
        public List<PersonaTableRowDTe> Personas { get; set; }

        [DataMember]
        public List<DocumentoAnexoTableRowDTe> Anexos { get; set; }

        [DataMember]
        public List<BloqueTableRowDTe> Detalles { get; set; }

        [DataMember]
        public List<EspecieItemListDTe> Especies { get; set; }

        [DataMember]
        public string Autorizacion { get; set; }

        [DataMember]
        public int CantidadRevisiones { get; set; }

        [DataMember]
        public int CantidadHistoricos { get; set; }

        [DataMember]
        public int CantidadPersonas { get; set; }

        [DataMember]
        public int CantidadBloques { get; set; }

        [DataMember]
        public int CantidadAnexos { get; set; }

        [DataMember]
        public string CodigoQR { get; set; }

        [DataMember]
        public string LocalKey { get; set; }

        [DataMember]
        public DateTime? FechaFirma { get; set; } //TYLER 20/11/19 FECHA FIRMA
        [DataMember]
        public string NumeroPredio { get; set; } //TYLER 20/11/19 FECHA FIRMA
        [DataMember]
        public string EstadoGeo { get; set; } //TYLER 20/11/19 FECHA FIRMA

        [DataMember]
        public string CodigoDistrito { get; set; }  //TYLER 14/12/19 FECHA FIRMA

        [DataMember]
        public string NombreAutoridad { get; set; }  //TYLER 14/12/19 FECHA FIRMA

        [DataMember]
        public string ObjectId { get; set; }  //TYLER 14/12/19 FECHA FIRMA

    }
}