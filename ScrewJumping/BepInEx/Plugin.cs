using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BoplFixedMath;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine.Yoga;

namespace ScrewJumping
{
    [BepInPlugin("com.rbdev.ScrewJumping", "ScrewJumping", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin ScrewJumping is loaded!");
            Harmony harmony = new Harmony("com.rbdev.ScrewJumping");

            MethodInfo original = AccessTools.Method(typeof(PlayerPhysics), "Jump");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "Patch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void Patch(PlayerPhysics __instance) {
            __instance.jumpStrength = (Fix)(-30L);
        }
    }
}