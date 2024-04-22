using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemDub
{
    public class GridContainer : MonoBehaviour
    {
        [SerializeField]
        private Transform gridBoard;

        [SerializeField]
        private GridTile gridPrefab;

        private List<GridTile> spawnedTiles;

        private readonly System.Random listRnd = new();

        private int currentActiveTiles = 0;

        #region Unity Functions
        protected void Awake()
        {
            spawnedTiles = new();
            MasterEventBus.GetMasterEventBus.StartGameWithConfiguration += StartGameWithConfiguration;

            MasterEventBus.GetMasterEventBus.OnPlayerActionDone += OnPlayerActionDone;
        }

        private void OnPlayerActionDone(bool isSuccess, int _)
        {
            if (isSuccess)
            {
                currentActiveTiles -= 2;
                if (currentActiveTiles <= 0)
                {
                    MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EGameOver);
                }
            }
        }

        private void StartGameWithConfiguration(GameConfiguration configuration, int selectedDifficulty)
        {
            for (int i = 0; i < spawnedTiles.Count; i++)
            {
                Destroy(spawnedTiles[i]);
            }
            spawnedTiles.Clear();

            switch (selectedDifficulty)
            {
                case 0:
                    currentActiveTiles = configuration.EasyTileCount;
                    break;
                case 1:
                    currentActiveTiles = configuration.MediumTileCount;
                    break;
                case 2:
                    currentActiveTiles = configuration.HardTileCount;
                    break;
                default:
                    currentActiveTiles = configuration.EasyTileCount;
                    break;
            }


            List<GridTile> tempHolder = new();

            for (int i = 0; i < currentActiveTiles; i++)
            {
                tempHolder.Add(Instantiate(gridPrefab, gridBoard));
            }

            while (tempHolder.Count > 0)
            {
                //Choose a random shape color combination
                var clr = configuration.GameTileColors[listRnd.Next(configuration.GameTileColors.Length)];
                var spr = configuration.Shapes[listRnd.Next(configuration.Shapes.Length)];

                //Choose two random tiles from the list
                int rndIndex = listRnd.Next(tempHolder.Count);
                var rndTile = tempHolder[rndIndex];
                rndTile.SetTileData(spr, clr);
                tempHolder.Remove(rndTile);
                spawnedTiles.Add(rndTile);

                //TODO Make this a function.
                rndIndex = listRnd.Next(tempHolder.Count);
                rndTile = tempHolder[rndIndex];
                rndTile.SetTileData(spr, clr);
                tempHolder.Remove(rndTile);
                spawnedTiles.Add(rndTile);
            }
        }
        #endregion

        #region Functionality

        public void ShowAllTiles()
        {
            foreach (GridTile tile in spawnedTiles)
            {
                tile.ShowTile();
            }
        }

        public void HideAllTiles()
        {
            foreach (GridTile tile in spawnedTiles)
            {
                tile.HideTile();
            }
        }

        #endregion

        #region Testing
        [ContextMenu("Generate test grid")]
        public void TestGridGeneration()
        {

        }
        #endregion
    }
}
