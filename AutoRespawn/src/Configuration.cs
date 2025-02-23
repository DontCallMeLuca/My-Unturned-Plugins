using Rocket.API;

namespace AutoRespawn
{
    public class Configuration : IRocketPluginConfiguration
    {
        public  bool    IsEnabled       { get; set; }
        public  bool    RespawnAtHome   { get; set; } 

        public void LoadDefaults()
        {
            IsEnabled       = true;
            RespawnAtHome   = false;
        }
    }
}

