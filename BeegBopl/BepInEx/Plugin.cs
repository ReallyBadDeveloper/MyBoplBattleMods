using BepInEx;
using BepInEx.Configuration;
using BoplFixedMath;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace BeegAbilities
{
    [BepInPlugin("com.rbdev.BeegAbilities", "BeegAbilities", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin BeegAbilities is loaded!");
            Harmony harmony = new Harmony("com.rbdev.BeegAbilities");

            MethodInfo original = AccessTools.Method(typeof(OrbitItems), "Awake");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "Patch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void Patch(ref float ___wideness) {
            ___wideness = 10f;
        }
    }
}