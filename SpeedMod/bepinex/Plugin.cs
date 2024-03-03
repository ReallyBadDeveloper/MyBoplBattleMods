using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using System.IO;


namespace SpeedModifier
{
    [BepInPlugin("com.rbdev.SpeedModifier", "SpeedModifier", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<float> speedUpValue;
        private void Awake()
        {
            Logger.LogInfo("Plugin SpeedModifier is loaded!");

            if (!File.Exists("/BepInEx/config/com.rbdev.SpeedModifier.cfg"))
            {
                speedUpValue = Config.Bind("General",
                                                    "Speed Up Value",
                                                    1f,
                                                    "The speed the game runs at. Players in an online match will follow the lowest setting set by a player.");
            }
            Time.timeScale = speedUpValue.Value;
        }
    }
}
