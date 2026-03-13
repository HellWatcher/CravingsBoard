using System;
using System.Linq;
using Eco.Core.Controller;
using Eco.Gameplay.Aliases;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems;
using Eco.Shared.Localization;
using Eco.Shared.Logging;

namespace CravingsBoard;

/// <summary>
/// Opens a ViewEditor for the cook to pick which players to track.
/// </summary>
public static class CookConfigEditor
{
    /// <summary>
    /// Opens the ViewEditor panel for the cook's tracked player list.
    /// </summary>
    public static void EditInteractive(User user)
    {
        var config = CookConfig.Load(user.Name);
        var viewModel = CreateViewModel(config);

        ViewEditor.Edit(
            user,
            viewModel,
            onSubmit: _ => ApplyAndSave(viewModel, config, user),
            buttonText: Localizer.DoStr("Save"),
            overrideTitle: Localizer.DoStr("CravingsBoard — Tracked Players"),
            windowType: ViewEditor.WindowType.Small);
    }

    /// <summary>
    /// Populate picker from saved player names.
    /// </summary>
    private static CookConfigViewModel CreateViewModel(CookConfig config)
    {
        var viewModel = new CookConfigViewModel();

        foreach (var playerName in config.TrackedPlayers)
        {
            var user = UserManager.FindUserByName(playerName);
            if (user != null)
                viewModel.PlayerFilter.Entries.Add(user);
        }

        return viewModel;
    }

    /// <summary>
    /// Extract player names from picker, save config.
    /// </summary>
    private static void ApplyAndSave(CookConfigViewModel viewModel, CookConfig config, User user)
    {
        try
        {
            config.TrackedPlayers = viewModel.PlayerFilter
                .GetObjects<IAlias>()
                .Select(alias => alias.Name)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            config.Save(user.Name);
            user.MsgLocStr($"[CravingsBoard] Now tracking {config.TrackedPlayers.Count} player(s).");
        }
        catch (Exception ex)
        {
            Log.WriteWarningLineLocStr($"[CravingsBoard] Error saving config: {ex.Message}");
            user.MsgLocStr($"[CravingsBoard] Error saving config: {ex.Message}");
        }
    }
}
