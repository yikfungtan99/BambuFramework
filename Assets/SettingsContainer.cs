using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [CreateAssetMenu(fileName = "Settings Container", menuName = "BAMBU/SETTINGS/Settings Container")]
    public class SettingsContainer : SingletonSerializedScriptableObject<SettingsContainer>
    {
        public Dictionary<ESettingOptions, VisualTreeAsset> SettingOptionsTemplateKVP = new Dictionary<ESettingOptions, VisualTreeAsset>();
        public List<SettingOption> settingOptions;
    }
}
