<h1 align="center">✨ Drugs Plugin ✨</h1>

<h6 align="center"><em>Plugin which applies effects for the player when using a consumable item</em></h6>

## 📝 Overview
This plugin is more or less a joke. It applies *interesting* effects to the player which consumes a specific item
(e.g. Superpower, weird visuals, switched controls, etc.).
It also includes attack functionality (Drugs aren't safe).

## ⚙ Configuration

### Fields
- `int damage` - The damage that *could* be applied
- `int duration` - How long the effects last in seconds
- `float speed` - Speed multiplier to use
- `float jump` - The jump multiplier to use
- `float gravity` - The gravity multiplier
- `bool enableMilk` - Make drinking milk break legs
- `List<ushort> Items` - Itemids that can apply these effects

### Default
- `damage`: `99`
- `duration`: `15`
- `enableMilk`: `true`
- `items`:
```c#
{
	269,
	389,
	390,
	404,
	387,
	391,
	388,
	392
}
```

## 💾 Grab a copy
You can find the the compiled dll here: [DrugsPlugin.dll](../Plugins/DrugsPlugin/bin/DrugsPlugin.dll)
