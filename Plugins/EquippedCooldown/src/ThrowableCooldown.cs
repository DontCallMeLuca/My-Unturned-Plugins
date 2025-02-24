using System.ComponentModel;
using System.Xml.Serialization;

namespace EquipedCooldown
{
    public class ThrowableCooldown
    {
        [XmlAttribute]
        public ushort ID { get; set; }

        [XmlAttribute]
        public ushort Seconds { get; set; }
    }
}
