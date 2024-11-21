using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class SettingsMenu : MenuScreen
    {
        [InlineEditor]
        [SerializeField] private SettingsContainer settingsContainer;
        protected override Button firstButton => null;

        private TabView tabView;
        private Button btnBack;
        private int tabCount;

        private InputSystem_Actions inputActions;

        private List<Focusable> focussables = new List<Focusable>();

        private int currentSettingIndex;

        private void Awake()
        {
            tabView = Root.Q<TabView>("TabView"); // Make sure your TabView has the "TabView" name or update this accordingly

            if (tabView == null)
            {
                Bambu.Log("TabView not found in UI Document!");
                return;
            }

            btnBack = Root.Q<Button>("btnBack");
            btnBack.clicked += Back;

            // Instantiate settings options
            PopulateSettingsOptions();

            tabCount = tabView.childCount;

            // Initialize the first tab to be selected
            tabView.selectedTabIndex = 0;
            SetFocusOnTab(tabView.selectedTabIndex);
        }

        private void PopulateSettingsOptions()
        {
            if (settingsContainer == null || settingsContainer.settingOptions == null)
            {
                Bambu.Log("SettingsContainer or settingOptions is not assigned!");
                return;
            }

            focussables.Clear();

            foreach (SettingsTab tab in settingsContainer.Tabs)
            {
                var tabInstance = tabView.Q<VisualElement>($"tab{tab.TabName}");

                foreach (SettingOption settingOption in tab.SettingOptions)
                {
                    var settingElement = settingOption.SpawnUI(out List<Focusable> fs);

                    focussables.AddRange(fs);

                    tabInstance.Add(settingElement);
                }
            }
        }

        public override void Show(Player player = null, bool sortingOrder = true)
        {
            base.Show(player, sortingOrder);

            inputActions = player.InputActions;

            inputActions.UI.NextTab.performed += ctx => NavigateTabs(1);
            inputActions.UI.PreviousTab.performed += ctx => NavigateTabs(-1);
            inputActions.UI.Exit.performed += ctx => Back();
        }

        public override void Hide(bool sortingOrder = true)
        {
            base.Hide(sortingOrder);

            if (inputActions != null)
            {
                inputActions.UI.NextTab.performed -= ctx => NavigateTabs(1);
                inputActions.UI.PreviousTab.performed -= ctx => NavigateTabs(-1);
                inputActions.UI.Exit.performed -= ctx => Back();
            }
        }

        private void SetFocusOnSetting(int index)
        {
            if (focussables == null || focussables.Count == 0) return;

            // Remove focus from the previous setting
            if (focussables.Count > index)
            {
                var previousSetting = focussables[currentSettingIndex];
                previousSetting?.Blur();
            }

            // Apply focus to the new setting
            var selectedSetting = focussables[index];

            Bambu.Log(selectedSetting.ToString());
            selectedSetting?.Focus();
        }

        private void Back()
        {
            uiManager.ReturnToPrevious();
        }

        protected override void UpdateMenu()
        {

        }

        private void NavigateTabs(int direction)
        {
            if (tabView == null) return;
            // Update selected index and wrap around if necessary
            tabView.selectedTabIndex = (tabView.selectedTabIndex + direction + tabCount) % tabCount;
            SetFocusOnTab(tabView.selectedTabIndex);
        }

        private void SetFocusOnTab(int index)
        {
            if (tabView == null) return;
            // Highlight the currently selected tab header
            var selectedTab = tabView.Children().ElementAt(index) as VisualElement;
            selectedTab?.Focus();
        }
    }
}
