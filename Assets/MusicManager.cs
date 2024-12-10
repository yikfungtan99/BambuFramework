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

            PlayMainMenuMusic();
        }

        private void OnDestroy()
        {
            if (gameManager != null) gameManager.OnGameStart -= PlayGameMusic;
        }

        private void PlayMainMenuMusic()
        {
            AudioLibrary.Instance.MainMenuMusic.PlayAsInstance();
        }

        private void PlayGameMusic()
        {
            Bambu.Log("PLAY GAME MUSIC");
            AudioLibrary.Instance.MainMenuMusic.Stop();
        }
    }
}
