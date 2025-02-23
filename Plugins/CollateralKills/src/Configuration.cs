using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Rocket.API;

namespace CollateralKills
{
    public class Configuration : IRocketPluginConfiguration
    {
        public float MaxRange { get; set; }
        public float VelocityDropoff { get; set; }

        [XmlArrayItem(ElementName = "Item")]
        public List<DamageMultiplier> Items { get; set; }

        public void LoadDefaults()
        {
            MaxRange = 300;
            VelocityDropoff = 0.1f;

            Items = new List<DamageMultiplier>()
            {
                new DamageMultiplier()
                {
                    ID = 107,
                    DefaultMultiplier = 0.5f,
                    LimbMultiplier = 0.5f,
                    SpineMultiplier = 0.5f,
                    SkullMultiplier = 0.5f,
                    RangeStep = 10f
                },
                new DamageMultiplier()
                {
                    ID = 488,
                    LimbMultiplier = 0.5f,
                    DefaultMultiplier = 0.5f,
                    SpineMultiplier = 0.5f,
                    SkullMultiplier = 0.5f,
                    RangeStep = 10f
                }
            };
        }
    }
}
