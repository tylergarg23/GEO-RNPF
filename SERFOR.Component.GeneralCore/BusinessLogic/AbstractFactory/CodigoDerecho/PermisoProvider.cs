namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public enum TipoPermiso
    {
        SERVICIOS_DE_ECOSISTEMAS_FORESTALES,
        FORESTAL_MADERABLE_PREDIOS_PRIVADOS,
        FORESTAL_MADERABLE_PARA_COMUNIDADES,
        FORESTAL_MADERABLE_PARA_COMUNIDADES_POSESIONARIAS,
        PRODUCTOS_DIFERENTES_A_LA_MADERA_PREDIOS_PRIVADOS,
        PRODUCTOS_DIFERENTES_A_LA_MADERA_PARA_COMUNIDADES,
        PRODUCTOS_DIFERENTES_A_LA_MADERA_PARA_COMUNIDADES_POSESIONARIAS,
        FAUNA_PREDIOS_PRIVADOS,
        FAUNA_PARA_COMUNIDADES,
        FAUNA_PARA_COMUNIDADES_POSESIONARIAS
    }

    public class PermisoProvider : DerechoProvider
    {
        public PermisoProvider(TipoPermiso tipo, short sedeId, int ubigeoId)
            : base(sedeId, ubigeoId)
        {
            CodigoDerecho = "PER";
            NombreDerecho = "Permisos";
            switch (tipo)
            {
                case TipoPermiso.SERVICIOS_DE_ECOSISTEMAS_FORESTALES:
                    CodigoRecursoActividad = "SEF";
                    NombreRecursoActividad = "Aprovechamiento de Servicios de Ecosistemas Forestales";
                    break;
                case TipoPermiso.FORESTAL_MADERABLE_PARA_COMUNIDADES_POSESIONARIAS:
                    CodigoRecursoActividad = "FMCP";
                    NombreRecursoActividad = "Aprovechamiento Forestal maderable para Comunidades Posesionarias en Proceso de reconocimiento, titulación o ampliación.";
                    break;
                case TipoPermiso.PRODUCTOS_DIFERENTES_A_LA_MADERA_PARA_COMUNIDADES_POSESIONARIAS:
                    CodigoRecursoActividad = "FDCP";
                    NombreRecursoActividad = "Aprovechamiento Forestal de Productos diferentes a la madera para Comunidades Posesionarias en Proceso de reconocimiento, titulación o ampliación.";
                    break;
                case TipoPermiso.FAUNA_PARA_COMUNIDADES_POSESIONARIAS:
                    CodigoRecursoActividad = "FACP";
                    NombreRecursoActividad = "Aprovechamiento de Fauna para Comunidades Posesionarias en Proceso de reconocimiento, titulación o ampliación.";
                    break;
                case TipoPermiso.FORESTAL_MADERABLE_PARA_COMUNIDADES:
                    CodigoRecursoActividad = "FMC";
                    NombreRecursoActividad = "Aprovechamiento Forestal maderable para Comunidades.";
                    break;
                case TipoPermiso.PRODUCTOS_DIFERENTES_A_LA_MADERA_PARA_COMUNIDADES:
                    CodigoRecursoActividad = "FDC";
                    NombreRecursoActividad = "Aprovechamiento Forestal de Productos diferentes a la madera para Comunidades.";
                    break;
                case TipoPermiso.FAUNA_PARA_COMUNIDADES:
                    CodigoRecursoActividad = "FAC";
                    NombreRecursoActividad = "Aprovechamiento de Fauna silvestre para Comunidades.";
                    break;
                case TipoPermiso.FORESTAL_MADERABLE_PREDIOS_PRIVADOS:
                    CodigoRecursoActividad = "FMP";
                    NombreRecursoActividad = "Aprovechamiento Forestal maderable en Predios Privados.";
                    break;
                case TipoPermiso.PRODUCTOS_DIFERENTES_A_LA_MADERA_PREDIOS_PRIVADOS:
                    CodigoRecursoActividad = "FDP";
                    NombreRecursoActividad = "Aprovechamiento Forestal de Productos diferentes a la madera en Predios Privados.";
                    break;
                case TipoPermiso.FAUNA_PREDIOS_PRIVADOS:
                    CodigoRecursoActividad = "FAP";
                    NombreRecursoActividad = "Aprovechamiento de Fauna silvestre en Predios Privados.";
                    break;
            }
        }

    }
}