using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SERFOR.Component.DTEntities.Inventario
{
    [DataContract]
    public class Persona
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string TipoPersona { get; set; }

        [DataMember]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener como mínimo 3 letras y máximo 150.")]
        public string NombreCompleto { get; set; }

        [DataMember]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener como mínimo 3 letras y máximo 50.")]
        public string Nombres { get; set; }

        [DataMember]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido paterno debe tener como mínimo 3 letras y máximo 50.")]
        public string ApellidoPaterno { get; set; }

        [DataMember]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido materno debe tener como mínimo 3 letras y máximo 50.")]
        public string ApellidoMaterno { get; set; }

        [DataMember]
        [StringLength(250)]
        public string Direccion { get; set; }

        [DataMember]
        public byte? TipoZona { get; set; }

        [DataMember]
        public string Zona { get; set; }
        
        [DataMember]
        public byte IdTipoDocumento { get; set; }

        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string Sexo { get; set; }

        [DataMember]
        public string EstadoCivil { get; set; }

        [DataMember]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Telefono { get; set; }

        [DataMember]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Celular { get; set; }

        [DataMember]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Dirección de correo electrónico inválida")]
        public string Email { get; set; }

        [DataMember]
        [Required]
        public bool EsJuridica { get; set; }

        [DataMember]
        public string Departamento { get; set; }


        [DataMember]
        public string Provincia { get; set; }


        [DataMember]
        public string Distrito { get; set; }

        [DataMember]
        public int IdUbigeo { get; set; }
        
        [DataMember]
        public DateTime? FechaNacimiento { get; set; }

        [DataMember]
        public Boolean? EsTrabajador { get; set; }

        [DataMember]
        public Boolean? EsAdministrado { get; set; }
    }
}
