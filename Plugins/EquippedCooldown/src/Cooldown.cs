using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;

namespace EquipedCooldown
{
    public class Cooldown
    {
        public UseableThrowable Useable { get; set; }
        public DateTime Expire { get; set; }
    }
}
