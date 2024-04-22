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

        protected void Start()
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
                    MasterEventBus.GetMasterEventBus.StartGameWithConfiguration?.Invoke(gameConfiguration, 0);
                    break;
            }
        }

        private void OnTileSelected(GridTile selectedTile)
        {
            if (lastSelectedTile != null)
            {
                if (lastSelectedTile.TileMatches(selectedTile))
                {
                    _currentScore += gameConfiguration.ScorePerMatch;
                    MasterEventBus.GetMasterEventBus.OnPlayerActionDone?.Invoke(true, _currentScore);
                    lastSelectedTile.TileConsumed();
                    selectedTile.TileConsumed();
                }
                else
                {
                    lastSelectedTile = null;
                }
            }
            else
            {
                lastSelectedTile = selectedTile;
            }
        }
    }
}
