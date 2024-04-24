using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //Yet another evil reference
        [SerializeField]
        private GameManager gameManager;


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
                //Susceptible to bugs and count issues.
                //TODO need to change to something more solid.
                currentActiveTiles -= 2;
                if (currentActiveTiles <= 0)
                {
                    SaveManager.GetInstance.UpdateInGameState(false);
                    MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EGameOver);
                }
                //Save board state again here
                var x = gameManager.GetGameRoundData;
            }
        }

        private void StartGameWithConfiguration(GameConfiguration configuration, bool isReplay)
        {
            List<TileData> data = null;
            if (isReplay)
            {
                if (SaveManager.GetInstance.GetBoardState().Length > 0)
                {
                    var d = JsonUtility.FromJson<GameRoundData>(SaveManager.GetInstance.GetBoardState());
                    configuration.RowCount = d.BoardSizeX;
                    configuration.ColCount = d.BoardSizeY;
                    data = d.TileInformation;
                }
                else
                {

                }
            }
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
            List<Transform> rowContainer = new();
            for (int i = 0; i < configuration.RowCount; i++)
            {
                rowContainer.Add(Instantiate(horizontalContainer, gridBoard));
                for (int j = 0; j < configuration.ColCount; j++)
                {
                    var x = tempHolder[listRnd.Next(tempHolder.Count)];
                    x.transform.SetParent(rowContainer.Last().transform);
                    x.SetIndexData(i, j);
                    tempHolder.Remove(x);
                }
            }

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (rowContainer[item.X].GetChild(item.Y).TryGetComponent<GridTile>(out var tile))
                    {
                        tile.SetTileData(item.X, item.Y, item.Color, item.Type, item.IsConsumed);
                    }
                }
            }
            StartCoroutine(HideTilesPostShowForGame());
            SaveManager.GetInstance.UpdateInGameState(true);
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

        #endregion
    }
}
