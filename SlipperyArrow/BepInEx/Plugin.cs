using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;

namespace SlipperyArrow
{
    [BepInPlugin("com.rbdev.SlipperyArrow", "SlipperyArrow", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin SlipperyArrow is loaded!");
            Harmony harmony = new Harmony("com.rbdev.SlipperyArrow");

            MethodInfo original = AccessTools.Method(typeof(Arrow), "UpdateSim");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "Patch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void Patch(Arrow __instance, ref StickyRoundedRectangle ___stuckToGround, ref bool ___hasLanded) {
            __instance.StickTo = 0;
            ___stuckToGround = null;
            ___hasLanded = false;
        }
    }
}