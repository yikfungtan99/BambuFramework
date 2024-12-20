using BambuFramework.Settings;
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
        protected override Button firstButton => btnBack;

        private TabView tabView;
        private Button btnBack;
        private Button btnDefault;
        private Button btnApply;

        private int tabCount;

        private List<Focusable> focussables = new List<Focusable>();

        private int currentSettingIndex;

        private List<SettingOption> currentSettingOptions = new List<SettingOption>();

        private Player player;
        public Player Player
        {
            get
            {
                return player != null ? player : PlayerManager.Instance.HostPlayer;
            }
        }

        private TemplateContainer popupInstance;

        private void Awake()
        {
            tabView = Root.Q<TabView>("TabView"); // Make sure your TabView has the "TabView" name or update this accordingly

            if (tabView == null)
            {
                Bambu.Log("TabView not found in UI Document!");
                return;
            }

            // Instantiate settings options
            PopulateSettingsOptions();

            tabCount = tabView.childCount;

            // Initialize the first tab to be selected
            tabView.selectedTabIndex = 0;
        }

        private void PopulateSettingsOptions()
        {
            if (settingsContainer == null)
            {
                Bambu.Log("SettingsContainer or settingOptions is not assigned!");
                return;
            }

            focussables.Clear();

            foreach (SettingsTab tab in settingsContainer.Tabs)
            {
                Tab tabInstance = tabView.Q<Tab>($"tab{tab.TabName}");

                btnBack = tabInstance.Q<Button>("btnBack");
                btnBack.clicked += Back;

                btnDefault = tabInstance.Q<Button>("btnDefault");
                btnDefault.clicked += Default;

                btnApply = tabInstance.Q<Button>("btnApply");
                if (btnApply != null) btnApply.clicked += Apply;

                tabView.Add(tabInstance);

                foreach (SettingOption settingOption in tab.SettingOptions)
                {
                    settingOption.SpawnUI(this, out List<TemplateContainer> uiInstances, out List<Focusable> fs);

                    for (int i = 0; i < uiInstances.Count; i++)
                    {
                        if (uiInstances[i] == null)
                        {
                            Bambu.Log($"{settingOption} NOT FOUND!");
                        }

                        tabInstance.Add(uiInstances[i]);
                    }

                    focussables.AddRange(fs);

                    currentSettingOptions.Add(settingOption);
                }

                if (btnBack != null) focussables.Add(btnBack);
                if (btnDefault != null) focussables.Add(btnDefault);
                if (btnApply != null) focussables.Add(btnApply);

                UpdateAllSettingOptions();
            }
        }

        private void UpdateAllSettingOptions()
        {
            foreach (var settingOptionInstance in currentSettingOptions)
            {
                settingOptionInstance.UpdateSettingOption();
            }
        }

        public override void Show(Player player = null, bool sortingOrder = true)
        {
            base.Show(player, sortingOrder);

            this.player = player;

            player.PlayerInput.actions["NextTab"].performed += ctx => NavigateTabs(1);
            player.PlayerInput.actions["PreviousTab"].performed += ctx => NavigateTabs(-1);
            player.PlayerInput.actions["Exit"].performed += ctx => Back();

            if (player != null) player.OnInputDeviceChanged += UpdateAllSettingOptions;

            UpdateAllSettingOptions();
        }

        public override void Hide(bool sortingOrder = true)
        {
            base.Hide(sortingOrder);

            if (SettingsManager.Instance.IsBusy) return;

            if (player != null)
            {
                player.PlayerInput.actions["NextTab"].performed -= ctx => NavigateTabs(1);
                player.PlayerInput.actions["PreviousTab"].performed -= ctx => NavigateTabs(-1);
                player.PlayerInput.actions["Exit"].performed -= ctx => Back();
            }

            if (player != null) player.OnInputDeviceChanged -= UpdateAllSettingOptions;
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
            if (SettingsManager.Instance.HaveChanges())
            {
                ShowApplySettingsPopup();
                return;
            }

            uiManager.ReturnToPrevious(initiatedPlayer, true);
        }

        private void Default()
        {
            ShowDefaultSettingsPopUp();
        }

        private void RevertToPrevious()
        {
            SettingsManager.Instance.LoadSettings();
            UpdateAllSettingOptions();
        }

        private void Apply()
        {
            SettingsManager.Instance.ApplySetting(tabView.selectedTabIndex);
            UpdateAllSettingOptions();
        }

        protected override void UpdateMenu()
        {

        }

        private void NavigateTabs(int direction)
        {
            if (tabView == null) return;
            if (SettingsManager.Instance.IsBusy) return;

            // Update selected index and wrap around if necessary
            tabView.selectedTabIndex = (tabView.selectedTabIndex + direction + tabCount) % tabCount;
            SetFocusOnTab(tabView.selectedTabIndex);
        }

        private void ShowApplySettingsPopup()
        {
            PopupData applySettingsPopup = new PopupData();
            applySettingsPopup.Title = "UNSAVED CHANGES";
            applySettingsPopup.Description = "Do you wish to apply unchanged settings?";
            applySettingsPopup.ButtonsText = new string[]
            {
                    "YES",
                    "NO",
                    "CANCEL"
            };
            applySettingsPopup.Actions = new System.Action[]
            {
                ApplyThenBack,
                BackFromPopup,
                ClosePopup
            };

            ShowPopup(applySettingsPopup);
        }

        private void ShowDefaultSettingsPopUp()
        {
            PopupData revertSettingsPopup = new PopupData();
            revertSettingsPopup.Title = "REVERT TO DEFAULT";
            revertSettingsPopup.Description = $"Do you wish to revert <color=yellow>{settingsContainer.Tabs[tabView.selectedTabIndex].TabName}</color> settings to default?";
            revertSettingsPopup.ButtonsText = new string[]
            {
                    "YES",
                    "NO",
            };
            revertSettingsPopup.Actions = new System.Action[]
            {
                RevertToDefault,
                ClosePopup
            };

            ShowPopup(revertSettingsPopup);
        }

        private void ShowPopup(PopupData popupData)
        {
            popupInstance = UIManager.Instance.ShowPopupWindow(popupData);
            Root.Add(popupInstance);
            SettingsManager.Instance.IsBusy = true;

            SetAllFocusables(false);
        }

        private void ClosePopup()
        {
            if (popupInstance == null) return;
            Root.Remove(popupInstance);
            SettingsManager.Instance.IsBusy = false;

            SetAllFocusables(true);
        }

        private void SetAllFocusables(bool foc)
        {
            foreach (var item in focussables)
            {
                item.focusable = foc;
            }
        }

        private void ApplyThenBack()
        {
            ClosePopup();
            Apply();
            UpdateAllSettingOptions();
            Back();
        }

        private void BackFromPopup()
        {
            ClosePopup();
            SettingsManager.Instance.LoadSettings();
            UpdateAllSettingOptions();
            Back();
        }

        private void RevertToDefault()
        {
            ClosePopup();
            SettingsManager.Instance.RevertDefaultSettings(tabView.selectedTabIndex);
            UpdateAllSettingOptions();
        }

        private void SetFocusOnTab(int index)
        {
            if (tabView == null) return;
            // Highlight the currently selected tab header
            var selectedTab = tabView.Children().ElementAt(index) as VisualElement;
            selectedTab?.Focus();
            uiManager.Select();
        }
    }
}
