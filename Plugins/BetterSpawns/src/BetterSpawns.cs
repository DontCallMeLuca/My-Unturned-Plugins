using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using HarmonyLib;
using UnityEngine;
using Steamworks;
using BetterSpawns.Structures;

using static Rocket.Core.Logging.Logger;

namespace BetterSpawns
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }

        public Configuration c { get; private set; }

        public const string HarmonyID = "Arty.BetterSpawns";

        public bool HomeIsEnabled { get; set; }

        public bool ShouldSpawnRandomly { get; set; }

        public bool ShouldUseCustomSpawns { get; set; }

        public bool ShouldSmartSpawn { get; set; }

        public bool ShouldAutoRespawn { get; set; }

        public bool ShouldOnlyUseCustomSpawns { get; set; }

        public bool ShouldOnlyRespawnHome { get; set; }

        public bool Verbose { get; set; }

        protected override void Load()
        {
            Instance = this;

            c = Configuration.Instance;

            HomeIsEnabled = c.EnableHomeRespawning;
            ShouldSpawnRandomly = c.EnableRandomSpawns;
            ShouldUseCustomSpawns = c.EnableCustomSpawns;
            ShouldSmartSpawn = c.EnableSmartSpawningBehavior;
            ShouldAutoRespawn = c.EnableAutoRespawning;
            ShouldOnlyUseCustomSpawns = c.ShouldOnlyUseCustomSpawns;
            ShouldOnlyRespawnHome = c.ShouldOnlyRespawnHome;

            Verbose = c.Verbose;

            Level.onLevelLoaded += OnLevelLoaded;
            PlayerLife.onPlayerDied += OnDeath;
            Provider.onLoginSpawning += OnLoginSpawning;

            new Harmony(HarmonyID).PatchAll(Assembly);

            if (Verbose) Log("Successfully patched ServerRespawn method", ConsoleColor.Green);

            Log("Better Spawns Loaded Successfully", ConsoleColor.Green);
        }

        protected override void Unload()
        {
            Level.onLevelLoaded -= OnLevelLoaded;
            PlayerLife.onPlayerDied -= OnDeath;
            Provider.onLoginSpawning -= OnLoginSpawning;

            Configuration.Save();

            if (Verbose) Log("Successfully Saved Configuration", ConsoleColor.Green);

            Log("Better Spawns Unloaded Successfully", ConsoleColor.Green);
        }

        private void OnLevelLoaded(int _)
        {
            List<PlayerSpawnpoint> spawns = LevelPlayers.spawns;

            foreach (SpawnInfo spawn in c.CustomSpawns)
                spawns.Add(spawn.ToPlayerSpawnpoint());

            if (!ShouldSpawnRandomly && spawns.Count < Provider.maxPlayers)
                if (LevelManager.isArenaMode)
                    Log($"[WARNING] The server has only {spawns.Count} for {Provider.maxPlayers} maximum players (Not Enough)", ConsoleColor.Yellow);
                else
                    Log($"[WARNING] The server spawn to max player ratio is low! ({(spawns.Count / Provider.maxPlayers) * 100}%)", ConsoleColor.Yellow);
        }

        public PlayerSpawnpoint[] GetCustomSpawns()
        {
            Vector3[] customPositions = c.CustomSpawns
                .Select(x => x.Position).ToArray();

            PlayerSpawnpoint[] CustomSpawns = LevelPlayers.spawns.Where(
                x => customPositions.Any(p => p == x.point)).ToArray();

            return CustomSpawns;
        }

        private Vector3[] GetCorners(Bounds bounds)
        {
            Vector3[] corners = new Vector3[8];

            corners[0] = bounds.min;

            corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[4] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            corners[6] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

            corners[7] = bounds.max;

            return corners;
        }

        public PlayerSpawnpoint[] GetArenaSpawns()
        {
            if (LevelManager.isArenaMode)
                return null;

            return new PlayerSpawnpoint[Provider.clients.Count];
        }

        public PlayerSpawnpoint GetSpawn()
        {
            if (ShouldSpawnRandomly && Level.level.gameObject
                .TryGetComponent(out Collider levelCollider))
            {
                Bounds bounds = levelCollider.bounds;

                Vector3 randomPosition = new Vector3(
                    UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                    UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                    UnityEngine.Random.Range(bounds.min.z, bounds.max.z));

                randomPosition = AvoidCollisionWithPlayer(randomPosition);

                return new PlayerSpawnpoint(randomPosition, PlayerLook.characterYaw, false);
            }
            else
            {
                PlayerSpawnpoint[] Spawns = LevelPlayers.getRegSpawns().ToArray();
                int index = UnityEngine.Random.Range(0, Spawns.Length);
                PlayerSpawnpoint[] CustomSpawns = GetCustomSpawns();

                if (ShouldUseCustomSpawns)
                    return ShouldOnlyUseCustomSpawns ? CustomSpawns[index] : Spawns[index];
                return Spawns.Where(x => CustomSpawns.Any(i => i.point != x.point)).ToArray()[index];
            }
        }

        public PlayerSpawnpoint[] GetOptimalArenaSpawns()
        {
            if (!LevelManager.isArenaMode)
                return null;

            float strengthCoefficient = c.SmartSpawningStrengthCoefficient;
            strengthCoefficient *= UnityEngine.Random.Range(0.7f, 1f);

            int playerCount = Provider.clients.Count;

            PlayerSpawnpoint[] OptimalSpawns = new PlayerSpawnpoint[playerCount];

            if (ShouldSpawnRandomly && Level.level.gameObject
                .TryGetComponent(out Collider levelCollider))
            {
                Bounds bounds = levelCollider.bounds;
                Vector3 center = bounds.center;

                Vector3[] corners = GetCorners(bounds);

                foreach (Vector3 corner in corners)
                {
                    float distance = Vector3.Distance(center, corner);
                    float stepSize = distance / (playerCount + 1);

                    Vector3 direction = (corner - center).normalized;
                    Vector3 startPosition = center + direction * stepSize;

                    for (int i = 1; i <= playerCount; i++)
                    {
                        Vector3 point = startPosition + direction * stepSize * i;
                        point = AvoidCollisionWithPlayer(point);
                        OptimalSpawns[i - 1] = new PlayerSpawnpoint(
                            point, PlayerLook.characterYaw, false);
                    }
                }
            }
            else
            {
                PlayerSpawnpoint[] spawnPoints = ShouldOnlyUseCustomSpawns && ShouldUseCustomSpawns
                    ? c.CustomSpawns.Select(spawn => spawn.ToPlayerSpawnpoint()).ToArray() : LevelPlayers.spawns.ToArray();

                if (ShouldUseCustomSpawns && ShouldOnlyUseCustomSpawns)
                {
                    PlayerSpawnpoint[] customSpawns = GetCustomSpawns();
                    spawnPoints = spawnPoints.Where(
                        spawn1 => customSpawns.Any(spawn2 =>
                        spawn1.point == spawn2.point)).ToArray();

                }
                else if (!ShouldUseCustomSpawns)
                {
                    PlayerSpawnpoint[] customSpawns = GetCustomSpawns();
                    spawnPoints = spawnPoints.Where(
                        spawn1 => customSpawns.Any(spawn2 =>
                        spawn1.point != spawn2.point)).ToArray();
                }

                Vector3[] positions = spawnPoints.Select(x => x.point).ToArray();
                Vector3 aggregateCenter = positions.Aggregate((acc, val) => acc + val) / spawnPoints.Length;
                spawnPoints = spawnPoints.OrderBy(spawn => Vector3.Distance(spawn.point, aggregateCenter)).ToArray();
                int stepSize = Mathf.Max(Mathf.CeilToInt((float)spawnPoints.Length / playerCount), 1);

                for (int i = 0; i < playerCount; i++)
                {
                    int index = i * stepSize;

                    if (index >= spawnPoints.Length)
                        index = spawnPoints.Length - 1;

                    OptimalSpawns[i] = spawnPoints[index];
                }
            }

            return OptimalSpawns;
        }

        private void AdjustYPoint(ref Vector3 point)
        {
            if (point.y != 0 || point.y != float.NaN)
                point = new Vector3(point.x, 0, point.z);

            point.y = LevelGround.getHeight(point) + 0.1f;
        }

        private Vector3 AvoidCollisionWithPlayer(Vector3 position)
        {
            Vector3 closestPoint = position;
            const float playerRadius = 0.767166f;

            AdjustYPoint(ref closestPoint);

            int layerMaskValue = (1 << (int)ELayerMask.VEHICLE)   |
                                 (1 << (int)ELayerMask.BARRICADE) |
                                 (1 << (int)ELayerMask.STRUCTURE) |
                                 (1 << (int)ELayerMask.GROUND)    |
                                 (1 << (int)ELayerMask.GROUND2);

            LayerMask mask = layerMaskValue;

            RaycastHit[] hits = Physics.SphereCastAll(
                position, playerRadius, Vector3.zero, mask);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    Vector3 directionToNewPosition = position - hit.point;
                    closestPoint = hit.point + directionToNewPosition.normalized * playerRadius;
                }
            }

            return closestPoint;
        }

        public PlayerSpawnpoint GetOptimalSpawn()
        {
            float strengthCoefficient = c.SmartSpawningStrengthCoefficient;
            strengthCoefficient *= UnityEngine.Random.Range(0.7f, 1f);

            Vector3[] positions = Provider.clients.Select(x => x.player.transform.position).ToArray();

            Vector3 centerPosition = positions.Aggregate((acc, val) => acc + val) / Provider.clients.Count;

            Vector3 closestPlayerPosition = positions.OrderBy(position => Vector3.Distance(centerPosition, position)).First();
            Vector3 furthestPlayerPosition = positions.OrderByDescending(position => Vector3.Distance(centerPosition, position)).First();

            float distanceToClosestPlayer = Vector3.Distance(centerPosition, closestPlayerPosition);
            float distanceToFurthestPlayer = Vector3.Distance(centerPosition, furthestPlayerPosition);

            float distanceCoefficient = distanceToClosestPlayer / distanceToFurthestPlayer;

            PlayerSpawnpoint optimalSpawn = null;
            float maxDistance = float.MinValue;

            if (ShouldSpawnRandomly && Level.level.gameObject.TryGetComponent(out Collider levelCollider))
            {
                Bounds bounds = levelCollider.bounds;
                Vector3 center = bounds.center;

                Vector3[] corners = GetCorners(bounds);

                foreach (Vector3 corner in corners)
                {
                    float minDistance = positions.Min(position => Vector3.Distance(
                        corner, new Vector3(position.x, corner.y, position.z)));

                    if (minDistance > maxDistance)
                    {
                        maxDistance = minDistance;

                        optimalSpawn = new PlayerSpawnpoint(corner,
                            optimalSpawn?.angle ?? PlayerLook.characterYaw,
                            optimalSpawn?.isAlt ?? false);
                    }
                }

                optimalSpawn = new PlayerSpawnpoint(AvoidCollisionWithPlayer(
                    Vector3.Lerp(center, optimalSpawn.point, distanceCoefficient / strengthCoefficient)),
                    optimalSpawn.angle, optimalSpawn.isAlt);
            }
            else
            {
                PlayerSpawnpoint[] spawnPoints = ShouldOnlyUseCustomSpawns && ShouldUseCustomSpawns
                    ? c.CustomSpawns.Select(spawn => spawn.ToPlayerSpawnpoint()).ToArray() : LevelPlayers.spawns.ToArray();

                if (ShouldUseCustomSpawns && ShouldOnlyUseCustomSpawns)
                {
                    PlayerSpawnpoint[] customSpawns = GetCustomSpawns();
                    spawnPoints = spawnPoints.Where(
                        spawn1 =>customSpawns.Any(spawn2 => 
                        spawn1.point == spawn2.point)).ToArray();

                }
                else if (!ShouldUseCustomSpawns)
                {
                    PlayerSpawnpoint[] customSpawns = GetCustomSpawns();
                    spawnPoints = spawnPoints.Where(
                        spawn1 => customSpawns.Any(spawn2 =>
                        spawn1.point != spawn2.point)).ToArray();
                }

                Vector3[] spawnPositions = spawnPoints.Select(x => x.point).ToArray();

                Array.Sort(spawnPoints, (spawnA, spawnB) =>
                    spawnPositions.Min(position => Vector3.Distance(spawnA.point, new Vector3(position.x, spawnA.point.y, position.z)))
                    .CompareTo(spawnPositions.Min(position => Vector3.Distance(spawnB.point, new Vector3(position.x, spawnB.point.y, position.z)))));


                optimalSpawn = spawnPoints[Mathf.RoundToInt((1 - distanceCoefficient / strengthCoefficient) * (spawnPoints.Length - 1))];
            }

            return optimalSpawn;
        }

        private void OnLoginSpawning(SteamPlayerID playerID, ref Vector3 point, 
            ref float yaw, ref EPlayerStance initialStance, ref bool needsNewSpawnpoint)
        {
            if (needsNewSpawnpoint)
                UnturnedPlayer.FromCSteamID(playerID.steamID).Player.life.ServerRespawn(false);
        }

        private void OnDeath(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            sender.ServerRespawn(ShouldOnlyRespawnHome ? true : false);
        }

        public static void addSpawn(Vector3 point, float angle) => 
            LevelPlayers.spawns.Add(new PlayerSpawnpoint(point, angle, false));

        public static void removeSpawn(Vector3 point, float radius)
        {
            radius *= radius;
            List<PlayerSpawnpoint> playerSpawnpointList = new List<PlayerSpawnpoint>();
            for (int index = 0; index < LevelPlayers.spawns.Count; ++index)
            {
                PlayerSpawnpoint spawn = LevelPlayers.spawns[index];

                if ((double)(spawn.point - point).sqrMagnitude < (double)radius)
                    UnityEngine.Object.Destroy((UnityEngine.Object)spawn.node.gameObject);
                else
                    playerSpawnpointList.Add(spawn);
            }
        }
    }
}
