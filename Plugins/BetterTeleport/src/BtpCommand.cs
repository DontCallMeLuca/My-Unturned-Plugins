using System.Collections.Generic;
using SDG.Unturned;
using Rocket.API;

using static Rocket.Unturned.Chat.UnturnedChat;

namespace BetterTeleport
{
    public class BtpCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "btp";

        public string Help => "Improved teleport functionality.";

        public string Syntax => $"/{Name} <location?player?wp> ?<location?player?wp>";

        public List<string> Aliases => new List<string>() { "bettertp", "betterteleport", "bteleport" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            Player player = PlayerTool.getPlayer(caller.DisplayName);

            if (command.Length == 0 || string.Equals(command[0], "wp"))
            {
                if (!Main.I.TeleportToWaypoint(player))
                {
                    Error(caller, "No waypoint set!");
                    return;
                }

                return;
            }

            Player target = PlayerTool.getPlayer(command[0]);

            if (target != null)
            {
                if (!Main.I.TeleportToPlayer(player, target))
                {
                    Error(caller, "Failed to teleport to player!");
                    return;
                }

                return;
            }

            if (string.Equals("bed", command[0]) || string.Equals("home", command[0]))
            {
                if (!Main.I.TeleportToBed(player))
                {
                    Error(caller, "Failed to teleport to bed!");
                    return;
                }
            }

            if (!Main.I.TeleportToLocation(player, command[0]))
            {
                Error(caller, "Failed to teleport to location!");
                return;
            }
        }

        private void Error(IRocketPlayer caller, string message)
        {
            Say(caller, string.Join("", "<color=#ad1313>", message, "</color>"), true);
        }
    }
}
