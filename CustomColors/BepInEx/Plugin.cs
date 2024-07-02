using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace CustomColors
{
    [BepInPlugin("com.rbdev.CustomColors", "CustomColors", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        internal static float red = 255f;
        internal static float green = 255f;
        internal static float blue = 255f;
        internal static int localPlayerId = 1;
        internal static bool customColorsWindow = true;
        private static ManualLogSource logger;
        private void Awake()
        {
            logger = base.Logger;
            Logger.LogInfo("Plugin CustomColors is loaded!");
            Harmony harmony = new("com.rbdev.CustomColors");
            harmony.PatchAll(typeof(Patches));
            Plugin.InfoLog("Log test, ignore");
            Plugin.WarnLog("Warning test, ignore");
            Plugin.ErrorLog("Error test, ignore");
        }
        void CustomColorsWindow(int windowID)
        {
            GUI.color = new UnityEngine.Color(red / 255, 0f, 0f);
            red = GUI.HorizontalSlider(new Rect(10, 20, 150, 30), red, 0.0f, 255.0f);
            GUI.color = new UnityEngine.Color(0f, green / 255, 0f);
            green = GUI.HorizontalSlider(new Rect(10, 45, 150, 30), green, 0.0f, 255.0f);
            GUI.color = new UnityEngine.Color(0f, 0f, blue / 255);
            blue = GUI.HorizontalSlider(new Rect(10, 70, 150, 30), blue, 0.0f, 255.0f);

            GUI.color = new UnityEngine.Color(red / 255, 0f, 0f);
            red = float.Parse(GUI.TextField(new Rect(200, 20, 50, 20), red.ToString(), 5));
            GUI.color = new UnityEngine.Color(0f, green / 255, 0f);
            green = float.Parse(GUI.TextField(new Rect(200, 45, 50, 20), green.ToString(), 5));
            GUI.color = new UnityEngine.Color(0f, 0f, blue / 255);
            blue = float.Parse(GUI.TextField(new Rect(200, 70, 50, 20), blue.ToString(), 5));

            GUI.color = new UnityEngine.Color(red / 255, 0f, 0f);
            GUI.Label(new Rect(255, 20, 50, 20), "███████");
            GUI.color = new UnityEngine.Color(0f, green / 255, 0f);
            GUI.Label(new Rect(255, 45, 50, 20), "███████");
            GUI.color = new UnityEngine.Color(0f, 0f, blue / 255);
            GUI.Label(new Rect(255, 70, 50, 20), "███████");
            GUI.color = new UnityEngine.Color(red / 255, green / 255, blue / 255);
            GUI.Label(new Rect(25, 95, 200, 20), "███████████████████████████████████");
            GUI.DragWindow();
        }
        private void OnGUI() 
        {
            GUI.color = Color.yellow;
            if (GUI.Button(new Rect(25, 100, 100, 20), "Custom Colors"))
            { 
                if (customColorsWindow)
                {
                    customColorsWindow = false;
                } else
                {
                    customColorsWindow = true;
                }
            }

            if (customColorsWindow)
            {
                GUI.color = Color.yellow;
                GUI.Window(42069, new Rect(10, 125, 300, 125), CustomColorsWindow, "Custom Colors");
            }
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
            [HarmonyPatch(typeof(PlayerBody), nameof(PlayerBody.UpdateSim))]
            [HarmonyPostfix]
            public static void ColorPatch(ref IPlayerIdHolder ___idHolder)
            {
                foreach (SlimeController sc in GetSlimeControllers()) {
                    if (sc.GetPlayerId() == localPlayerId && sc != null)
                    {
                        Plugin.print("found a match!");
                        sc.GetPlayerSprite().material.SetColor("_ShadowColor", new UnityEngine.Color(Plugin.red / 255, Plugin.green / 255, Plugin.blue / 255));
                    }
                }
            }
            [HarmonyPatch(typeof(Player), nameof(Player.UpdateFRAMEDependentInputs))]
            [HarmonyPrefix]
            public static void PlayerPatch(Player __instance)
            {
                if (__instance.IsLocalPlayer)
                {
                    localPlayerId = __instance.Id;
                }
            }

            // thank you spotch <3
            public static GameSessionHandler GetGameSessionHandler()
            {
                FieldInfo selfRefField = typeof(GameSessionHandler).GetField("selfRef", BindingFlags.Static | BindingFlags.NonPublic);
                return selfRefField.GetValue(null) as GameSessionHandler;
            }
            public static SlimeController[] GetSlimeControllers()
            {
                FieldInfo slimeControllersField = typeof(GameSessionHandler).GetField("slimeControllers", BindingFlags.Instance | BindingFlags.NonPublic);
                return slimeControllersField.GetValue(GetGameSessionHandler()) as SlimeController[];
            }
        }
    }
}
