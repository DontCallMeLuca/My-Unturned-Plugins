<h1 align="center">✨ DaylightCycleHandler ✨</h1>

<h6 align="center"><em>Plugin to customize the vanilla game's daylight cycle</em></h6>

## 📝 Overview
This plugin allows you to customize the game's daylight cycle,
it allows you to modify the overall duration, change the cycle's bias,
set infinite day or night and freeze time.

## ⚙ Configuration

### Fields
- `uint CycleDuration` - The overall cycle's duration (day + night)
- `uint CycleBiasPercent` - Shift cycle's duration to be more day or night
- `bool InfiniteDay` - Will always be day.
- `bool InfiniteNight` - Will always be night.
- `bool DisableCycle` - Freezes time.
- `bool UseDefaultCycle` - Simple toggle to the vanilla logic.
- `uint CycleTimeOffset` - Offsets the start time of the cycle (for some maps that have a weird cycle like dango)

### Default
- `CycleDuration`: `3600u`
- `CycleBiasPercent`: `50u`
- `InfiniteDay`: `false`
- `InfiniteNight`: `false`
- `DisableCycle`: `false`
- `UseDefaultCycle`: `true`
- `CycleTimeOffset`: `0u`

## 🔎 Commands
- `/freeze` - Stops the daylight cycle (Freeze time)

## 💾 Grab a copy
You can find the the compiled dll here: [DaylightCycleHandler.dll](../Plugins/DaylightCycleHandler/bin/DaylightCycleHandler.dll)
