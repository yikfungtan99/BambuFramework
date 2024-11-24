using BambuFramework.Debugging;
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

        protected void Awake()
        {
            playButton = Root.Q<Button>("btnPlay");
            settingButton = Root.Q<Button>("btnSetting");
            exitButton = Root.Q<Button>("btnExit");

            playButton.clicked -= Play;
            playButton.clicked += Play;

            settingButton.clicked -= Setting;
            settingButton.clicked += Setting;

            exitButton.clicked -= Exit;
            exitButton.clicked += Exit;

            Show();
        }

        public override void Show(Player player = null, bool sortingOrder = true)
        {
            BambuLogger.Log($"Showing: {gameObject.name}", ELogCategory.UI);
            Root.visible = true;
            Root.schedule.Execute(() => firstButton?.Focus()).StartingIn(1);
        }

        public override void Hide(bool sortingOrder = false)
        {
            base.Hide(sortingOrder);
        }

        private void Play()
        {
            GameManager.Instance.Play();
            UIManager.Instance.HideInputHints();
            Hide();
            uiManager.ResetActive();
        }

        public void Setting()
        {
            UIManager.Instance.ShowSettings(PlayerManager.Instance.HostPlayer);
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
