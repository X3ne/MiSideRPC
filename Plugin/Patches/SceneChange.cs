using HarmonyLib;
using MiSideDiscord.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiSideRPC.Patches;

public class SceneChange
{
    [HarmonyPatch(typeof(SceneManager), "LoadScene", new[] { typeof(string) })]
    public static class SceneManager_LoadScene_Patch
    {
        static void Prefix(string sceneName)
        {
            Plugin.Log.LogInfo($"Scene loading started: {sceneName}");
        }

        static void Postfix(string sceneName)
        {
            Plugin.Log.LogInfo($"Scene loaded: {sceneName}");
            
            switch (sceneName)
            {
                case "SceneMenu":
                    MiSideDiscordRPC.UpdateDiscordStatus(new LocInfo("In Menu", "Main Menu", "", "logo"));
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(SceneManager), "LoadSceneAsync", new[] { typeof(string) })]
    public static class SceneManager_LoadSceneAsync_Patch
    {
        static void Prefix(string sceneName)
        {
        }

        static void Postfix(string sceneName, ref AsyncOperation __result)
        {
            Plugin.Log.LogInfo($"Async scene loaded: {sceneName}");
            
            switch (sceneName)
            {
                case "SceneMenu":
                    MiSideDiscordRPC.UpdateDiscordStatus(new LocInfo("In Menu", "Main Menu", "", "logo"));
                    break;
            }
        }
    }
}