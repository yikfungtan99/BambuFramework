using UnityEngine;

namespace BambuFramework
{
    public class MainMenu : MonoBehaviour
    {
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
