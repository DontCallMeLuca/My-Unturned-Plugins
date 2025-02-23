using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPlugin.Models;
using Rocket.Unturned.Player;

namespace DMPlugin
{
    public class MessageComponent : UnturnedPlayerComponent
    {
        public ulong id { get; private set; }

        public Preference preference { get; private set; }

        protected override void Load()
        {
            id = Player.CSteamID.m_SteamID;
            preference = Main.Instance.db.GetPreference(id);

            if (preference == null)
            {
                preference = new Preference(id, false, new List<ulong>());
            }
        }

        protected override void Unload()
        {
            Main.Instance.db.StorePreference(preference);
        }
    }
}
