using System;
using log4net.Util;
using UnityEngine;
using UnityEngine.UI;

namespace MemDub
{
    public class MainMenuController : MonoBehaviour
    {
        //Necessary evil, I really do not want to do this but for the time being
        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private Button continueButton;

        protected void Awake()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
        }

        protected void Start()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInMenu);
        }

        protected void OnEnable()
        {
            continueButton.interactable = SaveManager.GetInstance.GetInGameState();
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
            SaveManager.GetInstance.UpdateInGameState(false);
            MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInGame);
        }

        public void ContinueGame()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInGame);
        }
    }
}