using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemDub
{
    public class GridTile : MonoBehaviour
    {
        [SerializeField]
        private Image displayImage;

        public (Sprite, Color) GetTileMetadata
        {
            get => (displayImage.sprite, displayImage.color);
        }

        public void TileConsumed()
        {
            transform.localScale = Vector3.zero;
        }

        public void SetTileData(Sprite visual, Color color)
        {
            transform.localScale = Vector3.one;
            displayImage.sprite = visual;
            displayImage.color = color;
        }

        public void HideTile()
        {
            
        }

        public void ShowTile()
        {
            MasterEventBus.GetMasterEventBus.OnTileSelected(this);
        }

        public bool TileMatches(GridTile selectedTile)
        {
            if (GetTileMetadata.Item1 == selectedTile.GetTileMetadata.Item1 &&
                GetTileMetadata.Item2 == selectedTile.GetTileMetadata.Item2)
            {
                return true;
            }
            return false;
        }
    }
}
