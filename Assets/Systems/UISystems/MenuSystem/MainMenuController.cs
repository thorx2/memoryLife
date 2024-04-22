using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemDub
{
    public class MainMenuController : MonoBehaviour
    {
        //Necessary evil, I really do not want to do this but for the time being
        [SerializeField]
        private GameManager gameManager;

        protected void Awake()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
        }

        protected void Start()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInMenu);
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

        public void OnDifficultyChange(int val)
        {
            gameManager.SetDifficulty(val);
        }
    }
}