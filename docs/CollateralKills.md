<h1 align="center">✨ Collateral Kills ✨</h1>

<h6 align="center"><em>Plugin to allow for collateral kills & wallbangs</em></h6>

## 📝 Overview
This plugin implements logic for collateral kills and wallbangs.
In the vanilla game no such feature exists, where you must destroy the barricade
and no bullet can go "through" a player.
<br>
It supports body part based damage configuration as well as bullet travel configuration.
Please note it *should* work with bullet travel enabled on your server, but might not as expected.

## ⚙ Configuration

### Fields
- `float MaxRange` - Maximum range to apply functionality
- `float VelocityDropoff` - Bullet velocity and travel dropoff
- `List<DamageMultiplier> Items` - Valid items to apply functionality to

### Default
- `MaxRange` - `300`
- `VelocityDropoff` - `0.1f`
- `Items`:
```cs
{
	ID = 107,
	DefaultMultiplier = 0.5f,
	LimbMultiplier = 0.5f,
	SpineMultiplier = 0.5f,
	SkullMultiplier = 0.5f,
	RangeStep = 10f
},
{
	ID = 488,
	LimbMultiplier = 0.5f,
	DefaultMultiplier = 0.5f,
	SpineMultiplier = 0.5f,
	SkullMultiplier = 0.5f,
	RangeStep = 10f
}
```

## 💾 Grab a copy
You can find the the compiled dll here: [CollateralKills.dll](../Plugins/CollateralKills/bin/CollateralKills.dll)
