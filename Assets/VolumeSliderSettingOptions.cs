using BambuFramework.Audio;
using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class VolumeSliderSettingOption : SliderSettingOption
    {
        // Overrideable properties for setting different volume types (Master, SFX, Music)
        [SerializeField] private EAudioChannel channel;

        // These can be overridden in derived classes to set specific volume ranges
        public override float MinValue => 0f;
        public override float MaxValue => 100f;
        public override float Value => SettingsManager.Instance.GetAudioVolume(channel);

        // Override the UI logic, keeping the functionality reusable
        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);
            TemplateContainer uiInstance = templateContainers[0];

            slider = uiInstance.Q<Slider>("CustomSlider");
            textField = uiInstance.Q<TextField>("CustomTextField");

            if (slider != null)
            {
                slider.lowValue = MinValue;
                slider.highValue = MaxValue;
                slider.value = Value;

                if (textField != null)
                {
                    textField.value = ((int)Value).ToString();
                }

                // Update text field on slider change
                slider.RegisterValueChangedCallback(evt =>
                {
                    float newValue = evt.newValue;
                    if (textField != null)
                    {
                        textField.value = ((int)newValue).ToString();
                    }
                    ApplyVolumeSetting(newValue);
                    Bambu.Log($"{channel} volume changed to: {newValue}", Debugging.ELogCategory.UI);
                });
            }

            if (textField != null)
            {
                textField.RegisterValueChangedCallback(evt =>
                {
                    if (float.TryParse(evt.newValue, out float newValue))
                    {
                        newValue = Mathf.Clamp(newValue, MinValue, MaxValue);
                        if (slider != null) slider.value = newValue;
                        textField.SetValueWithoutNotify(newValue.ToString());
                        ApplyVolumeSetting(newValue);
                        Bambu.Log($"{channel} volume changed to: {newValue}", Debugging.ELogCategory.UI);
                    }
                    else
                    {
                        if (slider != null)
                        {
                            textField.SetValueWithoutNotify(((int)slider.value).ToString());
                        }
                        Bambu.Log($"Invalid input in TextField for {channel}. Resetting to current slider value.", Debugging.ELogCategory.UI);
                    }
                });
            }

            fs.Add(slider);

            slider.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            slider.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        }

        // Apply the volume setting (use a specific method in derived classes for actual setting)
        protected virtual void ApplyVolumeSetting(float volume)
        {
            // This method can be overridden in subclasses to apply the setting to specific systems
            // For example, master volume, music volume, or SFX volume
            SettingsManager.Instance.SetAudioVolume(channel, Mathf.FloorToInt(volume));
        }

        public override void UpdateSettingOption()
        {
            float value = Value;
            slider.SetValueWithoutNotify(value);
            textField.SetValueWithoutNotify(((int)value).ToString());
        }
    }
}
