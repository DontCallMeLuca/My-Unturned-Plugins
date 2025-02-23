using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BetterSpawns.Structures;
using HarmonyLib;
using Rocket.Unturned.Player;
using SDG.Framework.Devkit;
using SDG.NetTransport;
using SDG.Unturned;
using UnityEngine;
using static Rocket.Core.Logging.Logger;

namespace BetterSpawns.RespawnHandler
{
    [HarmonyPatch]
    public static class ServerRespawnPatch
    {
        private static Main main = Main.Instance;

        [HarmonyPatch(typeof(PlayerLife), "ServerRespawn", typeof(bool))]
        public static void ServerRespawn(bool atHome, PlayerLife __instance)
        {
            if (Main.Instance.Verbose) Log("Called Respawn Method");

            ClientInstanceMethod<Vector3, byte> SendRevive = ClientInstanceMethod<Vector3, byte>.Get(typeof(PlayerLife), "ReceiveRevive");
            PropertyInfo spawnpointInfo = typeof(PlayerLife).GetProperty("spawnpoint", BindingFlags.NonPublic | BindingFlags.Instance);

            if (__instance.IsAlive) return;

            __instance.sendRevive();

            if (!main.HomeIsEnabled || !atHome || !BarricadeManager.tryGetBed(
                __instance.channel.owner.playerID.steamID, out var point, out var angle))
            {
                if (Main.Instance.Verbose) Log("No Home");

                PlayerSpawnpoint spawnpoint = main.ShouldSmartSpawn 
                    ? main.GetOptimalSpawn() : main.GetSpawn();

                if (Main.Instance.Verbose) Log("Got spawn");

                if (spawnpoint == null)
                {
                    point = __instance.transform.position;
                    angle = 0;
                }
                else
                {
                    point = spawnpoint.point;
                    angle = MeasurementTool.angleToByte(spawnpoint.angle);
                }

                spawnpointInfo.SetValue(__instance, spawnpoint);

                if (Main.Instance.Verbose) Log("Set Spawn Value");

                // Handle NPCs

                string npcSpawnId = __instance.player.quests.npcSpawnId;

                if (!string.IsNullOrEmpty(npcSpawnId))
                {
                    Spawnpoint newSpawnpoint = SpawnpointSystemV2.Get().FindSpawnpoint(npcSpawnId);
                    if (spawnpoint != null)
                    {
                        point = newSpawnpoint.transform.position;
                        angle = MeasurementTool.angleToByte(newSpawnpoint.transform.rotation.eulerAngles.y);
                    }
                    else
                    {
                        LocationDevkitNode locationDevkitNode = LocationDevkitNodeSystem.Get().FindByName(npcSpawnId);
                        if (locationDevkitNode != null)
                        {
                            point = locationDevkitNode.transform.position;
                            angle = MeasurementTool.angleToByte(UnityEngine.Random.Range(0f, 360f));
                        }
                        else
                        {
                            __instance.player.quests.npcSpawnId = null;
                            UnturnedLog.warn("Unable to find spawnpoint or location matching NpcSpawnId \"" + npcSpawnId + "\"");
                        }
                    }
                }
            }

            // Players without a bed will respawn normally
            if (main.ShouldOnlyRespawnHome && BarricadeManager.tryGetBed(
                __instance.channel.owner.playerID.steamID, out var bedPoint, out var bedAngle))
            {
                angle = bedAngle; point = bedPoint;
            }

            if (Main.Instance.Verbose) Log("Handled 'Only Home'");

            float yaw = MeasurementTool.byteToAngle(angle);

            angle = MeasurementTool.angleToByte(yaw);

            point += new Vector3(0f, 0.5f, 0f);

            if (Main.Instance.Verbose) Log("Set Final Values");

            SendRevive.InvokeAndLoopback(__instance.GetNetId(), 0,
                Provider.GatherRemoteClientConnections(), point, angle);

            if (Main.Instance.Verbose) Log("Sent Revive to Client");
        }
    }

    public class RespawnPatch
    {
        public static void PatchServerRespawn()
        {
            Harmony harmony = new Harmony("Arty.BetterSpawns");
            string targetRespawnMethod = "SDG.Unturned.PlayerLife.ServerRespawn";
            MethodInfo originalMethod = AccessTools.Method(Type.GetType(targetRespawnMethod), "TargetMethod");
            MethodInfo replacementMethod = AccessTools.Method(typeof(ServerRespawnPatch), "ServerRespawn");
            harmony.Patch(originalMethod, new HarmonyMethod(replacementMethod));
            Log("Successfully patched ServerRespawn method", ConsoleColor.Green);
        }
    }
}
