using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class CapaArcGIS
    {
        public CapaArcGIS()
        {
            fields = new List<Campo>();
            features = new List<Caracteristica>();
        }
        [DataMember]
        public string displayFieldName { get; set; }

        [DataMember]
        public AliasCampo fieldAliases { get; set; }

        [DataMember]
        public string geometryType { get; set; }

        [DataMember]
        public ReferenciaEspacial spatialReference { get; set; }

        [DataMember]
        public List<Campo> fields { get; set; }

        [DataMember]
        public List<Caracteristica> features { get; set; }
    }

    [DataContract]
    public class AliasCampo
    {
        [DataMember]
        public string OBJECTID { get; set; }

        [DataMember]
        public string PLANID { get; set; }

        [DataMember]
        public string NUMERO { get; set; }

        [DataMember]
        public string NOMPRE { get; set; }

        [DataMember]
        public string DEPART { get; set; }

        [DataMember]
        public string PROVIN { get; set; }

        [DataMember]
        public string DISTRI { get; set; }

        [DataMember]
        public string NOMZON { get; set; }

        [DataMember]
        public string TITULA { get; set; }

        [DataMember]
        public string PROPIE { get; set; }
        
    }

    [DataContract]
    public class ReferenciaEspacial
    {
        [DataMember]
        public int wkid { get; set; }

        [DataMember]
        public int latestWkid { get; set; }
    }

    [DataContract]
    public class Campo
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string alias { get; set; }

        [DataMember]
        public int? length { get; set; }

    }

    [DataContract]
    public class Caracteristica
    {
        public Caracteristica()
        {
            attributes = new Atributos();
            geometry = new Geometria();
        }
        [DataMember]
        public Atributos attributes { get; set; }

        [DataMember]
        public Geometria geometry { get; set; }
    }

    [DataContract]
    public class Atributos
    {
        [DataMember]
        public int OBJECTID { get; set; }

        [DataMember]
        public int PLANID { get; set; }

        [DataMember]
        public string NUMERO { get; set; }

        [DataMember]
        public string NOMPRE { get; set; }

        [DataMember]
        public string DEPART { get; set; }

        [DataMember]
        public string PROVIN { get; set; }

        [DataMember]
        public string DISTRI { get; set; }

        [DataMember]
        public string NOMZON { get; set; }

        [DataMember]
        public string TITULA { get; set; }

        [DataMember]
        public string PROPIE { get; set; }

    }

    [DataContract]
    public class Geometria
    {
        public Geometria()
        {
            rings = new List<double[][]>();
        }
        [DataMember]
        public List<double[][]> rings { get; set; }       
    }

}
