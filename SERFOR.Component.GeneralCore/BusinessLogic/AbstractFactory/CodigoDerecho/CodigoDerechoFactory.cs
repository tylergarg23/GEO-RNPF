using System;

namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public class CodigoDerechoFactory
    {

        private string CodigoSede { get; set; }

        private string CodigoDepartamento { get; set; }

      
        public virtual RegistroProvider CreateRegistroProvider(TipoRegistro tipo, short annio, short sedeId, int ubigeoId)
        {
            var derecho = new RegistroProvider(tipo, sedeId, ubigeoId)
            {
                Annio = annio
            }; 

            return derecho;
        }
    }
   
}
