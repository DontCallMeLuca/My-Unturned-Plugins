using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPlugin.Models;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;

using Logger = Rocket.Core.Logging.Logger;

namespace DMPlugin
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }

        public DB db { get; private set; }

        public ulong id { get; private set; }

        public Preference preference { get; private set; }

        protected override void Load()
        {
            Instance = this;
            db = new DB();

            Logger.Log("DMPlugin Loaded Successfully");
        }

        protected override void Unload()
        {
            Logger.Log("DMPlugin Unloaded");
        }
    }
}
