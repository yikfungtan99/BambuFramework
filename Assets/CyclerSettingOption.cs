using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class CyclerSettingOption : SettingOption
    {
        [SerializeField] private string[] baseOptions;
        private int currentIndex = 0;

        public string[] CyclerOptions => baseOptions;
        public override ESettingOptions SettingsOption => ESettingOptions.CYCLER;

        public override TemplateContainer SpawnUI(out List<Focusable> fs)
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI(out fs);

            // Query the components in the template
            var label = uiInstance.Q<Label>("lblCurrentOption");
            var prevButton = uiInstance.Q<Button>("btnPrev");
            var nextButton = uiInstance.Q<Button>("btnNext");

            // Initialize the label with the current option
            if (label != null && CyclerOptions.Length > 0)
            {
                label.text = CyclerOptions[currentIndex];
            }

            // Set up the Previous button functionality
            if (prevButton != null)
            {
                prevButton.clicked += () =>
                {
                    if (CyclerOptions.Length > 0)
                    {
                        currentIndex = (currentIndex - 1 + CyclerOptions.Length) % CyclerOptions.Length;
                        label.text = CyclerOptions[currentIndex];
                        Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}");
                    }
                };
            }

            // Set up the Next button functionality
            if (nextButton != null)
            {
                nextButton.clicked += () =>
                {
                    if (CyclerOptions.Length > 0)
                    {
                        currentIndex = (currentIndex + 1) % CyclerOptions.Length;
                        label.text = CyclerOptions[currentIndex];
                        Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}");
                    }
                };
            }

            fs.Add(prevButton);
            fs.Add(nextButton);


            nextButton.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            prevButton.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            nextButton.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
            prevButton.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));


            return uiInstance;
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
