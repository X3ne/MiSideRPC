using UnityEngine;
using Discord;
using System;
using MiSideRPC;
using MiSideRPC.Scripts;

namespace MiSideDiscord.Components;

public struct LocInfo
{
    public string Action { get; }
    public string Chapter { get; }
    public string Version { get; }
    public string LargeImage { get; }

    public LocInfo(string action, string chapter, string version, string largeImage)
    {
        Action = action;
        Chapter = chapter;
        Version = version;
        LargeImage = largeImage;
    }
    
    public static LocInfo FromHintGameData(HintGameData data)
    {
        return new LocInfo(data.Name, data.Chapter, data.Version, data.CoverImage);
    }
}

public class MiSideDiscordRPC : MonoBehaviour
{
    private static Discord.Discord _client;
    
    private static long _startTime;

    void Start ()
    {
        _client = new Discord.Discord(1337925903606481036, (ulong)CreateFlags.NoRequireDiscord);
        
        _client.SetLogHook(LogLevel.Debug, LogDiscord);
        
        _startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        Plugin.Log.LogInfo("Discord RPC Initialized");
    }
    
    public static void UpdateDiscordStatus(LocInfo loc)
    {
        if (_client == null) return;
        
        var activityManager = _client.GetActivityManager();
        
        var state = string.IsNullOrEmpty(loc.Version) ? loc.Chapter : string.Concat(loc.Chapter, " - ", loc.Version);
        
        var activity = new Activity
        {
            State = state,
            Details = loc.Action,
            Assets =
            {
                LargeImage = loc.LargeImage,
                LargeText = loc.Chapter
            },
            Timestamps =
            {
                Start = _startTime
            }
        };

        activityManager.UpdateActivity(activity, (result) =>
        {
            if (result == Result.Ok)
            {
                Plugin.Log.LogInfo($"Discord Status Updated: {loc.Action} chapter {loc.Chapter}");
            }
            else
            {
                Plugin.Log.LogError($"Discord Status Update Failed: {result}");
            }
        });
    }
    
    void Update()
    {
        _client.RunCallbacks();
    }

    private void LogDiscord(LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.Debug:
                Plugin.Log.LogDebug($"Discord: {message}");
                break;
            case LogLevel.Error:
                Plugin.Log.LogError($"Discord: {message}");
                break;
            case LogLevel.Info:
                Plugin.Log.LogInfo($"Discord: {message}");
                break;
            case LogLevel.Warn:
                Plugin.Log.LogWarning($"Discord: {message}");
                break;
        }
    }
    
    private void DestroyRpc()
    {
        Plugin.Log.LogInfo("Destroying Discord RPC");
        Plugin.IsDiscordRPCEnabled = false;
        Plugin.CreateDiscordRPC();
        _client?.Dispose();
    }

    private void OnDestroy()
    {
        DestroyRpc();
    }

    private void OnApplicationQuit()
    {
        DestroyRpc();
    }
}