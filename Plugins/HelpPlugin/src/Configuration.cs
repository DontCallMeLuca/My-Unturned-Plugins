using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpPlugin
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string GroupId { get; set; }
        public int AlertInterval { get; set; }
        public void LoadDefaults()
        {
            GroupId = "default";
            AlertInterval = 600;
        }
    }
}
