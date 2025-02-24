using System;

using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;

using SDG.Unturned;

using static Rocket.Core.Logging.Logger;

namespace FallDamageManager
{
    public class Main : RocketPlugin<Configuration>
    {
        protected override void Load()
        {
            U.Events.OnPlayerConnected += OnConnect;
            U.Events.OnPlayerDisconnected += OnDisconnect;

            Log("DisableLegBreaking Loaded Successfully.", ConsoleColor.Green);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnConnect;
            U.Events.OnPlayerDisconnected -= OnDisconnect;

            Log("DisableLegBreaking Loaded Successfully.", ConsoleColor.Green);
        }

        private void OnConnect(UnturnedPlayer player)
        {
            player.Player.life.OnFallDamageRequested += OnFallDamageRequested;
        }

        private void OnDisconnect(UnturnedPlayer player)
        {
            player.Player.life.OnFallDamageRequested -= OnFallDamageRequested;
        }

        private void OnFallDamageRequested(PlayerLife component, float velocity, ref float damage, ref bool shouldBreakLegs)
        {
            if (!Configuration.Instance.EnableLegBreaking)
                shouldBreakLegs = false;

            if (!Configuration.Instance.EnableFallDamage)
            {
                damage = 0f;
            }
            else
            {
                damage *= Configuration.Instance.FallDamageMultiplier;
            }
        }
    }
}
