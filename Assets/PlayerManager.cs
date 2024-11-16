using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Player hostPlayer;

        private List<Player> playerList = new List<Player>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            AddPlayerInternal(hostPlayer);
        }

        public void AddPlayer()
        {
            Player playerInstance = Instantiate(playerPrefab, transform);
            AddPlayerInternal(playerInstance);
        }

        private void AddPlayerInternal(Player playerInstance)
        {
            playerList.Add(playerInstance);
        }

        public void RemovePlayer(int index)
        {
            if (index == 0) return;
        }
    }
}
