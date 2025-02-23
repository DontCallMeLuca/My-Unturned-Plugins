using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSpawns.Structures;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace BetterSpawns.Commands
{
    public class AddSpawnCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "addspawn";

        public string Help => "Sets current position as a new spawnpoint";

        public string Syntax => $"/{Name} <?use_current_view:bool>";

        public List<string> Aliases => new List<string>() { "setspawn" };

        public List<string> Permissions => Main.Instance.c.PermissionGroups;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            Vector3 position = player.Position;

            float angle;

            if (command.Length < 1)
            {
                string param = command[0].ToLower();
                angle = param == "true" || param == "yes" && 
                       (param != "false" || param != "no")
                      ? player.Player.look.yaw : PlayerLook.characterYaw;
            }
            else
            {
                angle = PlayerLook.characterYaw;
            }

            SpawnInfo spawn = new SpawnInfo(position, angle);
            Main.Instance.c.CustomSpawns.Add(spawn);
            LevelPlayers.spawns.Add(spawn.ToPlayerSpawnpoint());
        }
    }
}
