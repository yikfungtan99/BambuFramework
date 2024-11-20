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

        private List<Focusable> focussable;

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

            // Find the tab container named "tabGameplay"
            var gameplayTab = tabView.Q<VisualElement>("tabGameplay");

            if (gameplayTab == null)
            {
                Bambu.Log("tabGameplay not found in TabView!");
                return;
            }

            foreach (var settingOption in settingsContainer.settingOptions)
            {
                var settingElement = settingOption.SpawnUI();

                // Add the settingElement to the gameplayTab
                gameplayTab.Add(settingElement);
            }
        }

        public override void Show(Player player = null, bool sortingOrder = true)
        {
            base.Show(player, sortingOrder);

            inputActions = player.InputActions;

            inputActions.UI.NextTab.performed += ctx => NavigateTabs(1);
            inputActions.UI.PreviousTab.performed += ctx => NavigateTabs(-1);
        }

        public override void Hide(bool sortingOrder = true)
        {
            base.Hide(sortingOrder);

            if (inputActions != null) inputActions.UI.NextTab.performed -= ctx => NavigateTabs(1);
            if (inputActions != null) inputActions.UI.PreviousTab.performed -= ctx => NavigateTabs(-1);
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
