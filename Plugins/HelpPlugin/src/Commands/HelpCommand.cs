using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpPlugin.Commands
{
    public class HelpCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "help";

        public string Help => "Displays a list of commands or info on a specific command";

        public string Syntax => $"/{Name} <?commandname>";

        public List<string> Aliases => new List<string>() { "h", "info" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                string name = command[0].ToLower();
                IRocketCommand cmd = Main.Instance.GetCommand(name);
                string cmdName = Main.Instance.GetCmdName(cmd);
                string cmdSyntax = Main.Instance.GetCmdSyntax(cmd);
                string cmdAliases = Main.Instance.GetCmdAliases(cmd);
                string cmdHelp = Main.Instance.GetCmdHelp(cmd);

                UnturnedChat.Say(caller, string.Format(Main.Instance.Translate("CMDINFO"), cmdName, cmdSyntax), true);
                UnturnedChat.Say(caller, string.Format(Main.Instance.Translate("CMDALIASES"), cmdAliases), true);
                UnturnedChat.Say(caller, string.Format(Main.Instance.Translate("CMDHELP"), cmdHelp), true);
            }
            
            catch (IndexOutOfRangeException)
            {
                string commands = Main.Instance.GetCmdNames();
                UnturnedChat.Say(caller, string.Format(Main.Instance.Translate("CMDLIST"), commands), true);
            }
        }
    }
}
