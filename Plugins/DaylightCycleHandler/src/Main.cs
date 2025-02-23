using System;

using SDG.Unturned;
using Rocket.Core.Plugins;

using static Rocket.Core.Logging.Logger;

namespace DaylightCycleHandler
{
    public class Main : RocketPlugin<Configuration>
    {
        public  static Main     Instance    { get; private set; }

        public  Configuration   Conf        { get; set; }

        public  bool            isFrozen    { get; set; }

        protected override void Load()
        {
            Instance    = this;
            Conf        = Configuration.Instance;

            if (Conf.CycleBiasPercent > 100u)
                Conf.CycleBiasPercent = 100u;

            Level.onLevelLoaded += OnLevelLoaded;

            Log("Daylight Cycle Handler Successfully Loaded",   ConsoleColor.Green);
        }

        protected override void Unload()
        {
            Instance    = null;
            Conf        = null;

            Level.onLevelLoaded -= OnLevelLoaded;

            Log("Daylight Cycle Handler Successfully Unloaded", ConsoleColor.Green);
        }

        public void OnLevelLoaded(int _)
        {
            if (Conf.InfiniteDay && Conf.InfiniteNight)
            {
                Conf.DisableCycle       = true;
                Conf.InfiniteDay        = false;
                Conf.InfiniteNight      = false;
            }

            if (Conf.CycleBiasPercent == 100u)
            {
                Conf.InfiniteDay        = true;
                Conf.InfiniteNight      = false;
                Conf.DisableCycle       = true;
            }

            if (Conf.CycleBiasPercent == 0u)
            {
                Conf.InfiniteDay        = false;
                Conf.InfiniteNight      = true;
                Conf.DisableCycle       = true;
            }

            if (Conf.InfiniteDay)
            {
                Conf.DisableCycle       = true;
                LevelLighting.bias      = 0.5f;

                LightingManager.time    = (uint)(LightingManager.cycle * LevelLighting.bias / 2);
            }

            if (Conf.InfiniteNight)
            {
                Conf.DisableCycle       = true;
                LevelLighting.bias      = 0.5f;

                LightingManager.time    = (uint)((LightingManager.cycle * LevelLighting.bias)
                                        + LightingManager.cycle * (1f - LevelLighting.bias) / 2);
            }

            if (Conf.CycleTimeOffset != 0u)
            {
                LightingManager.time    = (LightingManager.time + Conf.CycleTimeOffset) % LightingManager.cycle;
            }

            if (Conf.UseDefaultCycle)
            {
                LightingManager.cycle   = 3600u;
                LightingManager.time    = 0u;
                LevelLighting.bias      = 0.5f;
            }

            else if (Conf.DisableCycle)
            {
                LightingManager.cycle   = uint.MaxValue;
                isFrozen                = true;
            }

            else
            {
                LightingManager.cycle   = Conf.CycleDuration;
                LevelLighting.bias      = Conf.CycleBiasPercent / 100f;
            }

            Log($"Using Default Cycle: {Conf.UseDefaultCycle}",     ConsoleColor.White);
            Log($"Total Cycle Duration: {LightingManager.cycle}",   ConsoleColor.White);

            if (!Conf.UseDefaultCycle)
            {
                Log($"Daylight Cycle is Enabled: {!Conf.DisableCycle}", ConsoleColor.White);
                Log($"Infinite Day is Enabled: {Conf.InfiniteDay}",     ConsoleColor.White);
                Log($"Infinite Night is Enabled: {Conf.InfiniteNight}", ConsoleColor.White);
                Log($"Cycle Bias: {LevelLighting.bias}",                ConsoleColor.White);
            }
        }
    }
}
