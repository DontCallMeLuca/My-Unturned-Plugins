using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace DMPlugin.Commands
{
    public class DNDCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "dms";

        public string Help => "Blocks/Allows incoming dms";

        public string Syntax => "dms <on?off>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;

                MessageComponent component = player.GetComponent<MessageComponent>();

                if (command[0].ToLower() == "on")
                {
                    if (component.preference.dnd)
                    {
                        component.preference.dnd = false;
                        UnturnedChat.Say(player, "Successfully enabled dms.");
                    }

                    else
                    {
                        UnturnedChat.Say(player, "Your dms are already enabled.");
                    }
                }

                else if (command[0].ToLower() == "off")
                {
                    if (component.preference.dnd)
                    {
                        UnturnedChat.Say(player, "Your dms are already disabled.");
                    }

                    else
                    {
                        component.preference.dnd = true;
                        UnturnedChat.Say(player, "Successfully disabled dms.");
                    }
                }
                else
                {
                    UnturnedChat.Say(player, Syntax);
                }
            }
            catch 
            {
                UnturnedChat.Say(caller, Syntax);
            }
        }
    }
}
