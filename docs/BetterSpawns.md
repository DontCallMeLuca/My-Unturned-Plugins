<h1 align="center">✨ Better Spawns ✨</h1>

<h6 align="center"><em>Plugin to improve spawning logic.</em></h6>

## 📝 Overview
This plugin aims to improve the vanilla game's spawning logic.

## ⚙ Configuration

### Fields
- `bool Verbose` - Enable extra logging
- `bool EnableHomeRespawning` - Allow players to respawn home
- `bool EnableRandomSpawns` - Ignore set spawns and spawn anywhere on the map randomly
- `bool EnableCustomSpawns` - Allow spawning from custom defined spawns
- `bool EnableSmartSpawningBehavior` - Will spawn based on player locations and aggregations (Good to avoid spawn camping)
- `bool EnableAutoRespawning` - Automatically respawn, avoiding the respawn screen
- `bool ShoudOnlyUseCustomSpawns` - Only use custom defined spawns
- `bool ShouldOnlyRespawnHome` - Only allows the player to respawn at their set home
- `float SmartSpawningStrengthCoefficient` - The strength factor to avoid nearby players
- `List<string> PermissionGroups` - Rocket permission groups allowed to use commands
- `List<SpawnInfo> CustomSpawns` - Custom spawn dat set by you!

### Default
- `Verbose` - `false`
- `EnableHomeRespawning` - `true`
- `EnableRandomSpawns` - `false`
- `EnableCustomSpawns` - `true`
- `EnableSmartSpawningBehavior` - `false`
- `EnableAutoRespawning` - `false`
- `ShoudOnlyUseCustomSpawns` - `false`
- `ShouldOnlyRespawnHome` - `false`
- `SmartSpawningStrengthCoefficient` - `0.5f` (50%)
- `PermissionGroups` - `["default"]`
- `CustomSpawns` - `[]`

## 🔎 Commands
- `/addspawn <?use_current_view:bool>` - Sets current position as a new spawnpoint (optionally set to use current view vector)
- `/spawns` - Displays a list of spawn positions
- `/position <?playername>` - Displays a player's current position
- `/removespawn ? <x> <y> <z>` - Removes a spawn point (defaults to last set spawn if not specified)

## 💾 Grab a copy
You can find the the compiled dll here: [BetterSpanws.dll](../Plugins/BetterSpawns/bin/BetterSpawns.dll)
