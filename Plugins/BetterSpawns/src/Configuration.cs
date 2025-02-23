using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BetterSpawns.Structures;
using Rocket.API;
using SDG.Unturned;
using UnityEngine;

namespace BetterSpawns
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Verbose { get; set; }

        public bool EnableHomeRespawning { get; set; }

        public bool EnableRandomSpawns { get; set; }

        public bool EnableCustomSpawns { get; set; }

        public bool EnableSmartSpawningBehavior { get; set; }

        public bool EnableAutoRespawning { get; set; }

        public bool ShouldOnlyUseCustomSpawns { get; set; }

        public bool ShouldOnlyRespawnHome { get; set; }

        public float SmartSpawningStrengthCoefficient { get; set; }

        [XmlArrayItem(ElementName = "PermissionGroup")]
        public List<string> PermissionGroups { get; set; }

        [XmlArrayItem(ElementName = "Spawn")]
        public List<SpawnInfo> CustomSpawns { get; set; }

        public void LoadDefaults()
        {
            Verbose = false;

            EnableHomeRespawning = true;

            EnableRandomSpawns = false;
            EnableCustomSpawns = true;

            EnableSmartSpawningBehavior = false;

            EnableAutoRespawning = false;

            ShouldOnlyUseCustomSpawns = false;

            ShouldOnlyRespawnHome = false;

            SmartSpawningStrengthCoefficient = 0.5f;

            PermissionGroups = new List<string>()
            {
                "default"
            };

            CustomSpawns = new List<SpawnInfo>
            {
                new SpawnInfo()
            };
        }
    }
}
