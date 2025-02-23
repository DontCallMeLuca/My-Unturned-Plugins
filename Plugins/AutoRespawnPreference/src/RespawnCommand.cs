using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace AutoRespawnPlugin
{
    public class RespawnCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "autorespawn";

        public string Help => "Toggles the auto respawn feature";

        public string Syntax => $"/{Name}";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller == null) return;

            UnturnedPlayer player = (UnturnedPlayer)caller;
            RespawnModel model = Main.Instance.configs[player.CSteamID.m_SteamID];

            model.shouldRespawn = !model.shouldRespawn;
            UnturnedChat.Say(player, $"Auto respawn {(model.shouldRespawn ? "enabled" : "disabled")}.");
        }
    }
}
