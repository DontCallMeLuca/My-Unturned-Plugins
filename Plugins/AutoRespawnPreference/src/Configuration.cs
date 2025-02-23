using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace AutoRespawnPlugin
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string server { get; set; }
        public string database { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string port { get; set; }

        public void LoadDefaults()
        {
            server = "localhost";
            database = string.Empty;
            user = "root";
            password = string.Empty;
            port = "3306";
        }
    }
}
