using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using UnityEngine;

namespace BetterSpawns.Structures
{
    public class SpawnInfo
    {
        public Vector3 Position { get; set; }

        public float Angle { get; set; }

        public SpawnInfo(Vector3 position, float angle)
        {
            Position = position;
            Angle = angle;
        }

        public SpawnInfo(Vector3 position)
        {
            Position = position;
            Angle = PlayerLook.characterYaw;
        }

        public SpawnInfo()
        {
            Position = Vector3.zero;
            Angle = PlayerLook.characterYaw;
        }

        public static SpawnInfo FromPlayerSpawnpoint(PlayerSpawnpoint spawnpoint)
        {
            return new SpawnInfo(spawnpoint.point, spawnpoint.angle);
        }

        public PlayerSpawnpoint ToPlayerSpawnpoint()
        {
            return new PlayerSpawnpoint(Position, Angle, false);
        }
    }
}
