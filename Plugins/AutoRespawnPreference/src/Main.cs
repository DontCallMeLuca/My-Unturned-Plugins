using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using static Rocket.Core.Logging.Logger;

namespace AutoRespawnPlugin
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }

        public Dictionary<ulong, RespawnModel> configs { get; private set; }

        protected override void Load()
        {
            Instance = this;

            configs = new Dictionary<ulong, RespawnModel>();

            PlayerLife.onPlayerDied += onDeath;
            U.Events.OnPlayerConnected += OnPlayerConnect;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnect;

            Log("Auto Respawn Successfully Loaded", ConsoleColor.Green);
        }

        protected override void Unload()
        {
            foreach (RespawnModel model in configs.Values)
                DBManager.InsertRespawnModel(model);

            PlayerLife.onPlayerDied -= onDeath;
            U.Events.OnPlayerConnected -= OnPlayerConnect;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnect;

            Log("Auto Respawn Successfully Unloaded", ConsoleColor.Green);
        }

        private void OnPlayerConnect(UnturnedPlayer player)
        {
            RespawnModel model = DBManager.RetrieveRespawnModels(player.CSteamID.m_SteamID);
            if (model == null) model = new RespawnModel(player.CSteamID.m_SteamID, true);

            configs.Add(player.CSteamID.m_SteamID, model);
            Log($"Retrieved respawn data for player {player.SteamName}");
        }

        private void OnPlayerDisconnect(UnturnedPlayer player)
        {
            DBManager.InsertRespawnModel(configs[player.CSteamID.m_SteamID]);
            configs.Remove(player.CSteamID.m_SteamID);
            Log($"Stored respawn data for player {player.SteamName}");
        }

        private void onDeath(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            if (configs[UnturnedPlayer.FromPlayer(sender.player).CSteamID.m_SteamID].shouldRespawn)
                sender.ServerRespawn(false);
        }
    }
}
