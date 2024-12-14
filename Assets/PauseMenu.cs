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
            GameManager.Instance.Resume();
            Hide();
            UIManager.Instance.HideInputHints();
            uiManager.ResetActive();
        }

        private void Setting()
        {
            uiManager.ShowSettings(initiatedPlayer);
        }

        private void MainMenu()
        {
            GameManager.Instance.Quit();
        }

        // Update is called once per frame
        protected override void UpdateMenu()
        {

        }
    }
}
