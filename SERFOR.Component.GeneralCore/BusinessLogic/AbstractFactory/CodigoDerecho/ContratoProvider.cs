namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public enum TipoContrato
    {
        ACCESO_A_RECURSOS_GENETICOS,
        CESION_EN_USO_EN_BOSQUES_RESIDUALES_O_REMANENTES,
        CESION_EN_USO_PARA_SISTEMAS_AGROFORESTALES
    }

    public class ContratoProvider : DerechoProvider
    {
        public ContratoProvider(TipoContrato tipo, short sedeId, int ubigeoId)
            : base(sedeId, ubigeoId)
        {
            CodigoDerecho = "CTO";
            NombreDerecho = "Contrato";
            switch (tipo)
            {
                case TipoContrato.ACCESO_A_RECURSOS_GENETICOS:
                    CodigoRecursoActividad = "ARG";
                    NombreRecursoActividad = "Acceso a recursos Genéticos.";
                    break;
                case TipoContrato.CESION_EN_USO_EN_BOSQUES_RESIDUALES_O_REMANENTES:
                    CodigoRecursoActividad = "BRR";
                    NombreRecursoActividad = "Cesión en uso en Bosques Residuales o Remanentes.";
                    break;
                case TipoContrato.CESION_EN_USO_PARA_SISTEMAS_AGROFORESTALES:
                    CodigoRecursoActividad = "SAG";
                    NombreRecursoActividad = "Cesión en uso para Sistemas Agroforestales.";
                    break;
            }
        }
    }
}