using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Plugins;
using Rocket.Core.Commands;
using Rocket.Unturned;
using SDG.Unturned;
using Rocket.Core;
using Rocket.API.Serialisation;

using static Rocket.Core.Commands.RocketCommandManager;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.API.Collections;
using System.Security;
using Steamworks;
using System.Collections;
using UnityEngine;
using Rocket.Unturned.Chat;

namespace HelpPlugin
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; private set; }
        public static int AlertInterval { get; private set; }
        public List<RegisteredRocketCommand> commands { get; private set; }

        protected override void Load()
        {
            Instance = this;

            AlertInterval = Configuration.Instance.AlertInterval;

            gameObject.AddComponent<Alerter>();

            Level.onLevelLoaded += OnLoaded;

            Logger.Log("Help Plugin Loaded Successfully");
        }

        protected override void Unload()
        {
            Level.onLevelLoaded -= OnLoaded;
            Logger.Log("Help Plugin Unloaded");
        }

        private void OnLoaded(int level)
        {
            RocketPermissionsGroup perms = R.Permissions
                .GetGroup(Configuration.Instance.GroupId);

            commands = R.Commands.Commands
                       .Where(x => perms.Permissions
                       .Select(y => y.Name)
                       .ToList()
                       .Contains(x.Name))
                       .ToList();
        }

        public IRocketCommand GetCommand(string name)
        {
            RegisteredRocketCommand registeredRocketCommand = commands.Where(
                (RegisteredRocketCommand c) => c.Name.ToLower() == name.ToLower()).FirstOrDefault();

            if (registeredRocketCommand == null)
                commands.Where((RegisteredRocketCommand c) => c.Aliases.Select(
                    (string a) => a.ToLower()).Contains(name.ToLower())).FirstOrDefault();

            return registeredRocketCommand;
        }

        public string GetCmdNames()
        {
            return string.Join(", ", commands.Select(x => x.Name).ToList());
        }

        public string GetCmdName(IRocketCommand cmd)
        {
            return cmd.Name;
        }

        public string GetCmdHelp(IRocketCommand cmd)
        {
            return cmd.Help;
        }

        public string GetCmdSyntax(IRocketCommand cmd)
        {
            return cmd.Syntax;
        }

        public string GetCmdAliases(IRocketCommand cmd)
        {
            return string.Join(", ", cmd.Aliases);
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"CMDLIST", "<color=#36a4ff><b>Available commands:</color></b> {0}"},
            {"CMDINFO", "<color=#36a4ff><b>Name: {0}, Syntax: {1}"},
            {"CMDALIASES", "<color=#36ff7c><b>Aliases: {0}"},
            {"CMDHELP", "<color=#f5a442><b>Info: {0}"},
            {"ALERT", "<color=#fffb00><b>Try {0}help to see what a command does!"}
        };
    }
}
