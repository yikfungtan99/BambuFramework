﻿using System.Collections.Generic;
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

        protected Label label;
        protected Button prevButton;
        protected Button nextButton;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            // Clone the base template
            base.SpawnUI(menu, out templateContainers, out fs);

            TemplateContainer uiInstance = templateContainers[0];

            // Query the components in the template
            label = uiInstance.Q<Label>("lblCurrentOption");
            prevButton = uiInstance.Q<Button>("btnPrev");
            nextButton = uiInstance.Q<Button>("btnNext");

            UpdateSettingOption();

            // Set up the Previous button functionality
            if (prevButton != null)
            {
                prevButton.clicked += () =>
                {
                    if (CyclerOptions.Count > 0)
                    {
                        currentIndex = (currentIndex - 1 + CyclerOptions.Count) % CyclerOptions.Count;
                        label.text = CyclerOptions[currentIndex];
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
                    if (CyclerOptions.Count > 0)
                    {
                        currentIndex = (currentIndex + 1) % CyclerOptions.Count;
                        label.text = CyclerOptions[currentIndex];
                        UIManager.Instance.Submit();
                        Bambu.Log($"Cycler value changed to: {CyclerOptions[currentIndex]}");
                    }
                };
            }

            fs.Add(prevButton);
            fs.Add(nextButton);


            //nextButton.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            //prevButton.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            //nextButton.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
            //prevButton.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        }

        public override void UpdateSettingOption()
        {
            // Initialize the label with the current option
            if (label != null && CyclerOptions.Count > 0)
            {
                label.text = CyclerOptions[currentIndex];
            }

        }
    }
}
