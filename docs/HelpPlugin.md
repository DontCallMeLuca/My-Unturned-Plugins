<h1 align="center">✨ Help Plugin ✨</h1>

<h6 align="center"><em>Plugin that implements basic help functionality</em></h6>

## 📝 Overview
This plugin adds simple help functionality to rocket commands.
<br>
It just prints out extra info already present for rocket commands,
<br>
or displays general info on all available commands.
<br>
It also adds a coroutine which displays how to use the help command
every x seconds, in case a player is stuck.
<br>
Help messages can be modified in the translation list.

## ⚙ Configuration

### Fields
- `string GroupId` - The rocket group id used to retrieve the commands
- `int AlertInterval` - The delay in seconds of which the help message is displayed

### Default
- `GroupId`: `"default"`
- `AlertInterval`: `600`

## 🔎 Commands
- `/help <?commandname>` - Displays a help message.

## 💾 Grab a copy
You can find the the compiled dll here: [HelpPlugin.dll](../Plugins/HelpPlugin/bin/HelpPlugin.dll)
