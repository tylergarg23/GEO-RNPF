//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SERFOR.Component.InventarioCore.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class InventarioArbol
    {
        public int Id { get; set; }
        public int IdInventarioEspecie { get; set; }
        public int Indice { get; set; }
        public string Codigo { get; set; }
        public int CantidadRamas { get; set; }
        public decimal Azimut { get; set; }
        public decimal DistanciaAmarca { get; set; }
        public decimal DiametroAlturaPecho { get; set; }
        public decimal AlturaCopa { get; set; }
        public decimal AlturaTotal { get; set; }
        public bool EstaVivo { get; set; }
        public int IluminacionCopa { get; set; }
        public int FormaCopa { get; set; }
        public int CalidadFuste { get; set; }
        public int IdEstadoFitosanitario { get; set; }
        public string Observaciones { get; set; }
    
        public virtual EstadoFitosanitario EstadoFitosanitario { get; set; }
        public virtual InventarioEspecie InventarioEspecie { get; set; }
    }
}
