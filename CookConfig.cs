using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Eco.Shared.Logging;

namespace CravingsBoard;

/// <summary>
/// Per-cook tracked player list, persisted as JSON in Mods/CravingsBoard/config/.
/// </summary>
public sealed class CookConfig
{
    /// <summary>Player names this cook is tracking for cravings.</summary>
    public List<string> TrackedPlayers { get; set; } = new();

    // --- Persistence ---

    private static readonly ConcurrentDictionary<string, CookConfig> Cache = new(StringComparer.OrdinalIgnoreCase);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Load config for a cook (cached after first read).
    /// </summary>
    public static CookConfig Load(string cookName)
    {
        return Cache.GetOrAdd(cookName, LoadFromDisk);
    }

    private static CookConfig LoadFromDisk(string cookName)
    {
        var path = GetPath(cookName);

        if (!File.Exists(path))
            return new CookConfig();

        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<CookConfig>(json, JsonOptions) ?? new CookConfig();
        }
        catch (Exception ex)
        {
            Log.WriteWarningLineLocStr($"[CravingsBoard] Failed to load config for '{cookName}': {ex.Message}");
            return new CookConfig();
        }
    }

    /// <summary>
    /// Persist current settings to disk.
    /// </summary>
    public static void Save(CookConfig config, string cookName)
    {
        var path = GetPath(cookName);
        var dir = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(config, JsonOptions);
        File.WriteAllText(path, json);
        Cache[cookName] = config;
    }

    /// <summary>
    /// Convenience: save this instance for the given cook.
    /// </summary>
    public void Save(string cookName) => Save(this, cookName);

    private static string GetPath(string cookName)
    {
        var safeName = string.Join("_", cookName.Split(Path.GetInvalidFileNameChars()));
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Mods", "CravingsBoard", "config",
            $"{safeName}.json");
    }
}
