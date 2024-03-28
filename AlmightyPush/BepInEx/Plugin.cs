using BepInEx;
using BepInEx.Configuration;
using BoplFixedMath;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace AlmightyPush
{
    [BepInPlugin("com.rbdev.AlmightyPush", "Almighty Push", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Almighty Push is loaded!");
            Harmony harmony = new Harmony("com.rbdev.AlmightyPush");

            MethodInfo original = AccessTools.Method(typeof(Shockwave), "SetScale");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "GustPatch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void GustPatch(Shockwave __instance, ref Fix ___scale) {
            __instance.platformForce = (Fix)200f;
            __instance.radius = (Fix)200f;
            ___scale = (Fix)200f;
        }
    }
}