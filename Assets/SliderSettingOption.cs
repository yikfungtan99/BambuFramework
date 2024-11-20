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

        public float MinValue => minValue;
        public float MaxValue => maxValue;
        public float DefaultValue => defaultValue;

        public override ESettingOptions SettingsOption => ESettingOptions.SLIDER;

        public override TemplateContainer SpawnUI()
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI();

            // Query the slider and text field elements
            var slider = uiInstance.Q<Slider>("CustomSlider");
            var textField = uiInstance.Q<TextField>("CustomTextField");

            // Configure the slider
            if (slider != null)
            {
                slider.lowValue = MinValue;
                slider.highValue = MaxValue;
                slider.value = DefaultValue;

                // Update the text field with the initial slider value
                if (textField != null)
                {
                    textField.value = ((int)DefaultValue).ToString(); // Display one decimal place
                }

                // Synchronize the slider with the text field
                slider.RegisterValueChangedCallback(evt =>
                {
                    float newValue = evt.newValue;

                    if (textField != null)
                    {
                        textField.value = ((int)newValue).ToString();
                    }

                    Bambu.Log($"Slider changed to: {newValue}");
                });
            }
            else
            {
                Bambu.Log("Slider element with the name 'settingSlider' was not found in the UI template.");
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

                        Bambu.Log($"TextField changed to: {newValue}");
                    }
                    else
                    {
                        // Reset the text field to the slider's current value if parsing fails
                        if (slider != null)
                        {
                            textField.SetValueWithoutNotify(((int)slider.value).ToString());
                        }

                        Bambu.Log("Invalid input in TextField. Resetting to current slider value.");
                    }
                });
            }
            else
            {
                Bambu.Log("TextField element with the name 'sliderValueField' was not found in the UI template.");
            }

            return uiInstance;
        }
    }
}
