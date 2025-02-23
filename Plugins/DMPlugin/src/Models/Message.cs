using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace DMPlugin.Models
{
    public class Message
    {
        public ulong senderId { get; set; }

        public ulong targetId { get; set; }

        public string content { get; set; }

        public DateTime timestamp { get; set; }

        [BsonId]
        public ulong signature { get; set; }

        public Message() { }

        [BsonCtor]
        public Message(ulong senderId, ulong targetId, string content, DateTime timestamp)
        {
            this.senderId = senderId;
            this.targetId = targetId;
            this.content = content;
            this.timestamp = timestamp;

            signature = senderId + targetId + ToUnixSeconds(timestamp);
        }

        private ulong ToUnixSeconds(DateTime t) 
        {
            return (ulong)(t - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public void Send()
        {
            UnturnedPlayer sender = UnturnedPlayer
            .FromCSteamID(new Steamworks.CSteamID(senderId));

            UnturnedPlayer target = UnturnedPlayer
            .FromCSteamID(new Steamworks.CSteamID(targetId));

            UnturnedChat.Say(target, $"{sender.CharacterName}: {content}", true);
            UnturnedChat.Say(sender, "Successfully sent.");

            Main.Instance.db.StoreMessage(this);
        }
    }
}
