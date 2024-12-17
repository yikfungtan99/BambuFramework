using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class FramerateCyclerSettingOption : CyclerSettingOption
    {
        private int currentIndex;

        private List<string> frameRates = new List<string>();
        public override List<string> CyclerOptions
        {
            get
            {
                if (frameRates == null || frameRates.Count <= 0)
                {
                    frameRates = new List<string>();

                    foreach (var item in SettingsContainer.Instance.VideoFrameRates)
                    {
                        frameRates.Add(item.ToString());
                    }
                }

                return frameRates;
            }
        }

        //public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        //{
        //    base.SpawnUI(menu, out templateContainers, out fs);

        //    // Create the base UI instance using the inherited method
        //    TemplateContainer uiInstance = templateContainers[0];

        //    // Query the components in the template
        //    var label = uiInstance.Q<Label>("lblCurrentOption");
        //    var prevButton = uiInstance.Q<Button>("btnPrev");
        //    var nextButton = uiInstance.Q<Button>("btnNext");

        //    currentIndex = SettingsManager.Instance.VideoFramerate;
        //    currentIndex = Mathf.Clamp(currentIndex, 0, CyclerOptions.Count - 1);

        //    // Initialize the label with the current option (default to 60 FPS)
        //    if (label != null && CyclerOptions != null && CyclerOptions.Count > 0)
        //    {
        //        label.text = CyclerOptions[currentIndex];
        //    }

        //    // Set up the Previous button functionality
        //    if (prevButton != null)
        //    {
        //        prevButton.clicked += () =>
        //        {
        //            if (CyclerOptions.Count > 0)
        //            {
        //                currentIndex = (currentIndex - 1 + CyclerOptions.Count) % CyclerOptions.Count;
        //                label.text = CyclerOptions[currentIndex];
        //                ApplyFramerateSetting(currentIndex);
        //                Bambu.Log($"Framerate changed to: {CyclerOptions[currentIndex]}", Debugging.ELogCategory.UI);
        //            }
        //        };
        //    }

        //    // Set up the Next button functionality
        //    if (nextButton != null)
        //    {
        //        nextButton.clicked += () =>
        //        {
        //            if (CyclerOptions.Count > 0)
        //            {
        //                currentIndex = (currentIndex + 1) % CyclerOptions.Count;
        //                label.text = CyclerOptions[currentIndex];
        //                ApplyFramerateSetting(currentIndex);
        //                Bambu.Log($"Framerate changed to: {CyclerOptions[currentIndex]}", Debugging.ELogCategory.UI);
        //            }
        //        };
        //    }

        //    fs.Add(prevButton);
        //    fs.Add(nextButton);

        //    // Handle focus and blur events for visual feedback
        //    nextButton.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
        //    prevButton.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
        //    nextButton.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        //    prevButton.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        //}

        private void ApplyFramerateSetting(int currentIndex)
        {
            SettingsManager.Instance.SetVideoFramerate(currentIndex);
        }

        public override void UpdateSettingOption()
        {
            currentIndex = SettingsManager.Instance.VideoFramerate;
            currentIndex = Mathf.Clamp(currentIndex, 0, CyclerOptions.Count - 1);

            base.UpdateSettingOption();

            //// Initialize the label with the current option (default to 60 FPS)
            //if (label != null && CyclerOptions != null && CyclerOptions.Count > 0)
            //{
            //    label.text = CyclerOptions[currentIndex];
            //}
        }
    }
}
