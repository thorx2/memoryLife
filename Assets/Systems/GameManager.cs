using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemDub
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameConfiguration gameConfiguration;

        private GridTile lastSelectedTile;

        private GameRoundData _gameRoundData;

        public GameRoundData GetGameRoundData
        {
            get => _gameRoundData;
        }

        protected void Awake()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
            MasterEventBus.GetMasterEventBus.OnTileSelected += OnTileSelected;
        }

        private void OnGameStateChanged(EGameState state)
        {
            switch (state)
            {
                case EGameState.EInGame:
                    _gameRoundData = new();
                    _gameRoundData.Score = 0;
                    MasterEventBus.GetMasterEventBus.StartGameWithConfiguration?.Invoke(gameConfiguration, SaveManager.GetInstance.GetInGameState());
                    break;
            }
        }

        private void OnTileSelected(GridTile selectedTile)
        {
            if (lastSelectedTile != null)
            {
                var isSuccess = lastSelectedTile.TileMatches(selectedTile);

                if (isSuccess)
                {
                    _gameRoundData.Score += gameConfiguration.ScorePerMatch;
                }
                StartCoroutine(DelayedTileHide(selectedTile, isSuccess));
            }
            else
            {
                lastSelectedTile = selectedTile;
            }
        }

        private IEnumerator DelayedTileHide(GridTile brokenPartner, bool isConsume)
        {
            yield return new WaitForSeconds(0.75f);
            if (isConsume)
            {
                MasterEventBus.GetMasterEventBus.OnPlayerActionDone?.Invoke(true, _gameRoundData.Score);
                lastSelectedTile.TileConsumed();
                brokenPartner.TileConsumed();
            }
            else
            {
                lastSelectedTile.HideTile();
                brokenPartner.HideTile();
            }
            lastSelectedTile = null;
        }

        protected void OnDestroy()
        {
            SaveManager.GetInstance.SaveBoardState(JsonUtility.ToJson(_gameRoundData));
        }
    }
}
