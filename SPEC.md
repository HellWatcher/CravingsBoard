# CravingsBoard — Specification

## Motivation

Cooks in Eco have no way to know what other players are craving. Since satisfying a craving gives +10% SP bonus (up to +30% with 3 satisfied cravings), this information is valuable for cooperative play. CravingsBoard lets cooks track specific players and see their active cravings aggregated by food.

## Eco Craving Mechanics

- Players develop food cravings approximately every 2 real-time hours
- Each satisfied craving grants +10% SP bonus
- Maximum 3 simultaneous cravings (+30% total bonus)
- Cravings are stored in `user.Stomach.Craving`
- Craving names match food item type names (may have "Item" suffix)

## Cook-Centric Tracking Model

Each cook independently selects which players to track:

1. Cook opens `/cravingsboard config` → ViewEditor with `GamePickerList(typeof(IAlias))`
2. Selects players (and possibly demographic groups) from the picker
3. Saves → player names persisted to `Mods/CravingsBoard/config/{cookName}.json`
4. `/cravingsboard list` reads cravings only for tracked players

## Command Specification

### `/cravingsboard`

Shows usage help with available subcommands.

### `/cravingsboard config`

Opens ViewEditor with player picker. On save, extracts IAlias names and persists.

### `/cravingsboard list`

Reads `Stomach.Craving` for each tracked player, aggregates by food name:

```
--- Active Cravings ---
  Corn (2): PlayerA, PlayerC
  Beet Salad (1): PlayerB
```

Edge cases:

- No tracked players configured → prompt to use config command
- Tracked players but no active cravings → "No tracked players have active cravings"
- Tracked player offline → silently skipped (UserManager returns null)

## Data Flow

```
/cravingsboard list
  → CookConfig.Load(cookName) — cached, from JSON
  → for each trackedPlayer:
      UserManager.FindUserByName(name)
      → user.Stomach.Craving?.Name
  → aggregate: Dictionary<foodName, List<playerName>>
  → format and display via user.MsgLocStr()
```

## Config Persistence

- Format: JSON via System.Text.Json
- Location: `Mods/CravingsBoard/config/{sanitizedCookName}.json`
- Caching: ConcurrentDictionary, loaded once per cook
- Fallback: missing or corrupt file → empty config (no tracked players)

## Future Ideas

- Tooltip on kitchen/stove showing cravings of nearby players
- Craving change notifications (alert cook when a tracked player gets a new craving)
- Integration with EcoDietMod for optimal meal planning
