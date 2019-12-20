//TYLER 16.12.19 - INICIO
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.DTEntities.Plantaciones
{
    public class GeometryFeature
    {
        public IEnumerable rings { get; set; }
        public SpatialReference spatialReference { get; set; }
    }
}
//TYLER 16.12.19 - FIN