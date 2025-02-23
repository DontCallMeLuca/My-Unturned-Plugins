using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Assets;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Logger = Rocket.Core.Logging.Logger;

namespace DrugsPlugin
{
    public class Main : RocketPlugin<Configuration>
    {
        public byte Damage { get; set; }
        public bool enableMilk { get; set; }
        public List<ushort> Items { get; set; }
        public int duration { get; set; }
        protected override void Load()
        {

            Damage = (byte)Configuration.Instance.damage;
            enableMilk = Configuration.Instance.enableMilk;
            Items = Configuration.Instance.Items;
            duration = Configuration.Instance.duration;

            UseableConsumeable.onConsumePerformed += onConsume;
            UseableConsumeable.onPerformingAid += onInject;

            Logger.Log("Drugs Plugin Loaded Successfully");
        }

        protected override void Unload()
        {
            UseableConsumeable.onConsumePerformed -= onConsume;
            UseableConsumeable.onPerformingAid -= onInject;

            Logger.Log("Drugs Plugin Unloaded");
        }

        private void onInject(Player instigator, Player target, ItemConsumeableAsset asset, ref bool shouldAllow)
        {
            if (Items.Contains(asset.id))
            {
                switch (ChooseMethod())
                {
                    case 0: Kill(target, instigator, EDeathCause.MELEE); break;
                    case 1: MovementTimer(target); break;
                    case 2: SetHallucination(target); break;
                    case 3: Trip(target); break;
                }
            }
        }

        private void onConsume(Player instigatingPlayer, ItemConsumeableAsset consumeableAsset)
        {
            if (consumeableAsset.id == 462 && enableMilk)
                instigatingPlayer.life.breakLegs();

            if (Items.Contains(consumeableAsset.id))
            {
                int n = ChooseMethod();
                switch (n)
                {
                    case 0: Kill(instigatingPlayer, instigatingPlayer, EDeathCause.SUICIDE); break;
                    case 1: MovementTimer(instigatingPlayer); break;
                    case 2: SetHallucination(instigatingPlayer); break;
                    case 3: Trip(instigatingPlayer); break;
                }
            }
        }

        public int ChooseMethod()
        {
            System.Random rand = new System.Random();
            int n = rand.Next(0, 4);
            return n;
        }

        public void MovementTimer(Player player)
        {
            Task.Run(async () => 
            {
                SetMovement(player);

                for (int i = duration; i > 0; --i)
                    await Task.Delay(1000);

                ResetMovement(player);
            });
        }

        private void Kill(Player player, Player Killer, EDeathCause cause)
        {
            player.life.askDamage(101, Vector3.up * 101f, cause, ELimb.SKULL,
                        UnturnedPlayer.FromPlayer(Killer).CSteamID, out var _);
        }

        private void SetHallucination(Player player)
        {
            player.life.serverModifyHallucination(duration);
        }

        private void Trip(Player player)
        {
            SetHallucination(player);
            MovementTimer(player);
        }

        private void SetMovement(Player player)
        {
            float[] randomFloats = new float[2];

            for (int i = 0; i < 2; ++i)
                randomFloats[i] = (float)(new System.Random()
                    .NextDouble() * (3.0f - 1.5f) + 1.5f);

            float randomGravity = (float)(new System.Random()
                    .NextDouble() * (2.0f - 0.1f) + 0.1f);

            player.movement.sendPluginSpeedMultiplier(randomFloats[0]);
            player.movement.sendPluginJumpMultiplier(randomFloats[1]);
            player.movement.sendPluginGravityMultiplier(randomGravity);
        }

        private void ResetMovement(Player player)
        {
            player.movement.sendPluginSpeedMultiplier(1f);
            player.movement.sendPluginJumpMultiplier(1f);
            player.movement.sendPluginGravityMultiplier(1f);
        }

    }
}