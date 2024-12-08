using System.Collections.Generic;
using UnityEngine;
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

        protected virtual void Focus(VisualElement template)
        {
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 155);
            UIManager.Instance.Select();
        }

        protected virtual void Blur(VisualElement template)
        {
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 0);
        }

        public abstract void UpdateSettingOption();

        protected void SetTitle(TemplateContainer uiInstance, string title)
        {
            var label = uiInstance.Q<Label>("settingLabel");
            if (label != null) label.text = title;
        }
    }
}
