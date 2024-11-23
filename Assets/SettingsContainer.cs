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
        public Dictionary<ESettingOptions, VisualTreeAsset> SettingOptionsTemplateKVP = new Dictionary<ESettingOptions, VisualTreeAsset>();
        public List<int> VideoFrameRates;

        // List of tabs, each containing its own setting options
        [OdinSerialize][NonSerialized] public List<SettingsTab> Tabs = new List<SettingsTab>();
    }
}
