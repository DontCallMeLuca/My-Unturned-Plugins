using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRespawnPlugin
{
    public class RespawnModel
    {
        public ulong steamid { get; set; }

        public bool shouldRespawn { get; set; }

        public RespawnModel() { }

        public RespawnModel(ulong steamid, bool shouldRespawn)
        {
            this.steamid = steamid;
            this.shouldRespawn = shouldRespawn;
        }
    }
}
