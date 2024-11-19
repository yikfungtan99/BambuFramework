using BambuFramework.SceneManagement;
using System;
using UnityEngine;

namespace BambuFramework
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private PlayerManager playerManager;

        public event Action OnGameStart;

        public void Play()
        {
            SceneManager.Instance.LoadGameScenes();
            OnGameStart?.Invoke();
        }
    }
}
