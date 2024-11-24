using BambuFramework.Audio;
using UnityEngine;

namespace BambuFramework
{
    public class MusicManager : SingletonBehaviour<MusicManager>
    {
        [SerializeField] private AudioReference mainMenuMusic;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.Instance;

            gameManager.OnGameStart += PlayGameMusic;

            PlayMainMenuMusic();
        }

        private void PlayMainMenuMusic()
        {
            mainMenuMusic.Play();
        }

        private void PlayGameMusic()
        {

        }
    }
}
