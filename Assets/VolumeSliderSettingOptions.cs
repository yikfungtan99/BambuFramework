﻿using BambuFramework.Audio;
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
        public override float DefaultValue => SettingsManager.Instance.GetAudioVolume(channel);

        // Override the UI logic, keeping the functionality reusable
        public override TemplateContainer SpawnUI(out List<Focusable> fs)
        {
            TemplateContainer uiInstance = base.SpawnUI(out fs);

            var slider = uiInstance.Q<Slider>("CustomSlider");
            var textField = uiInstance.Q<TextField>("CustomTextField");

            if (slider != null)
            {
                slider.lowValue = MinValue;
                slider.highValue = MaxValue;
                slider.value = DefaultValue;

                if (textField != null)
                {
                    textField.value = ((int)DefaultValue).ToString();
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

            return uiInstance;
        }

        // Apply the volume setting (use a specific method in derived classes for actual setting)
        protected virtual void ApplyVolumeSetting(float volume)
        {
            // This method can be overridden in subclasses to apply the setting to specific systems
            // For example, master volume, music volume, or SFX volume
            SettingsManager.Instance.SetAudioVolume(channel, Mathf.FloorToInt(volume));
        }

        protected override void Focus(VisualElement template)
        {
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 155);
        }

        protected override void Blur(VisualElement template)
        {
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 0);
        }
    }
}
