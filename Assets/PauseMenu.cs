using BambuFramework.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace BambuFramework
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button mainMenuButton;

        [SerializeField] private GameObject canvas;

        private void Start()
        {
            canvas.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            resumeButton.onClick.AddListener(Resume);
            settingButton.onClick.AddListener(Setting);
            mainMenuButton.onClick.AddListener(MainMenu);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(Resume);
            settingButton.onClick.RemoveListener(Setting);
            mainMenuButton.onClick.RemoveListener(MainMenu);
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Show();
            }
        }

        public void Show()
        {
            canvas.gameObject.SetActive(true);
        }

        public void Hide()
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
