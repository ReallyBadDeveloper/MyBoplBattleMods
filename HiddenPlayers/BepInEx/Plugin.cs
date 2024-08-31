using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using UnityEngine;

namespace HiddenPlayers
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Harmony harmony = new(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(Patches));
        }
    }
    public class Patches {
        [HarmonyPatch(typeof(Player), nameof(Player.UpdateFRAMEDependentInputs))]
        [HarmonyPrefix]
        public static void Patch2(Player __instance)
        {
            __instance.isInvisible = true;
            __instance.timeSpentInvisible = (Fix)0f;
        }
    }
}
