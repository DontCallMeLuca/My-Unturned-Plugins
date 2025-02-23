using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CollateralKills
{
    public class DamageMultiplier
    {
        [XmlAttribute]
        public ushort ID;
        [XmlAttribute]
        public float DefaultMultiplier;
        [XmlAttribute]
        public float LimbMultiplier;
        [XmlAttribute]
        public float SpineMultiplier;
        [XmlAttribute]
        public float SkullMultiplier;
        [XmlAttribute]
        public float RangeStep;
    }
}
