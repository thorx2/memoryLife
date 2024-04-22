using System;
using System.Collections;
using System.Collections.Generic;
using MemDub;
using UnityEngine;

public class MasterCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] inGameUiElements;

    [SerializeField]
    private GameObject[] inMenuUiElements;
    [SerializeField]
    private GameObject[] gameOverUIElements;
    protected void Awake()
    {
        MasterEventBus.GetMasterEventBus.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(EGameState state)
    {
        foreach (var item in inGameUiElements)
        {
            item.SetActive(false);
        }
        foreach (var item in inMenuUiElements)
        {
            item.SetActive(false);
        }
        foreach (var item in gameOverUIElements)
        {
            item.SetActive(false);
        }
        switch (state)
        {
            case EGameState.EInGame:
                foreach (var item in inGameUiElements)
                {
                    item.SetActive(true);
                }
                break;
            case EGameState.EInMenu:
                foreach (var item in inMenuUiElements)
                {
                    item.SetActive(true);
                }
                break;
            case EGameState.EGameOver:
                {
                    foreach (var item in gameOverUIElements)
                    {
                        item.SetActive(true);
                    }
                }
                break;
        }
    }
}
