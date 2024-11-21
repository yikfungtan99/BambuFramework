using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public abstract class SettingOption
    {
        public abstract ESettingOptions SettingsOption { get; }
        protected VisualTreeAsset SettingsUI { get => SettingsContainer.Instance.SettingOptionsTemplateKVP[SettingsOption]; }

        public string Title;

        public virtual TemplateContainer SpawnUI(out List<Focusable> focussables)
        {
            TemplateContainer uiInstance = SettingsUI.CloneTree();

            // Assign data or behavior to the generated element (e.g., labels, sliders, toggles, etc.)
            var label = uiInstance.Q<Label>("settingLabel");
            if (label != null) label.text = Title;

            focussables = new List<Focusable>();

            return uiInstance;
        }
    }
}
