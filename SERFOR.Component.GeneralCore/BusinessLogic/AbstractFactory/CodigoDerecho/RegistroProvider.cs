namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public enum TipoRegistro
    {
        PLANTACIONES,
        ASERRADORES_PORTATILES
    }

    public class RegistroProvider : DerechoProvider
    {
        public RegistroProvider(TipoRegistro tipo, short sedeId, int ubigeoId) 
            : base(sedeId, ubigeoId)
        {
            CodigoDerecho = "REG";
            NombreDerecho = "Registro";
            switch (tipo)
            {
                case TipoRegistro.PLANTACIONES:
                    CodigoRecursoActividad = "PLT";
                    NombreRecursoActividad = "Plantaciones";
                    break;
                case TipoRegistro.ASERRADORES_PORTATILES:
                    CodigoRecursoActividad = "ASP";
                    NombreRecursoActividad = "Aserradores Portátiles";
                    break;
            }
        }

    }
}