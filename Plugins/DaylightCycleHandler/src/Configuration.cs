using Rocket.API;

namespace DaylightCycleHandler
{
    public class Configuration : IRocketPluginConfiguration
    {
        /// <summary>
        /// The total duration of the day/night cycle in seconds.
        /// </summary>
        public uint CycleDuration       { get; set; }

        /// <summary>
        /// The bias for day/night duration.
        /// The closer to 100% the more day lasts.
        /// The closer to 0% the more the night lasts.
        /// if 50 then the cycle will be equally split.
        /// </summary>
        public uint CycleBiasPercent    { get; set; }

        /// <summary>
        /// Makes daytime infinite.
        /// Ignores cycle settigns.
        /// </summary>
        public bool InfiniteDay         { get; set; }

        /// <summary>
        /// Makes nighttime infinite
        /// Ingores cycle settings.
        /// </summary>
        public bool InfiniteNight       { get; set; }

        /// <summary>
        /// Disable the day/night cycle.
        /// Time essentially freezes.
        /// Ignores cycle settings.
        /// </summary>
        public bool DisableCycle        { get; set; }

        /// <summary>
        /// A hard switch to the default unturned day/night cycle
        /// Ignores all other configuration settings.
        /// </summary>
        public bool UseDefaultCycle     { get; set; }

        /// <summary>
        /// An offset for the cycle's time
        /// Just incase infinite day is weird
        /// </summary>
        public uint CycleTimeOffset     { get; set; }

        public void LoadDefaults()
        {
            CycleDuration       = 3600u;
            CycleBiasPercent    = 50u;

            InfiniteDay         = false;
            InfiniteNight       = false;

            DisableCycle        = false;
            UseDefaultCycle     = true;

            CycleTimeOffset     = 0u;
        }
    }
}
