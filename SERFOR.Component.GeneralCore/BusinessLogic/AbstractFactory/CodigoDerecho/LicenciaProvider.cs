namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public enum TipoLicencia
    {
        CONDUCTORES_CERTIFICADOS_DE_CAZA_DEPORTIVA,
        PARA_EJERCER_COMO_ESPECIALISTA,
        PARA_EJERCER_REGENCIA,
        PARA_CAZA_DEPORTIVA,
        PARA_CAZA_O_CAPTURA_COMERCIAL,
        PARA_CETRERIA
    }

    public class LicenciaProvider : DerechoProvider
    {
        public LicenciaProvider(TipoLicencia tipo, short sedeId, int ubigeoId)
            : base(sedeId, ubigeoId)
        {
            CodigoDerecho = "LIC";
            NombreDerecho = "Licencia";
            switch (tipo)
            {
                case TipoLicencia.CONDUCTORES_CERTIFICADOS_DE_CAZA_DEPORTIVA:
                    CodigoRecursoActividad = "CCD";
                    NombreRecursoActividad = "Conductores Certificados de Caza Deportiva.";
                    break;
                case TipoLicencia.PARA_EJERCER_COMO_ESPECIALISTA:
                    CodigoRecursoActividad = "ES";
                    NombreRecursoActividad = "Para ejercer como Especialista.";
                    break;
                case TipoLicencia.PARA_EJERCER_REGENCIA:
                    CodigoRecursoActividad = "RE";
                    NombreRecursoActividad = "Para ejercer la Regencia.";
                    break;
                case TipoLicencia.PARA_CAZA_DEPORTIVA:
                    CodigoRecursoActividad = "CD";
                    NombreRecursoActividad = "Para la Caza Deportiva.";
                    break;
                case TipoLicencia.PARA_CAZA_O_CAPTURA_COMERCIAL:
                    CodigoRecursoActividad = "CC";
                    NombreRecursoActividad = "Para la Caza o Captura comercial.";
                    break;
                case TipoLicencia.PARA_CETRERIA:
                    CodigoRecursoActividad = "CE";
                    NombreRecursoActividad = "Para la Cetrería.";
                    break;
            }
        }

    }
}