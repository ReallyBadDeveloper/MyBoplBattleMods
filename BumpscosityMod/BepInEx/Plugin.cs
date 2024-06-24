using BepInEx;
using System;
using UnityEngine;

namespace BumpscosityMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public float bumpscosity = 0.0f;
        public float colorTimer = 0.0f;
        public Color guiColor;
        public Rect labelPos = new(25, 50, 100, 50);
        private void Update()
        {
            float r = Mathf.Abs(Mathf.Sin(colorTimer * 0.4f));
            float g = Mathf.Abs(Mathf.Sin(colorTimer * 0.5f));
            float b = Mathf.Abs(Mathf.Sin(colorTimer * 0.6f));
            guiColor = new Color(r, g, b);
            colorTimer += Time.deltaTime;
        }
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
        private void OnGUI () {
            GUI.color = guiColor;
            GUI.Window(0, new Rect(25, 75, 150, 100), BumpscosityWindow, "Bumpscosity Modifier");
        }
        private void BumpscosityWindow(int id)
        {
            GUI.color = guiColor;
            bumpscosity = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), bumpscosity, 0.0f, 10.0f);
            if (bumpscosity == 0.0f) GUI.Label(labelPos, "Bumpscosity: Non-Sigma");
            if (bumpscosity == 10.0f) GUI.Label(labelPos, "Bumpscosity: 100% SIGMA");
            if (bumpscosity > 0.0f && bumpscosity < 10.0f) GUI.Label(labelPos, "Bumpscosity: " + Math.Floor(bumpscosity).ToString());
        }
    }
}
