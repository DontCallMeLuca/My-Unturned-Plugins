using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rocket.API;

namespace DrugsPlugin
{
    public class Configuration : IRocketPluginConfiguration
    {
        public int damage { get; set; }
        public int duration { get; set; }
        public float speed { get; set; }
        public float jump { get; set; }
        public float gravity { get; set; }
        public bool enableMilk { get; set; }

        [XmlArrayItem(ElementName="ID")]
        public List<ushort> Items { get; set; }

        public void LoadDefaults()
        {
            damage = 99;
            duration = 15;
            enableMilk = true;
            Items = new List<ushort>
            {
                269,
                389,
                390,
                404,
                387,
                391,
                388,
                392
            };
        }
    }
}
