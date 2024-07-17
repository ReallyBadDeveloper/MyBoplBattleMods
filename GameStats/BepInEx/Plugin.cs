using System;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Microsoft.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

namespace GameStats
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal bool isActive = false;
        internal DateTime _lastTime;
        internal int _framesRendered = 0;
        internal int _fps;
        internal int _frameCount;
        internal GameObject[] allObjects;
        internal PhysicsBodyList<Circle> circles;
        internal PhysicsBodyList<Box> boxes;
        internal int framesUntilCheck = 60;
        public Rect windowRect = new Rect(Screen.width - 220, 60, 200, 175);
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
        void Update()
        {
            if (framesUntilCheck == 0)
            {
                allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                if (DetPhysics.Get() != null)
                {
                    circles = DetPhysics.Get().circles;
                    boxes = DetPhysics.Get().boxes;
                }
                framesUntilCheck = 60;
            } else
            {
                framesUntilCheck--;
            }
            _framesRendered++;
            _frameCount++;

            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
            {
                _fps = _framesRendered;
                _framesRendered = 0;
                _lastTime = DateTime.Now;
            }     
        }

        private void OnGUI()
        {
            GUI.backgroundColor = new UnityEngine.Color(255f/255f, 74f/255f, 246f/255f);
            if (GUI.Button(new Rect(Screen.width - 145, 20, 125, 30), "Show/Hide Stats"))
            {
                isActive = !isActive;
            }
            if (isActive)
            {
                windowRect = GUI.Window(1001, new Rect(Screen.width - 195, 60, 175, 175), StatsMenu, "Stats");
            }
        }
        void StatsMenu(int windowID)
        {
            GUI.DragWindow();
            GUI.Label(new Rect(10, 20, 100, 20), $"FPS: {_fps}");
            GUI.Label(new Rect(10, 40, 100, 20), $"Frames: {_frameCount}");
            GUI.Label(new Rect(10, 60, 200, 20), $"GameObjects: {allObjects.Length}");
            if (boxes != null && circles != null)
            {
                GUI.Label(new Rect(10, 80, 200, 20), $"PhysObjects: {boxes.Length + circles.Length}");
            } else
            {
                GUI.Label(new Rect(10, 80, 200, 20), $"PhysObjects: {0}");
            }
            GUI.Label(new Rect(10, 100, 200, 20), $"Scene: {SceneManager.GetActiveScene().name}");
            GUI.Label(new Rect(10, 120, 200, 20), $"Connected: {System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()}");
            GUI.Label(new Rect(10, 145, 200, 30), $"Made by @reallybaddev");
        }
    }
}
