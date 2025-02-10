using HarmonyLib;
using MiSideDiscord.Components;
using MiSideRPC.Scripts;
using UnityEngine.UI;

namespace MiSideRPC.Patches;

[HarmonyPatch(typeof(Text), "set_text")]
public class Patch_HintUIText
{
    static void Prefix(Text __instance, ref string value)
    {
        if (__instance.transform.parent.name == "HintScreen")
        {
            Plugin.Log.LogInfo($"Hint text: {value}");
            HintGameData? data = Plugin.GameData.GetHintByText(value);
            
            if (data != null)
            {
                Plugin.Log.LogInfo($"Hint found: {data.Name}");
                MiSideDiscordRPC.UpdateDiscordStatus(LocInfo.FromHintGameData(data));
            }
        }
    }
}