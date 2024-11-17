using UnityEngine;

namespace BambuFramework.UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] private MenuScreen pauseMenu;
        [SerializeField] private MenuScreen settingsMenu;

        public void ShowPause()
        {
            pauseMenu.Show();
        }

        public void ShowSettings()
        {
            settingsMenu.Show();
        }
    }
}
