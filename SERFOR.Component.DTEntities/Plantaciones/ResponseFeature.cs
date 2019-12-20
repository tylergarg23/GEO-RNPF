using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

//Tyler 17.12.2019
namespace SERFOR.Component.DTEntities.Plantaciones
{
    [DataContract]
    public class ResponseFeature
    {
        [DataMember]
        public PlantacionesFeatureLayer attributes { get; set; }
        public GeometryFeature geometry { get; set; }
    }
}
