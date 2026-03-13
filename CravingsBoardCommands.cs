using System.Text;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;

namespace CravingsBoard;

/// <summary>
/// Chat commands for CravingsBoard.
///   /cravingsboard        — usage help
///   /cravingsboard config — open player picker
///   /cravingsboard list   — view tracked cravings
/// </summary>
[ChatCommandHandler]
public static class CravingsBoardCommands
{
    [ChatCommand("Cravings board for cooks — see what players are craving.", "cravingsboard")]
    public static void CravingsBoard(User user)
    {
        user.MsgLocStr(
            "[CravingsBoard] Commands:\n" +
            "  /cravingsboard config — Pick which players to track\n" +
            "  /cravingsboard list   — View tracked players' cravings");
    }

    [ChatSubCommand("CravingsBoard", "Configure tracked players.", "config")]
    public static void Config(User user)
    {
        CookConfigEditor.EditInteractive(user);
    }

    [ChatSubCommand("CravingsBoard", "View tracked players' cravings.", "list")]
    public static void List(User user)
    {
        var config = CookConfig.Load(user.Name);
        if (config.TrackedPlayers.Count == 0)
        {
            user.MsgLocStr("[CravingsBoard] No players tracked. Use /cravingsboard config to add players.");
            return;
        }

        var grouped = CravingsCollector.GetTrackedCravings(user.Name);
        if (grouped.Count == 0)
        {
            user.MsgLocStr("[CravingsBoard] No tracked players have active cravings.");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("--- Active Cravings ---");
        foreach (var (food, players) in grouped)
            sb.AppendLine($"  {food} ({players.Count}): {string.Join(", ", players)}");

        user.MsgLocStr(sb.ToString());
    }
}
