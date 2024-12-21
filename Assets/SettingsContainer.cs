using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [CreateAssetMenu(fileName = "Settings Container", menuName = "BAMBU/SETTINGS/Settings Container")]
    public class SettingsContainer : SingletonSerializedScriptableObject<SettingsContainer>
    {
        // Templates for settings options UI
        public Dictionary<ESettingOptions, VisualTreeAsset> SettingOptionsTemplateKVP = new Dictionary<ESettingOptions, VisualTreeAsset>();

        // Supported video frame rates
        public List<int> VideoFrameRates = new List<int> { 30, 60, 120, 144, 240 };

        // Default Settings Values
        [Header("Default Gameplay Settings")]
        public bool DefaultGameplayConsole = false;

        [Header("Default Video Settings")]
        public int DefaultVideoWindowMode = 0; // 0: Fullscreen, 1: Windowed, 2: Borderless
        public bool DefaultVsync = false;

        [Header("Default Audio Settings")]
        public int DefaultAudioMaster = 50; // Default master volume percentage
        public int DefaultAudioSFX = 50;    // Default SFX volume percentage
        public int DefaultAudioMusic = 50;  // Default music volume percentage
        public int DefaultAudioUI = 50;  // Default music volume percentage

        [Header("Default Input Settings")]
        [OdinSerialize]
        [NonSerialized]
        public Dictionary<string, string> DefaultInputBindings = new Dictionary<string, string>
        {
            { "Move_Forward", "<Keyboard>/w" },
            { "Move_Backward", "<Keyboard>/s" },
            { "Move_Left", "<Keyboard>/a" },
            { "Move_Right", "<Keyboard>/d" },
            { "Jump", "<Keyboard>/space" },
            { "Fire", "<Mouse>/leftButton" },
        };

        // Tabs for UI representation of settings
        [OdinSerialize][NonSerialized] public List<SettingsTab> Tabs = new List<SettingsTab>();
    }
}
