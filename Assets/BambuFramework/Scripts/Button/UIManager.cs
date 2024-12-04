﻿using System.Collections.Generic;
using UnityEngine;

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
    }
}
