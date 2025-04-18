﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace DMPlugin.Commands
{
    public class UnblockCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "unblock";

        public string Help => "Unblocks a player allowing them to message you";

        public string Syntax => $"/{Name} <player>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                string name = command[0];
                UnturnedPlayer target = UnturnedPlayer.FromName(name);

                if (target == null)
                {
                    UnturnedChat.Say(caller, "No player found.");
                    return;
                }

                UnturnedPlayer player = (UnturnedPlayer)caller;
                MessageComponent component = player.GetComponent<MessageComponent>();

                ulong targetId = target.CSteamID.m_SteamID;

                if (component.preference.blocked.Contains(targetId))
                {
                    component.preference.blocked.Remove(targetId);
                    UnturnedChat.Say(player, $"Successfully unblocked {target.CharacterName}");
                }

                else
                {
                    UnturnedChat.Say(player, $"{target.CharacterName} is not blocked!");
                }
            }

            catch (IndexOutOfRangeException)
            {
                UnturnedChat.Say(caller, $"Please specify a player: {Syntax}");
            }
        }
    }
}
