using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class SliderSettingOption : SettingOption
    {
        [SerializeField] private float minValue = 0f;
        [SerializeField] private float maxValue = 100f;
        [SerializeField] private float defaultValue = 50f;

        public virtual float MinValue => minValue;
        public virtual float MaxValue => maxValue;
        public virtual float Value => defaultValue;

        public override ESettingOptions SettingsOption => ESettingOptions.SLIDER;

        protected Slider slider;
        protected TextField textField;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);
            // Clone the base template
            TemplateContainer uiInstance = templateContainers[0];

            // Query the slider and text field elements
            slider = uiInstance.Q<Slider>("CustomSlider");
            textField = uiInstance.Q<TextField>("CustomTextField");

            // Configure the slider
            if (slider != null)
            {
                slider.lowValue = MinValue;
                slider.highValue = MaxValue;
                slider.value = Value;

                // Update the text field with the initial slider value
                if (textField != null)
                {
                    textField.value = ((int)Value).ToString(); // Display one decimal place
                }

                // Synchronize the slider with the text field
                slider.RegisterValueChangedCallback(evt =>
                {
                    float newValue = evt.newValue;

                    if (textField != null)
                    {
                        textField.value = ((int)newValue).ToString();
                    }

                    Bambu.Log($"Slider changed to: {newValue}", Debugging.ELogCategory.UI);
                });
            }
            else
            {
                Bambu.Log("Slider element with the name 'settingSlider' was not found in the UI template.", Debugging.ELogCategory.UI);
            }

            // Configure the text field
            if (textField != null)
            {
                textField.RegisterValueChangedCallback(evt =>
                {
                    if (float.TryParse(evt.newValue, out float newValue))
                    {
                        // Clamp the value within the slider range
                        newValue = Mathf.Clamp(newValue, MinValue, MaxValue);

                        // Update the slider value
                        if (slider != null)
                        {
                            slider.value = newValue;
                        }

                        // Update the text field to reflect the clamped value
                        textField.SetValueWithoutNotify(newValue.ToString());

                        Bambu.Log($"TextField changed to: {newValue}", Debugging.ELogCategory.UI);
                    }
                    else
                    {
                        // Reset the text field to the slider's current value if parsing fails
                        if (slider != null)
                        {
                            textField.SetValueWithoutNotify(((int)slider.value).ToString());
                        }

                        Bambu.Log("Invalid input in TextField. Resetting to current slider value.", Debugging.ELogCategory.UI);
                    }
                });
            }
            else
            {
                Bambu.Log("TextField element with the name 'sliderValueField' was not found in the UI template.", Debugging.ELogCategory.UI);
            }

            fs.Add(slider);

            slider.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            slider.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        }

        public override void UpdateSettingOption()
        {

        }
    }
}
