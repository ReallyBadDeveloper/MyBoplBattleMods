using BepInEx;
using BepInEx.Configuration;
using BoplFixedMath;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace PlatformZoom
{
    [BepInPlugin("com.rbdev.PlatformZoom", "PlatformZoom", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin PlatformZoom is loaded!");
            Harmony harmony = new Harmony("com.rbdev.PlatformZoom");

            MethodInfo original = AccessTools.Method(typeof(ControlPlatform), "Awake");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "PlatPatch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void PlatPatch(ControlPlatform __instance) {
            __instance.platformForce = (Fix)30L;
        }
    }
}