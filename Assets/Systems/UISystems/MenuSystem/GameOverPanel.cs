using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemDub
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text finalScoreLabel;

        private int _finalDisplayScore;

        protected void Awake()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
            MasterEventBus.GetMasterEventBus.OnPlayerActionDone += OnPlayerActionDone;
        }

        private void OnPlayerActionDone(bool _, int score)
        {
            _finalDisplayScore = score;
            SaveManager.GetInstance.UpdateInGameState(false);
        }

        private void OnGameStateChanged(EGameState state)
        {
            if (state == EGameState.EGameOver)
            {
                gameObject.SetActive(true);
                finalScoreLabel.text = $"You Scored\n{_finalDisplayScore}";
            }
        }

        public void OnContinueButtonHit()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInMenu);
        }
    }
}