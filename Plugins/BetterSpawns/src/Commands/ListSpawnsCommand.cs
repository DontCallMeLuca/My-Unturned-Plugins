using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace BetterSpawns.Commands
{
    public class ListSpawnsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "spawns";

        public string Help => "Displays a list of spawn positions";

        public string Syntax => $"/{Name}";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => Main.Instance.c.PermissionGroups;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            string spawns = string.Join(", ", Main.Instance.c.CustomSpawns
                .Select(x => $"x: {x.Position.x}, y: {x.Position.y}, z: {x.Position.z}"));

            UnturnedChat.Say(caller, spawns);
        }
    }
}
