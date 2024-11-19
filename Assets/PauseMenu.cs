using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class PauseMenu : MenuScreen
    {
        protected override Button firstButton => resumeButton;

        private Button resumeButton;
        private Button settingButton;
        private Button mainMenuButton;

        protected void Awake()
        {
            resumeButton = Root.Q<Button>("btnResume");
            settingButton = Root.Q<Button>("btnSetting");
            mainMenuButton = Root.Q<Button>("btnMainMenu");

            resumeButton.clicked += Resume;
            settingButton.clicked += Setting;
            mainMenuButton.clicked += MainMenu;

            Hide();
        }

        private void Resume()
        {
            Hide();
            uiManager.ResetActive();
        }

        private void Setting()
        {
            uiManager.ShowSettings();
        }

        private void MainMenu()
        {
            uiManager.ShowMainMenu();
        }

        // Update is called once per frame
        protected override void UpdateMenu()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiManager.ShowPause();
            }
        }
    }
}
