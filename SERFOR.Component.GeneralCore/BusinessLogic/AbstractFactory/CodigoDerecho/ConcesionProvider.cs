namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public enum TipoConcesion
    {
        CONSERVACION,
        ECOTURISMO,
        FAUNA,
        MADERABLE,
        PLANTACIONES,
        PRODUCTOS_FORESTALES_DIFERENTES_A_MADERA
    }

    public class ConcesionProvider : DerechoProvider
    {
        public ConcesionProvider(TipoConcesion tipo, short sedeId, int ubigeoId)
            : base(sedeId, ubigeoId)
        {
            CodigoDerecho = "CTO";
            NombreDerecho = "Contrato";
            switch (tipo)
            {
                case TipoConcesion.CONSERVACION:
                    CodigoRecursoActividad = "CON";
                    NombreRecursoActividad = "Conservación.";
                    break;
                case TipoConcesion.ECOTURISMO:
                    CodigoRecursoActividad = "ECO";
                    NombreRecursoActividad = "Ecoturismo.";
                    break;
                case TipoConcesion.FAUNA:
                    CodigoRecursoActividad = "FAU";
                    NombreRecursoActividad = "Fauna.";
                    break;
                case TipoConcesion.MADERABLE:
                    CodigoRecursoActividad = "MAD";
                    NombreRecursoActividad = "Maderable.";
                    break;
                case TipoConcesion.PLANTACIONES:
                    CodigoRecursoActividad = "PLT";
                    NombreRecursoActividad = "Plantaciones.";
                    break;
                case TipoConcesion.PRODUCTOS_FORESTALES_DIFERENTES_A_MADERA:
                    CodigoRecursoActividad = "PFDM";
                    NombreRecursoActividad = "Productos Forestales diferentes a la madera.";
                    break;
            }
        }
    }
}