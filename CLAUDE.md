# CravingsBoard

Standalone Eco mod — cravings board for cooks. Shows what tracked players are currently craving so cooks can prepare food that satisfies cravings (+10% SP bonus per satisfied craving, up to +30%).

## Build

```bash
dotnet build
```

## Structure

```
CravingsBoard.csproj          — net8.0, Eco.ReferenceAssemblies NuGet + Eco.ModKit.dll
CravingsBoardCommands.cs       — chat commands (/cravingsboard, config, list)
CravingsCollector.cs           — read cravings from tracked players, aggregate by food
CookConfig.cs                  — per-cook tracked player list (JSON persistence)
CookConfigViewModel.cs         — ViewEditor ViewModel with GamePickerList(IAlias)
CookConfigEditor.cs            — ViewEditor open/save logic
EcoModKit_v0.12.0.6-beta/      — ModKit reference (gitignored except Eco.ModKit.dll)
```

## Eco API Reference

### Key APIs Used

- `UserManager.FindUserByName(string)` — look up user by name
- `user.Stomach.Craving?.Name` — read active craving
- `user.MsgLocStr(string)` — send chat message
- `user.Player.OpenInfoPanel(title, content, category)` — resizable/draggable info panel
- `GamePickerList(typeof(IAlias))` — player/alias picker widget
- `ViewEditor.Edit(user, controller, onSubmit, ...)` — settings panel
- `[ChatCommandHandler]` + `[ChatCommand]` + `[ChatSubCommand]` — commands
- `Log.WriteWarningLineLocStr()` — logging (`Eco.Shared.Logging`)

### Known Gotchas

- **Logging namespace**: `Eco.Shared.Logging.Log` — NOT `Eco.Shared.Utils.Log`
- **Target framework**: net8.0 — NOT net9.0 (Eco 0.12.x)
- **Bool ViewEditor**: `[Autogen]` bool checkbox never fires `[AutoRPC]` — use `int` (0/1) workaround
- **Command naming**: Avoid conflicts with other mods' command prefixes
- **ChatCommand duplicate key**: Don't pass explicit alias matching method name — `[ChatCommand("desc")]` on `CravingsBoard()` auto-registers as `cravingsboard`; adding `"cravingsboard"` alias causes `ArgumentException: duplicate key`
- **[Serialized] on transient ViewModel**: Causes silent server hang at startup — use `[AutogenClass]` alone for ephemeral ViewEditor controllers

## Naming Conventions

- No single-letter variables except loop indices
- Descriptive suffixes: `_count`, `_weight`, `_delta`
- Booleans: `is_`, `has_`, `can_`, `should_` prefixes
- Constants: `_STRENGTH`, `_THRESHOLD`, `_WEIGHT`, `_PENALTY`/`_BONUS`
