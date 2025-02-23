using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using static UnityEngine.GraphicsBuffer;

namespace BetterSpawns.Commands
{
    public class PositionCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "position";

        public string Help => "Displays a player's current position";

        public string Syntax => $"/{Name} <?playername>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => Main.Instance.c.PermissionGroups;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer target;
            if (command.Length < 1)
            {
                target = (UnturnedPlayer)caller;
            }
            else
            {
                target = UnturnedPlayer.FromName(command[0]);
                if (target == null)
                {
                    UnturnedChat.Say(caller, $"Found no player named {command[0]}");
                    return;
                }
            }
            UnturnedChat.Say(caller, $"{target.DisplayName}'s position: {target.Position}");
        }
    }
}
