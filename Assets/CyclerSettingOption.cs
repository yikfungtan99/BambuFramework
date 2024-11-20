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

        public override TemplateContainer SpawnUI()
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI();

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

            return uiInstance;
        }
    }
}
