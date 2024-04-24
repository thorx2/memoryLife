using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Differences;
using UnityEngine;
using UnityEngine.UI;

namespace MemDub
{
    public class GridTile : MonoBehaviour
    {
        [SerializeField]
        private Image displayImage;

        [SerializeField]
        private CanvasGroup containerGroup;

        private bool consumed;

        private (int, int) TileIndexPos;

        private bool _animationRunning = false;
        private Button _parentButton;

        public (Sprite, Color) GetTileMetadata
        {
            get => (displayImage.sprite, displayImage.color);
        }

        public void TileConsumed()
        {
            consumed = true;
            containerGroup.alpha = 0f;
        }

        public void SetTileData(int x, int y, EShapeColor clr, EShapeType type, bool hasBeenConsumed)
        {
            SetIndexData(x, y);
            if (hasBeenConsumed)
            {
                TileConsumed();
            }
        }

        public void SetTileData(Sprite visual, Color color)
        {
            containerGroup.alpha = 1f;
            displayImage.sprite = visual;
            displayImage.color = color;
            displayImage.gameObject.SetActive(true);
            displayImage.preserveAspect = true;
            consumed = false;
            StartCoroutine(DelayedScaleSet());
            if (TryGetComponent(out _parentButton))
            {
                _parentButton.interactable = false;
            }
        }

        private IEnumerator DelayedScaleSet()
        {
            yield return new WaitForEndOfFrame();
            transform.localScale = Vector3.one;
        }

        public void TileTouchable(bool toggle)
        {
            if (_parentButton)
            {
                _parentButton.interactable = toggle;
            }
        }

        public void HideTile()
        {
            _animationRunning = true;
            StartCoroutine(FlipCard(false));
        }

        private IEnumerator FlipCard(bool isShow)
        {
            while (transform.localScale.x > 0f)
            {
                yield return new WaitForEndOfFrame();
                transform.localScale = new(transform.localScale.x - 0.1f, transform.localScale.y, transform.localScale.z);
            }
            displayImage.gameObject.SetActive(isShow);
            if (isShow)
            {
                MasterEventBus.GetMasterEventBus.OnTileSelected(this);
            }
            while (transform.localScale.x < 1f)
            {
                yield return new WaitForEndOfFrame();
                transform.localScale = new(transform.localScale.x + 0.1f, transform.localScale.y, transform.localScale.z);
            }
            transform.localScale = Vector3.one;
            _animationRunning = false;
        }

        public void ShowTile()
        {
            if (!consumed && !_animationRunning)
            {
                _animationRunning = true;
                StartCoroutine(FlipCard(true));
            }
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

        internal void SetIndexData(int i, int j)
        {
            TileIndexPos.Item1 = i;
            TileIndexPos.Item2 = j;
        }
    }
}
