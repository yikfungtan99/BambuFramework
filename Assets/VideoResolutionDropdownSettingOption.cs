using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class VideoResolutionDropdownSettingOption : CyclerSettingOption
    {
        List<Vector2Int> resolutions;

        private List<string> resolutionOptions;

        // Override the Dropdown options property
        public override List<string> CyclerOptions => resolutionOptions;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            // Populate resolution options dynamically
            resolutions = new List<Vector2Int>();
            resolutionOptions = new List<string>();
            foreach (var resolution in Screen.resolutions)
            {
                resolutions.Add(new Vector2Int(resolution.width, resolution.height));
                resolutionOptions.Add($"{resolution.width} x {resolution.height}");
            }

            base.SpawnUI(menu, out templateContainers, out fs);
        }

        protected override void OnCycle()
        {
            base.OnCycle();
            int index = Closestindex(resolutions[currentIndex]);
            SetResolution(resolutions[index].x, resolutions[index].y);
        }

        private void SetResolution(int width, int height)
        {
            // Update the resolution in the SettingsManager
            SettingsManager.Instance.ResolutionSetting.Set(new Vector2Int(width, height));
            Bambu.Log($"Video resolution set to: {width} x {height}", Debugging.ELogCategory.UI);
        }

        public override void UpdateSettingOption()
        {
            currentIndex = Closestindex(SettingsManager.Instance.ResolutionSetting.Value);

            base.UpdateSettingOption();
        }

        private int Closestindex(Vector2Int value)
        {
            // Find the closest resolution
            int closestIndex = 0;
            int closestDistance = int.MaxValue;

            for (int i = 0; i < resolutions.Count; i++)
            {
                int distance = (resolutions[i].x - value.x) * (resolutions[i].x - value.x) +
                               (resolutions[i].y - value.y) * (resolutions[i].y - value.y);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }
    }
}
