using BepInEx;
using HarmonyLib;
using BoplFixedMath;

namespace NoPlatformShrink
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
    class Patches
    {
        [HarmonyPatch(typeof(SlimeController), nameof(SlimeController.UpdateSim))]
        [HarmonyPrefix]
        public static void Update() {
            Constants.timeUntilPlatformsReturnToOriginalSize = Fix.MaxValue;
        }
    }
}