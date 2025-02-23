using System;

using System.Collections.Generic;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

using static Rocket.Core.Logging.Logger;

namespace BetterTeleport
{
    public class Main : RocketPlugin<Config>
    {
        public static Main I { get; protected set; }

        protected override void Load()
        {
            I = this;

            Log("BetterTeleport Loaded Successfully.", ConsoleColor.Green);
        }

        protected override void Unload()
        {
            I = null;

            Log("BetterTeleport Unloaded Successfully.", ConsoleColor.Green);
        }

        public LocationDevkitNode FindClosestMatch(string reference)
        {
            double bestAccuracy = Configuration.Instance.LocationNameMatchingAccuracy;
            LocationDevkitNode bestMatch = null;

            if (string.IsNullOrEmpty(reference))
                return null;

            foreach (LocationDevkitNode node in LocationDevkitNodeSystem.Get().GetAllNodes())
            {
                if (node.locationName == string.Empty)
                    continue;

                HashSet<char> setA = new HashSet<char>(reference);
                HashSet<char> setB = new HashSet<char>(node.locationName);

                HashSet<char> intersection = new HashSet<char>(setA);
                intersection.IntersectWith(setB);

                HashSet<char> union = new HashSet<char>(setA);
                union.UnionWith(setB);

                double accuracy = (double)intersection.Count / union.Count;

                if (accuracy >= 0.99f)
                    return node;

                if (accuracy > bestAccuracy)
                {
                    bestAccuracy = accuracy;
                    bestMatch = node;
                }
            }

            return bestMatch;
        }

        public bool TeleportToPlayer(Player player, Player target)
        {
            InteractableVehicle vehicle = target.movement.getVehicle();
            if (vehicle != null)
            {
                if (vehicle.tryAddPlayer(out byte seat, player))
                    return true;

                return player.teleportToLocation(
                    vehicle.transform.position - vehicle.transform.forward * 10f, player.look.yaw
                );
            }
            return player.teleportToPlayer(target);
        }

        protected void raycastFromNearPosition(ref Vector3 position)
        {
            if (Physics.Raycast(
                position + new Vector3(0f, 4f, 0f),
                Vector3.down, out var hitInfo, 8f, RayMasks.WAYPOINT))
            {
                position = hitInfo.point + Vector3.up;
            }
        }

        public bool TeleportToLocation(Player player, string location)
        {
            LocationDevkitNode match = FindClosestMatch(location);

            if (match != null)
            {
                Vector3 position2 = match.transform.position;
                raycastFromNearPosition(ref position2);
                return player.teleportToLocation(position2, player.transform.rotation.eulerAngles.y);
            }
            return false;
        }

        private bool raycastFromSkyToPosition(ref Vector3 position)
        {
            position.y = 1024f;
            if (Physics.Raycast(position, Vector3.down, out var hitInfo, 2048f, RayMasks.WAYPOINT))
            {
                position = hitInfo.point + Vector3.up;
                return true;
            }

            return false;
        }

        public bool TeleportToWaypoint(Player player)
        {
            Vector3 position = player.quests.markerPosition;
            if (raycastFromSkyToPosition(ref position))
                return player.teleportToLocation(position, player.transform.rotation.eulerAngles.y);

            return false;
        }

        public bool TeleportToBed(Player player)
        {
            return player.teleportToBed();
        }
    }
}
