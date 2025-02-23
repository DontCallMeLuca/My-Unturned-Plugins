using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPlugin.Models;
using LiteDB;

namespace DMPlugin
{
    public class DB
    {
        public string dir;

        public DB() 
        {
            dir = @"dms";
        }

        public void StorePreference(Preference preference)
        {
            using (var db = new LiteDatabase(dir))
            {
                var collection = db.GetCollection<Preference>("Preferences");
                collection.Upsert(preference);
            }
        }

        public void StoreMessage(Message message)
        {
            using (var db = new LiteDatabase(dir))
            {
                var collection = db.GetCollection<Message>("Messages");
                collection.Upsert(message);
            }
        }

        public Preference GetPreference(ulong id)
        {
            using (var db = new LiteDatabase(dir))
            {
                var collection = db.GetCollection<Preference>("Preferences");
                List<Preference> preferences = collection.Query().ToList();
                return preferences.FirstOrDefault(x => x.playerid == id);
            }
        }
    }
}
