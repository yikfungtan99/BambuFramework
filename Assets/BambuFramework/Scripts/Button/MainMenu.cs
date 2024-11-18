using BambuFramework.Debug;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class MainMenu : MenuScreen
    {
        private Button playButton;
        private Button settingButton;
        private Button exitButton;

        protected override Button firstButton => playButton;

        protected void Start()
        {
            playButton = Root.Q<Button>("btnPlay");
            settingButton = Root.Q<Button>("btnSetting");
            exitButton = Root.Q<Button>("btnExit");

            playButton.clicked += Play;
            settingButton.clicked += Setting;
            exitButton.clicked += Exit;

            Show();
        }

        public override void Show(bool sortingOrder = true)
        {
            BambuLogger.Log($"Showing: {gameObject.name}", ELogCategory.UI);
            Root.visible = true;
            Root.schedule.Execute(() => firstButton?.Focus()).StartingIn(1);
        }

        private void Play()
        {
            GameManager.Instance.Play();
        }

        public void Setting()
        {
            UIManager.Instance.ShowSettings();
        }

        public void Exit()
        {
            Application.Quit();
        }

        protected override void UpdateMenu()
        {

        }
    }
}
