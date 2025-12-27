using BepInEx;
using UnityEngine.InputSystem;

namespace F11ToFullscreen
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static bool fullscreen;
        
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Update()
        {
            fullscreen = Settings.instance.FullScreen == 1;
            if (Keyboard.current.f11Key.wasPressedThisFrame)
            {
                Settings.instance.FullScreen = Settings.instance.FullScreen == 0 ? 1 : 0;
                Settings.instance.Save();
                Logger.LogInfo("Switched to "+(Settings.instance.FullScreen == 1 ? "fullscreen" : "windowed")+" mode");
            }
        }
    }
}
