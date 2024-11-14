using BambuFramework.SceneManagement;
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

        protected void Start()
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
        }

        private void Setting()
        {

        }

        private void MainMenu()
        {
            SceneManager.Instance.LoadMainMenu();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Show();
            }
        }
    }
}
