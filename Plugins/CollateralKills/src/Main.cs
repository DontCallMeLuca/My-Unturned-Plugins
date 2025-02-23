using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Core.Plugins;
using SDG.Unturned;
using UnityEngine;
using static Rocket.Core.Logging.Logger;

namespace CollateralKills
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }

        public Dictionary<ushort, DamageMultiplier> Multipliers { get; private set; }
        public HashSet<ItemGunAsset> PreLoadedAssets = new HashSet<ItemGunAsset>();

        protected override void Load()
        {
            Instance = this;

            Multipliers = Configuration.Instance.Items.ToDictionary(item => item.ID);

            Level.onLevelLoaded += onLevelLoaded;
            UseableGun.onBulletHit += onBulletHit;
            Log("Collateral Kills Successfully Loaded", ConsoleColor.Green);
        }

        protected override void Unload()
        {
            Level.onLevelLoaded -= onLevelLoaded;
            UseableGun.onBulletHit -= onBulletHit;
            Log("Collateral Kills Successfully Unloaded", ConsoleColor.Green);
        }

        private void onLevelLoaded(int _)
        {
            foreach (DamageMultiplier multiplier in Multipliers.Values)
            {
                ItemGunAsset asset = (ItemGunAsset)new Item(multiplier.ID, true).GetAsset();
                if (asset != null) PreLoadedAssets.Add(asset);
            }
        }

        private void onBulletHit(UseableGun gun, BulletInfo bullet, InputInfo hit, ref bool shouldAllow)
        {
            if (hit.player != null)
            {
                if (!Multipliers.TryGetValue(gun.equippedGunAsset.id, out DamageMultiplier multiplierData)) return;
                if (!PreLoadedAssets.TryGetValue(gun.equippedGunAsset, out ItemGunAsset asset)) return;

                Vector3 velocity = bullet.velocity * (1 - Configuration.Instance.VelocityDropoff);

                float time = velocity.magnitude / Vector3.Distance(hit.point, gun.player.transform.position);

                Vector3 hitPosition = bullet.origin + (bullet.velocity * time);

                RaycastInfo raycastInfo = DamageTool.raycast(new Ray(hitPosition, bullet.velocity.normalized),
                                                             Configuration.Instance.MaxRange,
                                                             1 << (int)ELayerMask.PLAYER, null);

                float baseDamage = asset.playerDamageMultiplier.multiply(raycastInfo.limb);

                if (raycastInfo.player != null)
                {
                    float multiplier = multiplierData.DefaultMultiplier;

                    if (raycastInfo.limb == ELimb.SPINE) multiplier = multiplierData.SpineMultiplier;
                    if (raycastInfo.limb == ELimb.SKULL) multiplier = multiplierData.SkullMultiplier;

                    // Sort limbs

                    float distanceFactor = Mathf.Floor(raycastInfo.point.magnitude / multiplierData.RangeStep) * multiplierData.RangeStep * multiplier;

                    float damage = baseDamage * multiplier * (1 - distanceFactor / baseDamage * multiplier);

                    byte damageMultiplier = (byte)Mathf.Clamp(damage, byte.MinValue, byte.MaxValue);

                    raycastInfo.player.life.askDamage(damageMultiplier, Vector3.up * damageMultiplier, EDeathCause.GUN,
                                                      raycastInfo.limb, gun.player.channel.owner.playerID.steamID, out EPlayerKill _);

                }
            }
        }
    }
}
