using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rocket.API;
using Steamworks;

namespace EquipedCooldown
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool DebugMode { get; set; }

        [XmlArrayItem(ElementName = "Throwable")]
        public List<ThrowableCooldown> ItemCooldowns { get; set; }
        public void LoadDefaults()
        {
            DebugMode = false;
            ItemCooldowns = new List<ThrowableCooldown>()
            {
                new ThrowableCooldown()
                {
                    ID = 17354,
                    Seconds = 5
                }
            };
        }
    }
}
