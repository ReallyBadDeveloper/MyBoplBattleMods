using BepInEx;
using BoplFixedMath;
using HarmonyLib;

namespace DeadlyRoll
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
        [HarmonyPatch(typeof(Roll), nameof(Roll.UpdateSim))]
        [HarmonyPrefix]
        public static void Patch(Roll __instance)
        {
            __instance.dashTime = Fix.MaxValue;
            __instance.MaxSpeed = (Fix)10000L;
            __instance.MinSpeed = (Fix)10000L;
        }
    }
}
