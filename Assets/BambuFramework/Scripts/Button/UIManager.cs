using BambuFramework.SceneManagement;
using UnityEngine;

namespace BambuFramework.UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private InputHints inputHints;

        protected override void Awake()
        {
            base.Awake();
            pauseMenu.Init(this);
            settingsMenu.Init(this);
            inputHints.Init(this);

            HidePause();
            HideSettings();
            HideInputHints();
        }

        public void ShowMainMenu()
        {
            SceneManager.Instance.LoadMainMenu();
        }

        public void ShowPause()
        {
            pauseMenu.Show();
        }

        public void ShowSettings()
        {
            settingsMenu.Show();
            ShowInputHints(1);
        }

        public void ShowInputHints(int index = 0)
        {
            inputHints.Show(index);
        }

        public void HidePause()
        {
            pauseMenu.Hide();
        }

        public void HideSettings()
        {
            settingsMenu.Hide();
            ShowInputHints(0);
        }

        public void HideInputHints()
        {
            inputHints.Hide();
        }
    }
}
