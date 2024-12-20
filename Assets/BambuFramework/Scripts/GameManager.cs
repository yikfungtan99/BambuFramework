﻿using BambuFramework.SceneManagement;
using BambuFramework.UI;
using System;
using UnityEngine;

namespace BambuFramework
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private PlayerManager playerManager;

        public event Action OnGameStart;
        public event Action OnGamePaused;
        public event Action OnGameResume;
        public event Action OnReturnToMainMenu;

        private SceneManager sceneManager;

        private void Start()
        {
            sceneManager = SceneManager.Instance;
        }

        public void Play()
        {
            sceneManager.LoadGameScenes();

            OnGameStart?.Invoke();
        }

        public void Pause(Player player)
        {
            UIManager.Instance.ShowPause(player);

            OnGamePaused?.Invoke();
        }

        public void Resume()
        {
            OnGameResume?.Invoke();
        }

        public void MainMenu()
        {
            sceneManager.UnloadGameScenes();
            UIManager.Instance.ShowMainMenu(PlayerManager.Instance.HostPlayer);
            OnReturnToMainMenu?.Invoke();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
