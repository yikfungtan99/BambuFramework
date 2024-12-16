using BambuFramework.Audio;

namespace BambuFramework
{
    public class MusicManager : SingletonBehaviour<MusicManager>
    {
        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.Instance;

            gameManager.OnGameStart += PlayGameMusic;
            gameManager.OnReturnToMainMenu += PlayMainMenuMusic;

            PlayMainMenuMusic();
        }

        private void OnDestroy()
        {
            if (gameManager != null) gameManager.OnGameStart -= PlayGameMusic;
            if (gameManager != null) gameManager.OnReturnToMainMenu -= PlayMainMenuMusic;
        }

        private void PlayMainMenuMusic()
        {
            AudioLibrary.Instance.MainMenuMusic.PlayAsInstance();
        }

        private void PlayGameMusic()
        {
            AudioLibrary.Instance.MainMenuMusic.Stop();
        }
    }
}
