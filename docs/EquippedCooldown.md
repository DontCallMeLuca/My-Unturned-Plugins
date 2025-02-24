<h1 align="center">✨ Equipped Cooldown ✨</h1>

<h6 align="center"><em>Plugin that adds a cooldown to throwable items</em></h6>

## 📝 Overview
This plugin adds a configurable cooldown to throwable items (yes, the name isn't very appropriate).
<br>
Meaning, an item such as a grenade can only be throw every x seconds.
<br>
Please note that this is a very, VERY, old plugin, so the code is quite bad.

## ⚙ Configuration
        public bool DebugMode { get; set; }

        [XmlArrayItem(ElementName = "Throwable")]
        public List<ThrowableCooldown> ItemCooldowns { get; set; }
### Fields
- `bool DebugMode` - Toggle if debugging, will print extra messages if true.
- `List<ThrowableCooldown> ItemCooldowns` - List of configurable cooldowns.

### Default
- `DebugMode`: `false`
- `items`:
```c#
{
	new ThrowableCooldown()
	{
		ID = 17354,
		Seconds = 5
	}
};
```

## 💾 Grab a copy
You can find the the compiled dll here: [EquippedCooldown.dll](../Plugins/EquippedCooldown/bin/EquipedCooldown.dll)
