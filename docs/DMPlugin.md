<h1 align="center">✨ DM Plugin ✨</h1>

<h6 align="center"><em>Plugin which allows players to dm each other</em></h6>

## 📝 Overview
This plugin allows players to dm each other (send each other private messages on the server).
A player can disable incoming dms, block a specific user from sending dms and of course send dms to players.
<br>
Please note, this is one of the older plugins, it was made with [LiteDB](https://www.litedb.org/), which is a NoSql lightweight database. Its only use is to store player preferences (blocklist etc.). It works, but it might not be very scalable for large servers.

## ⚙ Configuration

**NO CONFIG**

## 🔎 Commands
- `/dm <player> <message>` - Dm a player
- `/block <player>` - Blocks a player from dming you
- `/dms <on?off>` - Block/Allow all incoming dms
- `/unblock <player>` - Unblocks a player allowing them to dm you

## 💾 Grab a copy
You can find the the compiled dll here: [DMPlugin.dll](../Plugins/DMPlugin/bin/DMPlugin.dll)
