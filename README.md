# RuntimeDebugDisplay for Unity

A lightweight, easy-to-use visual debugging tool for Unity that displays real-time debug information directly on your game screen. Perfect for monitoring FPS, player stats, game state, and any custom values during development and testing.

![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue)
![License](https://img.shields.io/badge/License-MIT-green)

## ✨ Features

- **Real-time FPS / Performance monitoring** - Automatic frame rate calculation and display
- **Visibility toggle** - toggle the panel on and off for visibility
- **Customizable debug values** - Add any key-value pairs you want to monitor
- **Singleton pattern** - One instance across all scenes, survives scene transitions
- **Highly configurable** - Adjust colors, font size, opacity, and update intervals via Inspector
- **Responsive UI** - Panel automatically resizes based on content
- **Zero setup required** - Works out of the box with sensible defaults
- **Performance optimized** - Minimal impact on your game's performance

## 🚀 Quick Start

### Installation

1. Download `DebugOverlay.cs`
2. Drop it into your Unity project's Scripts folder
3. That's it! The debugger will automatically initialize when first accessed.(you can attach it to a game object to access values like color or change them in the script)
4. you can toggle the overlay with backtick or bind it to your liking.

### Basic Usage

```csharp
// Display any value (automatically shows FPS)
RuntimeDebugDisplay.SetDebugValue("Current Level", "Forest Temple");

// Update values in real-time
void Update()
{
    RuntimeDebugDisplay.SetDebugValue("Player Position", transform.position);
    RuntimeDebugDisplay.SetDebugValue("Velocity", rigidbody.velocity.magnitude.ToString("F2"));
}
```

### Configuration

Select the `DebugOverlay` GameObject in the hierarchy and adjust these settings in the Inspector:

| Setting | Description | Default |
|---------|-------------|---------|
| **Update Interval** | How often FPS is calculated (seconds) | 0.5 |
| **Background Opacity** | Transparency of the debug panel | 0.2 |
| **Text Color** | Color of the debug text | Green |
| **Font Size** | Size of the debug text | 25 |
| **Padding** | Offset from screen edge | (10, 10) |
| **Resolution** | Screen size | (1920, 1080) |
| **Panel Width** | Background panel width | 400 |

## 📖 API Reference

### Static Methods

#### `SetDebugValue(string key, object value)`
Adds or updates a debug value to display.

## 🎮 Usage Examples

### Player Stats Monitoring
```csharp
public class PlayerStats : MonoBehaviour
{
    void Update()
    {
        RuntimeDebugDisplay.SetDebugValue("Health", currentHealth);
        RuntimeDebugDisplay.SetDebugValue("Mana", currentMana);
        RuntimeDebugDisplay.SetDebugValue("Level", playerLevel);
        RuntimeDebugDisplay.SetDebugValue("XP", $"{currentXP}/{maxXP}");
    }
}
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## 🙏 Acknowledgments

Created for the Unity community to make debugging easier and more accessible for developers of all skill levels.

---

**Happy Debugging!** 🐛✨

If you find this tool useful, please consider giving it a ⭐ on GitHub!
