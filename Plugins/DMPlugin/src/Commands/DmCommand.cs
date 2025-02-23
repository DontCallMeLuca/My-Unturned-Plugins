using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Commands;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Security.Cryptography;

using DMPlugin.Models;

namespace DMPlugin.Commands
{
    public class DmCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "dm";

        public string Help => "Messages a player privately and directly";

        public string Syntax => $"/{Name} <player> <message>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);

                if (player == null) 
                { 
                    UnturnedChat.Say(caller, "No such player exits.");
                    return;
                }

                MessageComponent component = player.GetComponent<MessageComponent>();

                if (component.preference.dnd)
                {
                    UnturnedChat.Say(caller, $"{player.CharacterName} has their dms disabled.");
                }

                else if (component.preference.blocked.Contains(player.CSteamID.m_SteamID))
                {
                    UnturnedChat.Say(caller, $"{player.CharacterName} blocked you!");
                }

                else
                {
                    List<string> text = command.ToList();
                    text.RemoveAt(0);
                    string content = string.Join(" ", text);
                    UnturnedPlayer fromPlayer = (UnturnedPlayer)caller;
                    Message message = new Message(fromPlayer.CSteamID.m_SteamID, 
                                                  player.CSteamID.m_SteamID, 
                                                  content, DateTime.UtcNow);

                    message.Send();
                }
            }
            catch (IndexOutOfRangeException)
            {
                UnturnedChat.Say(caller, "No player found!");
            }
        }
    }
}
