using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private InputHints inputHints;

        private MenuScreen previousScreen;
        private MenuScreen activeScreen;
        public MenuScreen ActiveScreen { get => activeScreen; }

        private Dictionary<MenuScreen, int> menuScreenInputHintKVP;

        public event Action OnSelect;
        public event Action OnSubmit;
        public event Action OnCancel;

        protected override void Awake()
        {
            base.Awake();

            menuScreenInputHintKVP = new Dictionary<MenuScreen, int>()
            {
                { mainMenu, 0 },
                { pauseMenu, 0 },
                { settingsMenu, 1 },
            };

            mainMenu.Init(this);
            pauseMenu.Init(this);
            settingsMenu.Init(this);
            inputHints.Init(this);

            ShowMainMenu(PlayerManager.Instance.HostPlayer);
            HidePause();
            HideSettings();
            HideInputHints();
        }

        public void ShowMainMenu(Player player)
        {
            ActivateScreen(mainMenu, player);
        }

        public void ShowPause(Player player)
        {
            ActivateScreen(pauseMenu, player);
        }

        public void ShowSettings(Player player)
        {
            ActivateScreen(settingsMenu, player);
        }

        private void ActivateScreen(MenuScreen menuScreen, Player player)
        {
            if (menuScreen == null) return;

            if (activeScreen != null)
            {
                if (activeScreen == menuScreen)
                {
                    return;
                }

                previousScreen = activeScreen;
                activeScreen.Hide();
            }

            activeScreen = menuScreen;

            activeScreen.Show(player);

            int index = menuScreenInputHintKVP.ContainsKey(activeScreen) ? menuScreenInputHintKVP[activeScreen] : -1;
            ShowInputHints(index);
        }

        public void ShowInputHints(int index = 0)
        {
            Bambu.Log($"Show Input Hints {index}", Debugging.ELogCategory.UI);
            inputHints.Show(index);
        }

        public void HideMainMenu()
        {
            mainMenu.Hide();
        }

        public void HidePause()
        {
            pauseMenu.Hide();
        }

        public void HideSettings()
        {
            settingsMenu.Hide();
        }

        public void HideInputHints()
        {
            inputHints.Hide();
        }

        public void ReturnToPrevious(Player player, bool reset = false)
        {
            ActivateScreen(previousScreen, player);
            if (reset) ResetPrevious();
        }

        internal void ResetActive()
        {
            activeScreen = null;
            previousScreen = null;
        }

        public void ResetPrevious()
        {
            previousScreen = null;
        }

        public void Select()
        {
            OnSelect?.Invoke();
        }

        public void Submit()
        {
            OnSubmit?.Invoke();
        }

        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public TemplateContainer ShowPopupWindow(PopupData popupData)
        {
            TemplateContainer popUpInstance = UITemplatesContainer.Instance.PopupWindow.CloneTree();

            // Set the position to absolute and expand to all anchors
            popUpInstance.style.position = Position.Absolute;
            popUpInstance.style.left = 0;
            popUpInstance.style.top = 0;
            popUpInstance.style.right = 0;
            popUpInstance.style.bottom = 0;

            popUpInstance.Q<Label>("lblTitle").text = popupData.Title;
            popUpInstance.Q<Label>("lblDescription").text = popupData.Description;

            Button firstButton = null;

            for (int i = 0; i < popupData.ButtonsText.Length; i++)
            {
                TemplateContainer buttonOptionInstance = UITemplatesContainer.Instance.PopupOptionButton.CloneTree();
                Button button = buttonOptionInstance.Q<Button>();

                button.text = popupData.ButtonsText[i];

                if (i < popupData.Actions.Length)
                {
                    button.clicked += popupData.Actions[i];
                }

                popUpInstance.Q<VisualElement>("containerOptionButtons").Add(buttonOptionInstance);

                // Store a reference to the first button
                if (i == 0)
                {
                    firstButton = button;
                }
            }

            // Schedule the focus on the first button for the next frame
            if (firstButton != null)
            {
                firstButton.schedule.Execute(() => firstButton.Focus());
            }

            return popUpInstance;
        }
    }

    public struct PopupData
    {
        public string Title;
        public string Description;
        public string[] ButtonsText;
        public Action[] Actions;
    }
}
