using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class CyclerSettingOption : SettingOption
    {
        [SerializeField] private List<string> baseOptions = new List<string>();
        private int currentIndex = 0;

        public virtual List<string> CyclerOptions => baseOptions;
        public override ESettingOptions SettingsOption => ESettingOptions.CYCLER;

        protected Label leftLabel;
        protected Label middleLabel;
        protected Label rightLabel;
        protected Button prevButton;
        protected Button nextButton;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            // Clone the base template
            base.SpawnUI(menu, out templateContainers, out fs);

            TemplateContainer uiInstance = templateContainers[0];

            // Query the components in the template
            leftLabel = uiInstance.Q<Label>("lblLeftOption");
            middleLabel = uiInstance.Q<Label>("lblMiddleOption");
            rightLabel = uiInstance.Q<Label>("lblRightOption");
            prevButton = uiInstance.Q<Button>("btnPrev");
            nextButton = uiInstance.Q<Button>("btnNext");

            UpdateSettingOption();

            // Set up the Previous button functionality
            if (prevButton != null)
            {
                prevButton.clicked += () =>
                {
                    if (CyclerOptions.Count > 0 && currentIndex > 0)
                    {
                        currentIndex--;
                        UpdateSettingOption();
                        UIManager.Instance.Submit();
                        Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}", Debugging.ELogCategory.SETTING);
                    }
                };
            }

            // Set up the Next button functionality
            if (nextButton != null)
            {
                nextButton.clicked += () =>
                {
                    if (CyclerOptions.Count > 0 && currentIndex < CyclerOptions.Count - 1)
                    {
                        currentIndex++;
                        UpdateSettingOption();
                        UIManager.Instance.Submit();
                        Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}", Debugging.ELogCategory.SETTING);
                    }
                };
            }

            fs.Add(prevButton);
            fs.Add(nextButton);
        }

        public override void UpdateSettingOption()
        {
            if (CyclerOptions.Count == 0) return;

            StyleColor selectedColor = new StyleColor(new Color(1f, 1f, 1f, 0.5f));
            StyleColor fadedColor = new StyleColor(new Color(1f, 1f, 1f, 0.15f));

            // Update left label (current selection)
            if (middleLabel != null)
            {
                string middle = CyclerOptions[currentIndex];
                middleLabel.style.backgroundColor = selectedColor; // Faded white background

                if (currentIndex == 0)
                {
                    middle = CyclerOptions[1];
                    middleLabel.style.backgroundColor = fadedColor; // Faded white background
                }

                if (currentIndex == CyclerOptions.Count - 1)
                {
                    middle = CyclerOptions[CyclerOptions.Count - 2];
                    middleLabel.style.backgroundColor = fadedColor; // Faded white background
                }

                middleLabel.text = middle;
            }

            // Update left label (previous selection)
            if (leftLabel != null)
            {
                string left = currentIndex > 0 ? CyclerOptions[currentIndex - 1] : CyclerOptions[0];
                leftLabel.style.backgroundColor = fadedColor; // Faded white background

                if (currentIndex == 0)
                {
                    leftLabel.style.backgroundColor = selectedColor; // Faded white background
                }

                if (currentIndex == CyclerOptions.Count - 1)
                {
                    left = CyclerOptions[CyclerOptions.Count - 3];
                }

                leftLabel.text = left;
                //leftLabel.style.opacity = currentIndex > 0 ? 1f : 0.5f; // Dim if no previous option
            }

            // Update right label (next selection)
            if (rightLabel != null)
            {
                string right = currentIndex < CyclerOptions.Count - 1 ? CyclerOptions[currentIndex + 1] : CyclerOptions[CyclerOptions.Count - 1];
                rightLabel.style.backgroundColor = fadedColor; // Faded white background

                if (currentIndex == 0)
                {
                    right = CyclerOptions[2];
                }

                if (currentIndex == CyclerOptions.Count - 1)
                {
                    rightLabel.style.backgroundColor = selectedColor; // Faded white background
                }

                rightLabel.text = right;
            }
        }
    }
}
