using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace MoreLives
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInIncompatibility("com.MorePlayersTeam.MorePlayers")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<int> baseHealth;
        public static List<int> pHealth = new List<int>();
        public static ConfigEntry<int> baseIFrames;
        public static List<int> iframes = new List<int>();
        private void Awake()
        {
            baseHealth = Config.Bind("General", "Life Count", 3, "The amount of lives each player starts with.");
            baseIFrames = Config.Bind("General", "I-Frames", 5, "The amount of invincibility frames (i-frames) each player gets after getting hit.");
            for (int i = 0; i < 4; i++)
            {
                pHealth.Add(baseHealth.Value);
            }
            for (int i = 0; i < 4; i++)
            {
                iframes.Add(0);
            }
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Harmony harmony = new(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(Patches));
        }
    }
    public class Patches {
        [HarmonyPatch(typeof(PlayerCollision), nameof(PlayerCollision.killPlayer))]
        [HarmonyPrefix]
        public static bool KillPatch(PlayerCollision __instance, CauseOfDeath __3)
        {
            int pid = __instance.GetPlayerId() - 1; // converts to zero-based index
            if (Plugin.iframes[pid] > 0) {
                Plugin.iframes[pid]--;
                return false;
            }
            if (Plugin.pHealth[pid] < 1)
            {
                Plugin.pHealth[pid] = Plugin.baseHealth.Value;
                return true;
            } else
            {
                Plugin.pHealth[pid]--;
                Plugin.iframes[pid] = Plugin.baseIFrames.Value;
                return false;
            }
        }
    }
}
