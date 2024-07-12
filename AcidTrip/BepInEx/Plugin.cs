using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System;
using System.Data.SqlTypes;
using System.Reflection;

namespace AcidTrip
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static Random random = new();
        internal static int localPlayerId = 1;
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Harmony harmony = new(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(Patches));
        }
    }
    public class Patches
    {
        [HarmonyPatch(typeof(Player), nameof(Player.HorizontalAxis))]
        [HarmonyPostfix]
        public static void HorizPostfix(Player __instance,ref Fix __result) {
            Plugin.print("Patched!");
            __result = -__instance.Inputs.InputVector.x;
        }
        [HarmonyPatch(typeof(Player), nameof(Player.VerticalAxis))]
        [HarmonyPostfix]
        public static void VertPostfix(Player __instance, ref Fix __result)
        {
            if (!__instance.IsLocalPlayer) return;
            Plugin.print("Patched!");
            __result = -__instance.Inputs.InputVector.y;
        }
        [HarmonyPatch(typeof(Player), nameof(Player.InputVector))]
        [HarmonyPostfix]
        public static void Vec2Postfix(Player __instance, ref Vec2 __result)
        {
            if (!__instance.IsLocalPlayer) return;
            Vec2 newVec = new Vec2(-__instance.Inputs.InputVector.x, -__instance.Inputs.InputVector.y);
            Plugin.print("Patched!");
            __result = newVec;
        }
        [HarmonyPatch(typeof(Player), nameof(Player.AbilityButtonIsDown))]
        [HarmonyPrefix]
        public static void AbilityPrefix(ref int __0, Player __instance)
        {
            if (!__instance.IsLocalPlayer) return;
            if (__0 == 0)
            {
                __0 = 2;
            } else if (__0 == 2)
            {
                __0 = 0;
            }
        }

        // stolen from Custom Colors
        [HarmonyPatch(typeof(PlayerBody), nameof(PlayerBody.UpdateSim))]
        [HarmonyPostfix]
        public static void ColorPatch(ref IPlayerIdHolder ___idHolder)
        {
            if (Plugin.random.Next(11) < 5) return;
            foreach (SlimeController sc in GetSlimeControllers())
            {
                if (sc.GetPlayerId() == Plugin.localPlayerId && sc != null)
                {
                    UnityEngine.Color prevColor = sc.GetPlayerSprite().material.GetColor("_ShadowColor");
                    sc.GetPlayerSprite().material.SetColor("_ShadowColor", new UnityEngine.Color(-prevColor.r + 1, -prevColor.g + 1, -prevColor.b + 1));
                }
            }
        }
        [HarmonyPatch(typeof(Player), nameof(Player.UpdateFRAMEDependentInputs))]
        [HarmonyPrefix]
        public static void PlayerPatch(Player __instance)
        {
            if (__instance.IsLocalPlayer)
            {
                Plugin.localPlayerId = __instance.Id;
            }
        }


        // useful snippets from splotch
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
