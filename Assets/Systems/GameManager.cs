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

        private int _currentScore;

        private int _difficultyValue = 0;

        protected void Awake()
        {
            MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
            MasterEventBus.GetMasterEventBus.OnTileSelected += OnTileSelected;
            _currentScore = 0;
        }

        private void OnGameStateChanged(EGameState state)
        {
            switch (state)
            {
                case EGameState.EInGame:
                    //TODO Difficulty menu?
                    MasterEventBus.GetMasterEventBus.StartGameWithConfiguration?.Invoke(gameConfiguration, _difficultyValue);
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
                    _currentScore += gameConfiguration.ScorePerMatch;
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
                MasterEventBus.GetMasterEventBus.OnPlayerActionDone?.Invoke(true, _currentScore);
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

        internal void SetDifficulty(int val)
        {
            _difficultyValue = val;
        }
    }
}
