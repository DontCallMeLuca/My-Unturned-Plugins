using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Rocket.Unturned.Player;
using Rocket.API;

using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned;
using System.Linq;
using Rocket.Unturned.Chat;

namespace EquipedCooldown
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }

        public Dictionary<CSteamID, Dictionary<ushort, Cooldown>> Cooldowns { get; private set; }
        public List<ThrowableCooldown> ItemCooldowns { get; set; }

        private List<ushort> ValidItems { get; set; }

        public bool DebugMode { get; set; }

        protected override void Load()
        {
            Instance = this;

            Cooldowns = new Dictionary<CSteamID, Dictionary<ushort, Cooldown>>();
            ItemCooldowns = Configuration.Instance.ItemCooldowns;

            ValidItems = new List<ushort>();
            foreach (var i in ItemCooldowns)
                ValidItems.Add(i.ID);

            DebugMode = Configuration.Instance.DebugMode;

            U.Events.OnPlayerConnected += OnPlayerConnect;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnect;
            UseableThrowable.onThrowableSpawned += OnThrow;

            if (DebugMode) 
            { 
                Logger.Log($"Dynamic Dict: {Cooldowns}");
                Logger.Log($"Config: {ItemCooldowns}");
                Logger.Log($"Valid Items: {ValidItems}");
            }

            Logger.Log("EquippedCooldown Successfully Loaded.");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnect;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnect;
            UseableThrowable.onThrowableSpawned -= OnThrow;
            Logger.Log("EquippedCooldown Successfully Unloaded.");
        }

        private void OnPlayerConnect(UnturnedPlayer player)
        {
            if (!Cooldowns.TryGetValue(player.CSteamID, out Dictionary<ushort, Cooldown> value))
            {
                Cooldowns.Add(player.CSteamID, new Dictionary<ushort, Cooldown>());
                if (DebugMode)
                    Logger.Log("Initialized Player Cooldowns.");
            }
            player.Player.equipment.onEquipRequested += onEquipRequested;
        }

        private void OnPlayerDisconnect(UnturnedPlayer player)
        {
            if (DebugMode)
                Logger.Log("Player Disconnect Detected, Resolving Allocated Keys.");
            Cooldowns.Remove(player.CSteamID);
            player.Player.equipment.onEquipRequested -= onEquipRequested;
        }

        private void OnThrow(UseableThrowable useable, UnityEngine.GameObject throwable)
        {
            if (ValidItems.Contains(useable.equippedThrowableAsset.id))
            {
                if (DebugMode)
                    Logger.Log("Detected UseableThrowable Spawn Event.");
                RegisterCooldown(useable.player.channel.owner.playerID.steamID, useable);
                useable.player.equipment.dequip();
            }
        }

        private void onEquipRequested(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            if (ValidItems.Contains(jar.item.id))
            {
                if (DebugMode)
                    Logger.Log($"Equip Request Flagged. ID={jar.item.id}");

                if (HasCooldown(equipment.channel.owner.playerID.steamID, jar.item.id))
                {
                    if (DebugMode)
                        Logger.Log("UseableThrowable Cooldown Did Not Yet Expire.");
                    shouldAllow = false;
                    if (DebugMode)
                        Logger.Log("Blocking Equip Request");
                    var UseableCooldown = Cooldowns[equipment.channel.owner.playerID.steamID][jar.item.id];
                    TimeSpan TimeLeft = UseableCooldown.Expire - DateTime.UtcNow;
                    if (DebugMode)
                        Logger.Log($"Time Left Before Expiry: {(int)TimeLeft.TotalSeconds}s");
                }
                else
                    shouldAllow = true;
            }
        }

        public bool HasCooldown(CSteamID playerid, ushort Id)
        {
            if (Cooldowns.TryGetValue(playerid, out Dictionary<ushort, Cooldown> UseableCooldown))
            {
                if (DebugMode)
                    Logger.Log("Found Player Cooldowns");

                if (!UseableCooldown.TryGetValue(Id, out Cooldown cooldown)) 
                {
                    if (DebugMode)
                        Logger.Log($"No Cooldown Registered For ID={Id}");

                    return false; 
                }
                if (UseableCooldown[Id].Expire.Subtract(DateTime.UtcNow).TotalSeconds <= 0)
                    return false;
                else
                    return true;
            }

            if (DebugMode)
                Logger.Log("Unable To Get Key Value (Cooldown).");

            return false;
        }

        public void RegisterCooldown(CSteamID playerid, UseableThrowable useable)
        {
            if (ValidItems.Contains(useable.equippedThrowableAsset.id))
            {
                if (DebugMode)
                    Logger.Log("Registering New Cooldown.");

                var UseableCooldown = new Cooldown()
                {
                    Useable = useable,
                    Expire = DateTime.UtcNow.AddSeconds(
                        ItemCooldowns.FirstOrDefault(x => x.ID.Equals(
                            useable.equippedThrowableAsset.id)).Seconds)
                };

                if (DebugMode)
                    Logger.Log($"Cooldown Expires: {UseableCooldown.Expire}");

                if (!Cooldowns.TryGetValue(playerid, out Dictionary<ushort, Cooldown> value))
                {
                    if (DebugMode)
                        Logger.Log("ERROR: Did Not Find Player Key, Initializing...");
                    Cooldowns.Add(playerid, new Dictionary<ushort, Cooldown>());
                }
                else
                {
                    if (!Cooldowns[playerid].TryGetValue(useable.equippedThrowableAsset.id, out Cooldown cooldown))
                    {
                        if (DebugMode)
                            Logger.Log("Unable To Find Useable Key In Player Dictionary.");
                        Cooldowns[playerid].Add(useable.equippedThrowableAsset.id, UseableCooldown);
                        if (DebugMode)
                            Logger.Log("Added Useable Key To Player Data.");
                    }
                    else
                        Cooldowns[playerid][useable.equippedThrowableAsset.id] = UseableCooldown;
                }
                UnturnedChat.Say(UnturnedPlayer.FromCSteamID(playerid),
                    $"Cannot equip item, cooldown: {(int)UseableCooldown.Expire.Subtract(DateTime.UtcNow).TotalSeconds + 1}s");
                if (DebugMode)
                    Logger.Log("Stored Cooldown.");
            }
        }
    }
}
