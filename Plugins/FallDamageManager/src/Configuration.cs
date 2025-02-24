using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace FallDamageManager
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool EnableLegBreaking { get; set; }

        public bool EnableFallDamage { get; set; }

        public float FallDamageMultiplier { get; set; }

        public void LoadDefaults()
        {
            EnableLegBreaking = false;
            EnableFallDamage = false;
            FallDamageMultiplier = 1.0f;
        }
    }
}
