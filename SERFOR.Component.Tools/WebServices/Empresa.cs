using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.Tools.WebServices
{
    public class Empresa
    {
        public Empresa()
        {
        }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }

        public string TipoVia { get; set; }
        public string NombreVia { get; set; }
        public string NumeroDireccion { get; set; }
        public string TipoZona { get; set; }
        public string NombreZona { get; set; }
        public string ReferenciaDireccion { get; set; }
        public string Direccion { get; set; }

        public string Rubro { get; set; }
        public string Anio { get; set; }
    }
}
