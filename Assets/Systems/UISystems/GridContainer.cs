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

        [SerializeField]
        private Transform horizontalContainer;


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

        public void RecreateBoardData(string dataJson)
        {

        }

        private void StartGameWithConfiguration(GameConfiguration configuration, int selectedDifficulty)
        {
            for (int i = gridBoard.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(gridBoard.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < spawnedTiles.Count; i++)
            {
                Destroy(spawnedTiles[i]);
            }
            spawnedTiles.Clear();

            currentActiveTiles = configuration.RowCount * configuration.ColCount;
            if (currentActiveTiles % 2 != 0)
            {
                Debug.LogError("Cannot clear board as tiles will not have any pairs!!!");
                MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInMenu);
                return;
            }

            List<GridTile> tempHolder = new();

            for (int i = 0; i < currentActiveTiles; i++)
            {
                tempHolder.Add(Instantiate(gridPrefab));
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
            tempHolder.AddRange(spawnedTiles);
            for (int i = 0; i < configuration.RowCount; i++)
            {
                var tempHor = Instantiate(horizontalContainer, gridBoard);
                for (int j = 0; j < configuration.ColCount; j++)
                {
                    var x = tempHolder[listRnd.Next(tempHolder.Count)];
                    x.transform.SetParent(tempHor.transform);
                    x.SetIndexData(i,j);
                    tempHolder.Remove(x);
                }
            }

            StartCoroutine(HideTilesPostShowForGame());
        }
        #endregion

        #region Functionality

        private IEnumerator HideTilesPostShowForGame()
        {
            yield return new WaitForSeconds(1f);
            foreach (var item in spawnedTiles)
            {
                item.HideTile();
                item.TileTouchable(true);
            }
        }

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
