using BepInEx;
using BepInEx.Configuration;
using BoplFixedMath;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace NoInvis
{
    [BepInPlugin("com.rbdev.NoInvis", "No Invis", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin No Invis is loaded!");
            Harmony harmony = new Harmony("com.rbdev.NoInvis");

            MethodInfo original = AccessTools.Method(typeof(Invisibility), "SetAlphaBasedOnPlayerInvisibiltyTime");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "Patch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void Patch(ref Player player) {
            player.isInvisible = false;
        }
    }
}