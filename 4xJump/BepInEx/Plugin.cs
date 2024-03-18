using BepInEx;
using BepInEx.Configuration;
using BoplFixedMath;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace FourXJump
{
    [BepInPlugin("com.rbdev.4xJump", "4x Jump", "1.0.0")]
    [BepInProcess("BoplBattle.exe")]

    public class Plugin : BaseUnityPlugin
    {
        public ConfigEntry<float> jumpHeight;
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin 4x Jump is loaded!");

            // failed config system, might bring it back?
            /*
            if (!File.Exists("/BepInEx/config/com.rbdev.JumpMod.cfg"))
                {
                    Logger.LogInfo("it works?");
                    jumpHeight = Config.Bind("General",
                                                        "Jump Height",
                                                        30f,
                                                        "The height players jump. WARNING: DESYNC WILL OCCUR IF OTHER PLAYERS ARE USING DIFFERENT VALUES!");
                }
            */
            Harmony harmony = new Harmony("com.rbdev.4xJump");

            MethodInfo original = AccessTools.Method(typeof(PlayerPhysics), "Jump");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "JumpPatch");

            harmony.Patch(original,new HarmonyMethod(patch));
        }
    }
    public class Patches {
        public static void JumpPatch(PlayerPhysics __instance) {
            __instance.jumpStrength = (Fix)120L;
        }
    }
}
/*
EEEEEEEEEEEEEEEEEEEEEE
E::::::::::::::::::::E
E::::::::::::::::::::E
EE::::::EEEEEEEEE::::E
  E:::::E       EEEEEE
  E:::::E             
  E::::::EEEEEEEEEE   
  E:::::::::::::::E   
  E:::::::::::::::E   
  E::::::EEEEEEEEEE   
  E:::::E             
  E:::::E       EEEEEE
EE::::::EEEEEEEE:::::E
E::::::::::::::::::::E
E::::::::::::::::::::E
EEEEEEEEEEEEEEEEEEEEEE
*/