using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : SingletonBase<LevelManager>
{
    public static UnityAction<int> OnLevelChanged;
    public static UnityAction<float> OnTimerChanged;

    public static UnityAction<int> OnModeAnimalChanged;
    
    public int modeAnimal;
    public int level;
    public float timer;


    private void Start()
    {
        GameManager.OnGameStateChanged += UpdateGameState;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;
    }

    void UpdateGameState(GameState state)
    {
        if (state == GameState.Playing && GameManager.Instance.isWin)
        {
            NextLevel();
            GameManager.Instance.isWin = false;
        }
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        OnLevelChanged?.Invoke(level);
    }

    public void NextLevel()
    {
        if (level < 12)
        {
            level++;
            OnLevelChanged?.Invoke(level);
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.MainMenu);
        }
    }
    public void UpdateTimer()
    {
        if (timer > 0 && GameManager.Instance.gameState == GameState.Playing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                GameManager.Instance.UpdateGameState(GameState.GameEnd, false);
            }

            OnTimerChanged?.Invoke(timer);
        }
    }

    public void SetTimer(float newTimer)
    {
        timer =  newTimer;
        OnTimerChanged?.Invoke(timer);
    }

    public void UpdateGameMode(int newGameMode)
    {
        modeAnimal = newGameMode;
        OnModeAnimalChanged?.Invoke(modeAnimal);
    }
}
