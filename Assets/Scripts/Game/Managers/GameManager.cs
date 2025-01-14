using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBase<GameManager>
{
    public static UnityAction<GameState> OnGameStateChanged;
    public GameState gameState;

    public bool isWin;
    public bool isPlaying;

    public float timeReset;

    [Header("Update bg in mode animal")] 
    public SpriteRenderer spriteBg;
    public Sprite[] bgSprites;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        LevelManager.OnModeAnimalChanged += UpdateModeGame;
    }

    private void OnDestroy()
    {
        LevelManager.OnModeAnimalChanged -= UpdateModeGame;

    }

    public void UpdateGameState(GameState newState, bool? _isWin = null)
    {
        gameState = newState;

        if (_isWin.HasValue)
        {
            isWin = _isWin.Value;
        }
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.ChooseLevel:
                isPlaying = false;
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                break;
            case GameState.GameEnd:
                isPlaying = false;
                break;
        }
        OnGameStateChanged?.Invoke(gameState);
    }

    void HandlePlaying()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            LevelManager.Instance.SetTimer(timeReset);
        }
    }

    void UpdateModeGame(int mode)
    {
        switch (mode)
        {
            case 8:
                timeReset =  120 + 5;
                spriteBg.sprite = bgSprites[0];
                break;
            case 16:
                timeReset = 120 * 2 + 5;
                spriteBg.sprite = bgSprites[1];
                break;
            case 24:
                timeReset = 120 * 4 + 5;
                spriteBg.sprite = bgSprites[2];
                break;
        }
    }
    
}
public enum GameState
{
    MainMenu,
    ChooseLevel,
    Playing,
    Paused,
    GameEnd
}
