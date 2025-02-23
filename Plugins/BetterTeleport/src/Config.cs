using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace BetterTeleport
{
    public class Config : IRocketPluginConfiguration
    {
        public float LocationNameMatchingAccuracy { get; set; }

        public void LoadDefaults()
        {
            LocationNameMatchingAccuracy = 0.8f;
        }
    }
}
