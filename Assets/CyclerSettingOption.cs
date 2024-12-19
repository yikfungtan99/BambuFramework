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

        protected Button leftButton;
        protected Button middleButton;
        protected Button rightButton;

        protected Button prevButton;
        protected Button nextButton;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            // Clone the base template
            base.SpawnUI(menu, out templateContainers, out fs);

            TemplateContainer uiInstance = templateContainers[0];

            // Query the components in the template
            leftButton = uiInstance.Q<Button>("btnLeft");
            middleButton = uiInstance.Q<Button>("btnMiddle");
            rightButton = uiInstance.Q<Button>("btnRight");
            prevButton = uiInstance.Q<Button>("btnPrev");
            nextButton = uiInstance.Q<Button>("btnNext");

            UpdateSettingOption();

            // Set up the Previous button functionality
            if (prevButton != null)
            {
                prevButton.clicked += () =>
                {
                    Previous();
                };
            }

            // Set up the Next button functionality
            if (nextButton != null)
            {
                nextButton.clicked += () =>
                {
                    Next();
                };
            }

            if (leftButton != null)
            {
                leftButton.clicked += () =>
                {
                    if (currentIndex == CyclerOptions.Count - 1)
                    {
                        Previous();
                    }

                    Previous();
                };
            }

            if (middleButton != null)
            {
                middleButton.clicked += () =>
                {
                    if (currentIndex == 0)
                    {
                        Next();
                    }
                    else if (currentIndex == CyclerOptions.Count - 1)
                    {
                        Previous();
                    }
                };
            }

            if (rightButton != null)
            {
                rightButton.clicked += () =>
                {
                    if (currentIndex == 0)
                    {
                        Next();
                    }

                    Next();
                };
            }

            fs.Add(prevButton);
            fs.Add(nextButton);
        }

        private void Next()
        {
            if (CyclerOptions.Count > 0 && currentIndex < CyclerOptions.Count - 1)
            {
                currentIndex++;
                UpdateSettingOption();
                UIManager.Instance.Submit();
                Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}", Debugging.ELogCategory.UI);
            }
        }

        private void Previous()
        {
            if (CyclerOptions.Count > 0 && currentIndex > 0)
            {
                currentIndex--;
                UpdateSettingOption();
                UIManager.Instance.Submit();
                Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}", Debugging.ELogCategory.UI);
            }
        }

        public override void UpdateSettingOption()
        {
            if (CyclerOptions.Count == 0) return;

            StyleColor selectedColor = new StyleColor(new Color(1f, 1f, 1f, 0.5f));
            StyleColor fadedColor = new StyleColor(new Color(1f, 1f, 1f, 0.15f));

            // Update left label (current selection)
            if (middleButton != null)
            {
                string middle = CyclerOptions[currentIndex];
                middleButton.style.backgroundColor = selectedColor; // Faded white background

                if (currentIndex == 0)
                {
                    middle = CyclerOptions[1];
                    middleButton.style.backgroundColor = fadedColor; // Faded white background
                }

                if (currentIndex == CyclerOptions.Count - 1)
                {
                    middle = CyclerOptions[CyclerOptions.Count - 2];
                    middleButton.style.backgroundColor = fadedColor; // Faded white background
                }

                middleButton.text = middle;
            }

            // Update left label (previous selection)
            if (leftButton != null)
            {
                string left = currentIndex > 0 ? CyclerOptions[currentIndex - 1] : CyclerOptions[0];
                leftButton.style.backgroundColor = fadedColor; // Faded white background

                if (currentIndex == 0)
                {
                    leftButton.style.backgroundColor = selectedColor; // Faded white background
                }

                if (currentIndex == CyclerOptions.Count - 1)
                {
                    left = CyclerOptions[CyclerOptions.Count - 3];
                }

                leftButton.text = left;
                //leftButton.style.opacity = currentIndex > 0 ? 1f : 0.5f; // Dim if no previous option
            }

            // Update right label (next selection)
            if (rightButton != null)
            {
                string right = currentIndex < CyclerOptions.Count - 1 ? CyclerOptions[currentIndex + 1] : CyclerOptions[CyclerOptions.Count - 1];
                rightButton.style.backgroundColor = fadedColor; // Faded white background

                if (currentIndex == 0)
                {
                    right = CyclerOptions[2];
                }

                if (currentIndex == CyclerOptions.Count - 1)
                {
                    rightButton.style.backgroundColor = selectedColor; // Faded white background
                }

                rightButton.text = right;
            }
        }
    }
}
