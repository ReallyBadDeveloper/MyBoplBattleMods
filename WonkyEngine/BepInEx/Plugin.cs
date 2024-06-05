using BepInEx;
using HarmonyLib;
using BoplFixedMath;
using UnityEngine.InputSystem;

namespace InfiniteEngine
{
    [BepInPlugin("com.ReallyBadDev.WonkyEngine", "WonkyEngine", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {"com.ReallyBadDev.WonkyEngine"} is loaded!");

            Harmony harmony = new("com.ReallyBadDev.WonkyEngine");
            
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
            var prop = AccessTools.Property(typeof(RocketEngine), nameof(RocketEngine.LocalPlatformPosition));
            prop.SetValue(__instance, __instance.LocalPlatformPosition + (Fix).01);
            for (int i = 0; i < __instance.ForceAnim.keys.Length; i++)
            {
                __instance.ForceAnim.keys[i].value = (Fix)(-200L);
            }
        }
    }
}
