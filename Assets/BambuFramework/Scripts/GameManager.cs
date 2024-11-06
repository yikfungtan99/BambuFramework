using BambuFramework.SceneManagement;

namespace BambuFramework
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public void Play()
        {
            SceneManager.Instance.LoadGameScenes();
        }
    }
}
