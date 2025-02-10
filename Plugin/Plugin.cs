using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HarmonyLib.Tools;
using MiSideDiscord.Components;
using MiSideRPC.Scripts;

namespace MiSideRPC;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    internal static Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    
    internal static GameHints GameData = new GameHints();
    internal static bool IsDiscordRPCEnabled = false;

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo("MiSideRPC is loaded!");
        
        Assembly.Load(MyPluginInfo.PLUGIN_GUID);
            
        HarmonyFileLog.Enabled = true;
        
        harmony.PatchAll();
        
        CreateDiscordRPC();
    }

    public static void CreateDiscordRPC()
    {
        if (!IsDiscordRPCEnabled)
        {
            IL2CPPChainloader.AddUnityComponent(typeof(MiSideDiscordRPC));
            IsDiscordRPCEnabled = true;
        }
    }
}
