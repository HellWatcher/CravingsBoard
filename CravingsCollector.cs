using System;
using System.Collections.Generic;
using System.Linq;
using Eco.Gameplay.Players;

namespace CravingsBoard;

/// <summary>
/// Reads cravings from tracked players and aggregates by food name.
/// </summary>
public static class CravingsCollector
{
    /// <summary>
    /// Returns cravings grouped by food: { "Corn" → ["PlayerA", "PlayerC"], ... }.
    /// Only includes tracked players who are online and have an active craving.
    /// </summary>
    public static Dictionary<string, List<string>> GetTrackedCravings(string cookName)
    {
        var config = CookConfig.Load(cookName);
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var playerName in config.TrackedPlayers)
        {
            var user = UserManager.FindUserByName(playerName);
            if (user?.Stomach?.Craving == null)
                continue;

            var foodName = user.Stomach.Craving.Name;
            if (string.IsNullOrEmpty(foodName))
                continue;

            // Strip trailing "Item" suffix for cleaner display
            if (foodName.EndsWith("Item", StringComparison.Ordinal))
                foodName = foodName[..^4];

            if (!result.TryGetValue(foodName, out var players))
            {
                players = new List<string>();
                result[foodName] = players;
            }
            players.Add(playerName);
        }

        return result.OrderBy(kv => kv.Key)
                     .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}
