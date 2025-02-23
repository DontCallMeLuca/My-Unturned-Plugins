using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;

using static Rocket.Unturned.Chat.UnturnedChat;

namespace DaylightCycleHandler
{
    public class FreezeCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "freeze";

        public string Help => "Stops the daylight cycle";

        public string Syntax => $"/{Name}";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller == null) return;

            if (Main.Instance.isFrozen)
            {
                LightingManager.cycle           = Main.Instance.Conf.CycleDuration;
                LevelLighting.bias              = Main.Instance.Conf.CycleBiasPercent;

                Main.Instance.isFrozen          = false;
                Main.Instance.Conf.DisableCycle = false;

                Say(caller, "Resumed daylight cycle");
            }

            else
            {
                LightingManager.cycle           = uint.MaxValue;

                Main.Instance.isFrozen          = true;
                Main.Instance.Conf.DisableCycle = true;

                Say(caller, "Paused daylight cycle");
            }
        }
    }
}
