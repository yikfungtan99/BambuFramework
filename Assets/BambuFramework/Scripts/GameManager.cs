using BambuFramework.SceneManagement;
using Eflatun.SceneReference;
using UnityEngine;

namespace BambuFramework
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private SceneReference[] gameScenes;

        public void Play()
        {
            SceneManager.Instance.SwapScenes(gameScenes);
        }
    }
}
