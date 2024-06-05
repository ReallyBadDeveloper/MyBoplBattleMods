using BepInEx;
using HarmonyLib;
using BoplFixedMath;
using UnityEngine.InputSystem;

namespace InfiniteEngine
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

    public class Patches
    {
        [HarmonyPatch(typeof(RocketEngine),nameof(RocketEngine.UpdateSim))]
        [HarmonyPrefix]
        public static void Patch(ref bool ___isEngineOn, ref Fix ___timeSinceEngineStarted, RocketEngine __instance) {
            ___isEngineOn = true;
            ___timeSinceEngineStarted = (Fix)1L;
            __instance.radius = (Fix)9L;
            
            for (int i = 0; i < __instance.ForceAnim.keys.Length; i++)
            {
                __instance.ForceAnim.keys[i].value = (Fix)(-50L);
            }
        }
    }
}
