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

        public virtual void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> focussables)
        {
            templateContainers = new List<TemplateContainer> { SettingsUI.CloneTree() };

            focussables = new List<Focusable>();

            // Assign data or behavior to the generated element (e.g., labels, sliders, toggles, etc.)

            SetTitle(templateContainers[0], Title);
        }

        protected abstract void Focus(VisualElement v);
        protected abstract void Blur(VisualElement v);

        public abstract void UpdateSettingOption();

        protected void SetTitle(TemplateContainer uiInstance, string title)
        {
            var label = uiInstance.Q<Label>("settingLabel");
            if (label != null) label.text = title;
        }
    }
}
