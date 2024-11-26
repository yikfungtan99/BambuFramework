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

        public virtual TemplateContainer SpawnUI(SettingsMenu menu, out List<Focusable> focussables)
        {
            TemplateContainer uiInstance = SettingsUI.CloneTree();

            // Assign data or behavior to the generated element (e.g., labels, sliders, toggles, etc.)

            SetTitle(uiInstance, Title);

            focussables = new List<Focusable>();

            return uiInstance;
        }

        protected abstract void Focus(VisualElement v);
        protected abstract void Blur(VisualElement v);

        public virtual void UpdateSettingOption()
        {
        }

        protected void SetTitle(TemplateContainer uiInstance, string title)
        {
            var label = uiInstance.Q<Label>("settingLabel");
            if (label != null) label.text = title;
        }
    }
}
