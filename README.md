# CravingsBoard

An Eco mod that lets cooks see what other players are craving. Satisfying a player's craving gives them a +10% SP bonus (up to +30% with 3 satisfied cravings), so this info helps cooks prepare the most beneficial meals.

## Installation

1. Build: `dotnet build`
2. Copy `bin/Debug/net8.0/CravingsBoard.dll` to your server's `Mods/` directory
3. Restart the server

## Commands

| Command                 | Description                               |
| ----------------------- | ----------------------------------------- |
| `/cravingsboard`        | Show usage help                           |
| `/cravingsboard config` | Open player picker to choose who to track |
| `/cravingsboard list`   | View tracked players' active cravings     |

## Example Output

```
--- Active Cravings ---
  Corn (2): PlayerA, PlayerC
  Beet Salad (1): PlayerB
```

When no one has cravings:

```
[CravingsBoard] No tracked players have active cravings.
```

## How It Works

1. A cook runs `/cravingsboard config` to select which players to track
2. The mod saves the selection per-cook in `Mods/CravingsBoard/config/`
3. Running `/cravingsboard list` reads each tracked player's `Stomach.Craving`
4. Results are aggregated by food and displayed with player counts

## Compatibility

- Eco 0.12.x (tested with 0.12.0.6-beta)
- .NET 8.0
