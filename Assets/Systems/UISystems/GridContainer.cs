using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private VerticalLayoutGroup verticalLayoutGroup;
        private List<GridTile> _spawnedTiles;

        private readonly System.Random _listRnd = new();

        private int _currentActiveTiles = 0;

        #region Unity Functions
        protected void Awake()
        {
            _spawnedTiles = new();
            MasterEventBus.GetMasterEventBus.StartGameWithConfiguration += StartGameWithConfiguration;

            MasterEventBus.GetMasterEventBus.OnPlayerActionDone += OnPlayerActionDone;
        }

        private void OnPlayerActionDone(bool isSuccess, int _)
        {
            if (isSuccess)
            {
                //Susceptible to bugs and count issues.
                //TODO need to change to something more solid.
                _currentActiveTiles -= 2;
                if (_currentActiveTiles <= 0)
                {
                    MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EGameOver);
                }
                //Save board state again here
                //Inefficient for memory, but can be ignore for casual games?
                var x = gameManager.GetGameRoundData;
                var td = new List<TileData>();
                foreach (var item in _spawnedTiles)
                {
                    td.Add(item.CreateTileData());
                }
                x.TileInformation = td;
                gameManager.GetGameRoundData = x;
            }
        }

        private void StartGameWithConfiguration(GameConfiguration configuration, bool isReplay)
        {
            List<TileData> data = null;
            int roundRowCount = configuration.RowCount;
            int roundColCount = configuration.ColCount;
            if (isReplay)
            {
                if (SaveManager.GetInstance.GetBoardState().Length > 0)
                {
                    var d = JsonUtility.FromJson<GameRoundData>(SaveManager.GetInstance.GetBoardState());
                    roundRowCount = d.BoardSizeX;
                    roundColCount = d.BoardSizeY;
                    data = d.TileInformation;
                    if (data == null || data.Count == 0)
                    {
                        roundRowCount = configuration.RowCount;
                        roundColCount = configuration.ColCount;
                        Debug.LogError($"Requested game continue, missing save state, reverting to default game");
                    }
                }
                else
                {
                    Debug.LogError($"Requested game continue, missing save state, reverting to default game");
                }
            }
            for (int i = gridBoard.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(gridBoard.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < _spawnedTiles.Count; i++)
            {
                Destroy(_spawnedTiles[i]);
            }
            _spawnedTiles.Clear();

            _currentActiveTiles = roundRowCount * roundColCount;

            verticalLayoutGroup.childControlHeight = roundRowCount > 2;

            if (_currentActiveTiles % 2 != 0)
            {
                Debug.LogError("Cannot clear board as tiles will not have any pairs!!!");
                MasterEventBus.GetMasterEventBus.OnGameStateChanged?.Invoke(EGameState.EInMenu);
                return;
            }

            List<GridTile> tempHolder = new();

            for (int i = 0; i < _currentActiveTiles; i++)
            {
                tempHolder.Add(Instantiate(gridPrefab));
            }

            while (tempHolder.Count > 0)
            {
                //Choose a random shape color combination
                int colorIndex = _listRnd.Next(configuration.GameTileColors.Length);
                var clr = configuration.GameTileColors[colorIndex];
                int shapeIndex = _listRnd.Next(configuration.Shapes.Length);
                var spr = configuration.Shapes[shapeIndex];

                //Choose two random tiles from the list
                int rndIndex = _listRnd.Next(tempHolder.Count);
                var rndTile = tempHolder[rndIndex];
                rndTile.SetTileData(spr, clr, shapeIndex, colorIndex);
                tempHolder.Remove(rndTile);
                _spawnedTiles.Add(rndTile);

                //TODO Make this a function.
                rndIndex = _listRnd.Next(tempHolder.Count);
                rndTile = tempHolder[rndIndex];
                rndTile.SetTileData(spr, clr, shapeIndex, colorIndex);
                tempHolder.Remove(rndTile);
                _spawnedTiles.Add(rndTile);
            }
            tempHolder.AddRange(_spawnedTiles);
            List<Transform> rowContainer = new();
            for (int i = 0; i < roundRowCount; i++)
            {
                rowContainer.Add(Instantiate(horizontalContainer, gridBoard));
                for (int j = 0; j < roundColCount; j++)
                {
                    var x = tempHolder[_listRnd.Next(tempHolder.Count)];
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
                        tile.SetTileData(tile.GetTileIndexPos.Item1, tile.GetTileIndexPos.Item2,
                                            configuration.GameTileColors[item.Color],
                                            configuration.Shapes[item.Type], item.IsConsumed,
                                            item.Type, item.Color);
                        if (item.IsConsumed)
                        {
                            --_currentActiveTiles;
                        }
                    }
                }
            }
            StartCoroutine(HideTilesPostShowForGame());
            var rd = gameManager.GetGameRoundData;
            rd.BoardSizeX = roundRowCount;
            rd.BoardSizeY = roundColCount;
            var td = new List<TileData>();
            foreach (var item in _spawnedTiles)
            {
                td.Add(item.CreateTileData());
            }
            rd.TileInformation = td;
            gameManager.GetGameRoundData = rd;
            SaveManager.GetInstance.UpdateInGameState(true);
            StartCoroutine(TileSizeSet());
        }
        #endregion

        #region Functionality

        private IEnumerator TileSizeSet()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            foreach (var rndTile in _spawnedTiles)
            {
                rndTile.EnableAspectRatioHelper();
            }
        }

        private IEnumerator HideTilesPostShowForGame()
        {
            yield return new WaitForSeconds(1f);
            foreach (var item in _spawnedTiles)
            {
                item.HideTile();
                item.TileTouchable(true);
            }
        }

        public void ShowAllTiles()
        {
            foreach (GridTile tile in _spawnedTiles)
            {
                tile.ShowTile();
            }
        }

        public void HideAllTiles()
        {
            foreach (GridTile tile in _spawnedTiles)
            {
                tile.HideTile();
            }
        }

        #endregion

        #region Testing

        #endregion
    }
}
