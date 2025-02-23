<h1 align="center">✨ My Unturned Plugins ✨</h1>

<h6 align="center"><em>Collection of most of my Unturned Plugins</em></h6>

## 📝 Overview

This repository contains the majority of my finished Unturned plugins.
They're all in one repo to avoid having like 50 repos for unturned plugins.
<br>
Unturned is an old .NET Unity Engine game from 2014 (still active today),
the game has two main plugin frameworks, RocketMod and OpenMod.
OpenMod being the successor to RocketMod.
<br>
While OpenMod improves on RocketMod on virtually everything,
the plugins here are all still RocketMod, because most servers
still use RocketMod.

## 🌐 External Resources

- [Unturned](https://store.steampowered.com/app/304930/Unturned/)
- [RocketMod](https://github.com/RocketMod/Rocket)
- [UnityEngine](https://learn.unity.com/)

###### For learning resources, see the [learning](#-learning-resources) section

## 📂 Directory structure

The repository is structured as follows:

```
docs/
├──Plugin_1/
│   └── Docs.md
├──Plugin_2/
│   └── Docs.md
Plugins/
├── Plugin_1/
│   ├── src/
│   │   ├── source_1.cs
│   │   └── source_2.cs
│   └── bin/
│       └── Plugin_1.dll
└── Plugin_2/
    ├── src/
    │   ├── source_1.cs
    │   └── source_2.cs
    └── bin/
        └── Plugin_2.dll
```

Where the docs folder contains all the documentation on the given plugin.
<br>
Each plugin has a given src directory with the source code,
<br>
and a bin directory for the compiled plugin.

## 🛠️ Plugins
- [AutoRespawn](./docs/AutoRespawn.md)
- [AutoRespawnPreference](./docs/AutoRespawnPreference.md)
- [BetterSpawns](./docs/BetterSpawns.md)
- [BetterTeleport](./docs/BetterTeleport.md)

## Conventions

Commands' arguments are specified using the following syntax:

```html
/command <arg1> <arg2>
```

Optional arguments will have a question mark infront of them:

```html
/command ?<arg>
```

Further expanding on this, options for an argument are separated as follows:

```html
/command <option1?option2?option3>
```

Some older plugins might specify the expected argument type:

```html
/command <arg:bool>
```

If all the remaining arguments are optional, you might see something like:

```html
/command ? <arg1> <arg2> <arg3>
```

So don't be scared if you see something like:

```html
/command <arg1:int> ? <option1?option2:bool> <arg3>
```

## 📚 Learning Resources

Unturned development has a serious lack of learning resources.
This makes it much harder for newer developers to get started making plugins and/or mods.
<br>

Below I listed some useful learning resources:

- [Unturned Documentation](https://github.com/SmartlyDressedGames/Unturned-Docs) _(Official Unturned documentation)_
- [Unturned Datamining](https://github.com/Unturned-Datamining/Unturned-Datamining) _(Maintained Unturned sourcecode dump)_
- [Imperial Plugins Discord](https://discordapp.com/invite/nCd8QKz) _(Imperial Plugins discord server, with developer resources)_
- [Basic Plugin Tutorial](https://youtu.be/A1MhnhJBnd4?si=SXXibk4eVogFdDYs) _(Basic Rocketmod plugin tutorial by Restore Monarchy)
- [Basic Plugin UI Tutorial](https://youtu.be/J1mcQoxPkpU?si=-tmdBNOKEv9BHj_w) _(Basic plugin UI tutorial by blazethrower320)
- [HarmonyLib](https://github.com/pardeike/Harmony) _(.NET Runtime patching library in C#)
- [C# Language](https://learn.microsoft.com/en-us/dotnet/csharp/) _(The C# programming language)_
- [UnityEngine](https://learn.unity.com/) _(Unity Engine)_
- [MariaDB/MySql](https://mariadb.org/) _(MariaDB/MySql database)_
- [OpenMod](https://openmod.github.io/openmod-docs/) _(The OpenMod Framework)_
- [Modules](https://steamcommunity.com/sharedfiles/filedetails/?id=790629631) _(Guide to making Unturned modules)_

Unturned is made on the unity engine using [MONO](https://www.mono-project.com/). This means that it runs on the .NET framework in C#.
To get started invest in learning C# and the .NET framework. On windows, you can start developing using
[Visual Studio](https://visualstudio.microsoft.com/).
<br>
When starting to develop plugins, I strongly recommend you learn to decompile the game yourself.
Most of your time spent developing you will be looking for methods and properties.
While the Unturned Datamining repository is a good start, it can be beneficial to do this process yourself.
I also recommend you learn OpenMod instead of RocketMod, it's much better and provides detailed API documentation.
<br>
There is a good chance that for some more complicated plugins you will need to modify the game's internal logic,
for this I recommend using HarmonyLib, which allows you to patch the game's internal code.
<br>
Combining this with [reflection](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/)
you can further extend this functionality.
<br>
Plugins are all serversided, when working on the clientside you need to create something called a [module](https://steamcommunity.com/sharedfiles/filedetails/?id=790629631).
This is an API within unturned which allows you to create your own custom modules, which can also be supported
on the serverside (e.g. RocketMod and OpenMod are both modules). Please do note that because the game runs
using the Battleye Anticheat, custom modules will not run with the anticheat enabled.
<br>
When working on a plugin which stores data beyond the server's runtime, you will need a database.
The most commonly used database in this context is MariaDB/MySql. Its simple and effective for this scope.
For example, when working on a basic kits plugin, you could store all the kit data in the plugin's config xml file.
This is ineffective and not very portable, so a common and better implementation is to use a database.
You can find the MySql library for C# using NuGet (the C# .NET package manager).

## ⚠️ Disclaimer

Please note that the plugins aren't actively maintained.
<br>
Nelson (the developer behind unturned) has put tons of efforts into backwards compatibility,
<br>
so there's a good chance they'll still work, but please don't be surprised if they don't anymore.
<br><br>
I will also not be posting modules here, because my modules are usually too big and deserve their own repository.
<br><br>
Some plugins are very old. So my conventions might not always be the same, though generally they should be.

## ✨ Future Changes

- Expand on the plugin collection
- Possible openmod adaptations

## 📃 License
This project uses the `GNU GENERAL PUBLIC LICENSE v3.0` license
<br>
For more info, please find the `LICENSE` file here: [License](LICENSE)
