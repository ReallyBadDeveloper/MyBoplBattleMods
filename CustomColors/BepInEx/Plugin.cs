using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CustomColors
{
    [BepInPlugin("com.rbdev.CustomColors", "CustomColors", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        private static ManualLogSource logger;
        internal static ConfigEntry<float> boplRed;
        internal static ConfigEntry<float> boplGreen;
        internal static ConfigEntry<float> boplBlue;
        internal static ConfigEntry<float> boplAlpha;
        private void Awake()
        {
            logger = base.Logger;
            Logger.LogInfo("Plugin CustomColors is loaded!");
            Harmony harmony = new("com.rbdev.CustomColors");

            if (!File.Exists("/BepInEx/config/com.rbdev.CustomColors.cfg"))
            {
                boplRed = Config.Bind("General.Color", "Red", 255f, "The red value of the Bopl.");
                boplGreen = Config.Bind("General.Color", "Green", 255f, "The green value of the Bopl.");
                boplBlue = Config.Bind("General.Color", "Blue", 255f, "The blue value of the Bopl.");

                Logger.LogInfo("Bound new config file because existing config could not be found");
            }
            harmony.PatchAll(typeof(Patches));
            Plugin.InfoLog("Log test, ignore");
            Plugin.WarnLog("Warning test, ignore");
            Plugin.ErrorLog("Error test, ignore");
        }

        internal static void InfoLog(string message)
        {
            logger.LogInfo(message);
        }
        internal static void WarnLog(string message)
        {
            logger.LogWarning(message);
        }
        internal static void ErrorLog(string message)
        {
            logger.LogError(message);
        }

        public class Patches
        {
            [HarmonyPatch(typeof(SlimeController), nameof(SlimeController.UpdateSim))]
            [HarmonyPostfix]
            public static void ColorPatch(SlimeController __instance)
            {
                // Plugin.InfoLog("Patched player!");
                __instance.GetPlayerSprite().material.SetColor("_ShadowColor", new UnityEngine.Color(Plugin.boplRed.Value / 255, Plugin.boplGreen.Value / 255, Plugin.boplBlue.Value / 255));
            }
        }
    }
}