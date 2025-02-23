using System;

using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;

using static Rocket.Core.Logging.Logger;

namespace AutoRespawn
{
    public class Main : RocketPlugin<Configuration>
    {
        protected override void Load()
        {
            PlayerLife.onPlayerDied += onDeath;

            Log("Auto Respawn Successfully Loaded", ConsoleColor.Green);
        }

        protected override void Unload()
        {
            PlayerLife.onPlayerDied -= onDeath;

            Log("Auto Respawn Successfully Unloaded", ConsoleColor.Green);
        }

        private void onDeath(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            if (Configuration.Instance.IsEnabled)
                sender.ServerRespawn(Configuration.Instance.RespawnAtHome);
        }
    }
}

