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

        #region Unity Functions
        protected void Start()
        {
            spawnedTiles = new();
            MasterEventBus.GetMasterEventBus.StartGameWithConfiguration += StartGameWithConfiguration;
        }

        private void StartGameWithConfiguration(GameConfiguration configuration, int selectedDifficulty)
        {
            for (int i = 0; i < spawnedTiles.Count; i++)
            {
                Destroy(spawnedTiles[i]);
            }
            spawnedTiles.Clear();

            int newCount;
            switch (selectedDifficulty)
            {
                case 0:
                    newCount = configuration.EasyTileCount;
                    break;
                case 1:
                    newCount = configuration.MediumTileCount;
                    break;
                case 2:
                    newCount = configuration.HardTileCount;
                    break;
                default:
                    newCount = configuration.EasyTileCount;
                    break;
            }
            
            List<GridTile> tempHolder = new();
            
            for (int i = 0; i < newCount; i++)
            {
                tempHolder.Add(Instantiate(gridPrefab, gridBoard));
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
