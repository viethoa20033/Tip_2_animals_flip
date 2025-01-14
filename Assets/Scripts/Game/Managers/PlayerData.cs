using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public GameObject lockLevel;
    public Text levelText;

    public Transform[] buttonLevels;

    private void Start()
    {
        LevelManager.OnModeAnimalChanged += UpdateModeAnimal;

        GameManager.OnGameStateChanged += UpdateGameState;
    }

    private void OnDestroy()
    {
        LevelManager.OnModeAnimalChanged -= UpdateModeAnimal;
        
        GameManager.OnGameStateChanged -= UpdateGameState;

    }

    public void LoadData(int mode)
    {
        int levelMax = PlayerPrefs.GetInt("level" + mode, 0);

        foreach (Transform button in buttonLevels)
        {
            button.GetComponent<Button>().interactable = false;
            foreach (Transform child in button)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < buttonLevels.Length; i++)
        {
            if (i <= levelMax)
            {
                buttonLevels[i].GetComponent<Button>().interactable = true;
                Text _leveltext = Instantiate(levelText, buttonLevels[i]);
                _leveltext.text = (i + 1).ToString();
            }
            else
            {
                Instantiate(lockLevel, buttonLevels[i]);
            }
        }
    }

    public void SaveData(int level, int mode)
    {
        int levelMax = PlayerPrefs.GetInt("level" + mode, 0);

        if (level > levelMax)
        {
            PlayerPrefs.SetInt("level" + mode, level);
            PlayerPrefs.Save();
            LoadData(mode);
        }
    }

    void UpdateModeAnimal(int animal)
    {
        LoadData(animal);
    }

    void UpdateGameState(GameState state)
    {
        if (state == GameState.GameEnd)
        {
            if (GameManager.Instance.isWin)
            {
                SaveData(LevelManager.Instance.level, LevelManager.Instance.modeAnimal);
            }
        }
    }
}
