using UnityEngine;
using UnityEngine.UI;

namespace BambuFramework
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(Play);
            settingButton.onClick.AddListener(Setting);
            quitButton.onClick.AddListener(Quit);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(Play);
            settingButton.onClick.RemoveListener(Setting);
            quitButton.onClick.RemoveListener(Quit);
        }

        public void Play()
        {
            GameManager.Instance.Play();
        }

        public void Setting()
        {

        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
