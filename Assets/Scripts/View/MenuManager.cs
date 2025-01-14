using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button[] buttonModeAnimals;


    private void Start()
    {
        for (int i = 0; i < buttonModeAnimals.Length; i++)
        {
            int index = i;
            buttonModeAnimals[i].onClick.AddListener(() => ButtonClick(index));
        }
    }

    public void ButtonChooseLevel()
    {
        GameManager.Instance.UpdateGameState(GameState.ChooseLevel);
    }

    public void ButtonBack()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    public void ButtonPlay()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void ButtonPause()
    {
        GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void TestWin()
    {
        GameManager.Instance.UpdateGameState(GameState.GameEnd, true);
    }

    public void ButtonClick(int index)
    {
        LevelManager.Instance.UpdateGameMode((index + 1) * 8);
    }
}
