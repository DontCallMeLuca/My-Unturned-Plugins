using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace BetterSpawns.Commands
{
    public class RemoveSpawnCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "removespawn";

        public string Help => "Removes a spawn point";

        public string Syntax => $"/{Name} ? <x> <y> <z>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => Main.Instance.c.PermissionGroups;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            PlayerSpawnpoint spawnpoint;

            if (command.Length < 1)
            {
                spawnpoint = LevelPlayers.spawns.Last();
            }
            else if (command.Length < 3)
            {
                UnturnedChat.Say(caller, "All 3 components must be specified (x, y, z)");
                return;
            }
            else
            {
                if (float.TryParse(command[0], out float x) && 
                    float.TryParse(command[1], out float y) && 
                    float.TryParse(command[2], out float z))
                {
                    Vector3 position = new Vector3(x, y, z);

                    spawnpoint = LevelPlayers.spawns.LastOrDefault(spawn =>
                        Mathf.RoundToInt(spawn.point.x) == Mathf.RoundToInt(position.x) &&
                        Mathf.RoundToInt(spawn.point.y) == Mathf.RoundToInt(position.y) &&
                        Mathf.RoundToInt(spawn.point.z) == Mathf.RoundToInt(position.z));
                }
                else
                {
                    UnturnedChat.Say(caller, $"Invalid argument! x, y, z components must be a number or a decimal!");
                    return;
                }
            }

            if (spawnpoint != null)
            {
                Vector3[] customSpawns = Main.Instance.GetCustomSpawns()
                                         .Select(i => i.point).ToArray();

                LevelPlayers.spawns.Remove(spawnpoint);

                if (customSpawns.Contains(spawnpoint.point))
                {
                    Main.Instance.c.CustomSpawns.Remove(Main.Instance.c.CustomSpawns
                        .FirstOrDefault(i => i.Position == spawnpoint.point));
                }

                UnturnedChat.Say(caller, $"Successfully removed spawn (x: {spawnpoint.point.x}, y: {spawnpoint.point.y}, z: {spawnpoint.point.z})");
            }
            else
            {
                UnturnedChat.Say(caller, "There was an error retrieving the spawnpoint.");
            }
        }
    }
}
