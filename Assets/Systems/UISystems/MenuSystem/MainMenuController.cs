using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemDub
{
    public class MainMenuController : MonoBehaviour
    {
        protected void Start()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(EGameState state)
        {
            switch (state)
            {
                case EGameState.EInGame:
                    gameObject.SetActive(false);
                    break;
                case EGameState.EInMenu:
                    gameObject.SetActive(true);
                    break;
            }
        }

        public void StartGame()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInGame);
        }
    }
}