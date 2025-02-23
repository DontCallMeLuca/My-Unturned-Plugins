using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DMPlugin.Models
{
    public class Preference
    {
        [BsonId]
        public ulong playerid { get; set; }

        public bool dnd { get; set; }

        public List<ulong> blocked { get; set; }

        public Preference() { }

        [BsonCtor]
        public Preference(ulong playerid, bool dnd, List<ulong> blocked)
        {
            this.playerid = playerid;
            this.dnd = dnd;
            this.blocked = blocked;
        }
    }
}
