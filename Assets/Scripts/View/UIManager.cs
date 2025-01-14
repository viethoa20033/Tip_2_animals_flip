using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    //Lock click button after click
    public GameObject lockClick;
    
    [Header("Main Menu")]
    public CanvasGroup bg;
    public GameObject mainMenu;

    [Header("Main")] 
    public GameObject main;
    public RectTransform logo;
    public RectTransform buttonAnimals;
    
    [Header("Choose Level")] 
    public RectTransform chooseLevel;
    public Button[] levelButtons;

    [Space] 
    [Header("Game Play")] 
    public RectTransform gamePlay;
    public RectTransform gamePause;
    public RectTransform gameOver;
    public RectTransform gameSuccess;

    [Space] [Header("Text")] 
    public Text[] levelTexts;
    public Text[] textPopUp;
    public Text timeText;

    [Header("Changed Sound")] 
    public Image[] soundImages;
    public Sprite[] soundIcons;
    public bool isMusic;

    [Header("Button Anim")] 
    public Button[] buttonAnimationClicks;
    private void Start()
    {
        GameManager.OnGameStateChanged += UpdateGameState;

        LevelManager.OnLevelChanged += Updatelevel;
        LevelManager.OnTimerChanged += UpdateTimer;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[i].onClick.AddListener(()=> ButtonLevelOnClick(index));
        }

        for (int i = 0; i < buttonAnimationClicks.Length; i++)
        {
            int index = i;
            buttonAnimationClicks[i].onClick.AddListener(()=> ButtonAnimationClick(index));
        }

        isMusic = PlayerPrefs.GetInt("isMusic", 1) == 1;
        if (isMusic)
        {
            AudioListener.volume = 1;
            foreach (var soundImage in soundImages)
            {
                soundImage.sprite = soundIcons[1];
            }
        }
        else
        {
            AudioListener.volume = 0;
            foreach (var soundImage in soundImages)
            {
                soundImage.sprite = soundIcons[0];
            }
        }

    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;

        LevelManager.OnLevelChanged -= Updatelevel;
        LevelManager.OnTimerChanged -= UpdateTimer;
    }

    void ButtonLevelOnClick(int index)
    {
        int level = index + 1;
        LevelManager.Instance.SetLevel(level);
    }
    void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.ChooseLevel:
                HandleChooseLevel();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePause();
                break;
            case GameState.GameEnd:
                HandleGameEnd();
                break;
        }
    }

    void HandleMainMenu()
    {
        lockClick.SetActive(true);
        //Choose level => main menu
        if (chooseLevel.gameObject.activeInHierarchy)
        {
            chooseLevel.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                    {
                        lockClick.SetActive(false);
                        
                        chooseLevel.gameObject.SetActive(false);

                        main.SetActive(true);
                        logo.anchoredPosition = new Vector2(0, Screen.height);
                        logo.DOAnchorPos(new Vector2(0, 500), 1f).SetEase(Ease.OutBounce).SetUpdate(true);

                        buttonAnimals.anchoredPosition = new Vector2(0, -Screen.height);
                        buttonAnimals.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetUpdate(true);
                    });
        }

        //Game play => Menu game
        if (gamePlay.gameObject.activeInHierarchy)
        {
            gamePlay.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(
                () =>
                {
                    lockClick.SetActive(false);
                    
                    gamePlay.gameObject.SetActive(false);

                    bg.alpha = 0;
                    bg.DOFade(1, 1f);

                    mainMenu.SetActive(true);
                    main.SetActive(true);

                    logo.anchoredPosition = new Vector2(0, Screen.height);
                    logo.DOAnchorPos(new Vector2(0, 500), 1f).SetEase(Ease.OutBounce).SetUpdate(true);

                    buttonAnimals.anchoredPosition = new Vector2(0, -Screen.height);
                    buttonAnimals.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetUpdate(true);

                    bg.DOFade(1, 1f);

                });
        }

    }

    void HandleChooseLevel()
    {
        lockClick.SetActive(true);
        
        //Main Menu => choose level
        logo.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true);
        buttonAnimals.DOAnchorPos(new Vector2(0, -Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(() =>
        {
            lockClick.SetActive(false);
            
            main.SetActive(false);
            
            chooseLevel.gameObject.SetActive(true);
            chooseLevel.anchoredPosition = new Vector2(0, Screen.height);
            chooseLevel.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetUpdate(true);
            
            StartCoroutine(ButtonLevelAnimation());
        });
        
        //GamePlay (gamePause, gamelose,gamewin) => choose Level
        if (gamePause.gameObject.activeInHierarchy)
        {
            gamePause.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                {
                    lockClick.SetActive(false);
                    
                    gamePause.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    
                    gamePlay.gameObject.SetActive(false);
                    bg.DOFade(1, 1f);
                    
                    mainMenu.SetActive(true);
                    
                    chooseLevel.gameObject.SetActive(true);
                    chooseLevel.anchoredPosition = new Vector2(0, Screen.height);
                    chooseLevel.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);
                    
                    StartCoroutine(ButtonLevelAnimation());
                });
        }
        
        //game over => choose level
        if (gameOver.gameObject.activeInHierarchy)
        {
            gameOver.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                {
                    lockClick.SetActive(false);
                    
                    gameOver.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    
                    gamePlay.gameObject.SetActive(false);
                    bg.DOFade(1, 1f);
                    
                    mainMenu.SetActive(true);
                    
                    chooseLevel.gameObject.SetActive(true);
                    chooseLevel.anchoredPosition = new Vector2(0, Screen.height);
                    chooseLevel.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);
                    
                    StartCoroutine(ButtonLevelAnimation());
                    
                });
        }
        
        //Game success => choose level
        if(gameSuccess.gameObject.activeInHierarchy)
        {
            gameSuccess.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                {
                    lockClick.SetActive(false);
                    
                    gameSuccess.gameObject.SetActive(false);
                    Time.timeScale = 1;
                    
                    gamePlay.gameObject.SetActive(false);
                    bg.DOFade(1, 1f);
                    
                    mainMenu.SetActive(true);
                    
                    chooseLevel.gameObject.SetActive(true);
                    chooseLevel.anchoredPosition = new Vector2(0, Screen.height);
                    chooseLevel.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);
                    
                    StartCoroutine(ButtonLevelAnimation());
                });
        }
    }

    void HandlePlaying()
    {
        lockClick.SetActive(true);
        //Menu => Playing
        if (chooseLevel.gameObject.activeInHierarchy)
        {
            bg.DOFade(0, 1f);
            chooseLevel.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(
                () =>
                {
                    lockClick.SetActive(false);

                    
                    mainMenu.SetActive(false);
                    chooseLevel.gameObject.SetActive(false);

                    gamePlay.gameObject.SetActive(true);
                    gamePlay.anchoredPosition = new Vector2(0, Screen.height);
                    gamePlay.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);
                });
        }

        //Pause => playing
        if (gamePause.gameObject.activeInHierarchy)
        {
            gamePause.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                {
                    lockClick.SetActive(false);
                    
                    gamePause.gameObject.SetActive(false);
                    Time.timeScale = 1;
                });
        }
        
        //Game Over => playing
        if (gameOver.gameObject.activeInHierarchy)
        {
            gameOver.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                {
                    lockClick.SetActive(false);
                    
                    
                    gameOver.gameObject.SetActive(false);
                    Time.timeScale = 1;
                });
        }
        
        //Game Success => playing
        if (gameSuccess.gameObject.activeInHierarchy)
        {
            gameSuccess.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBounce).SetUpdate(true)
                .OnComplete(() =>
                {
                    lockClick.SetActive(false);

                    
                    gameSuccess.gameObject.SetActive(false);
                    Time.timeScale = 1;
                });
        }
    }

    void HandlePause()
    {
        lockClick.SetActive(true);
        Time.timeScale = 0;
        gamePause.gameObject.SetActive(true);
        gamePause.anchoredPosition = new Vector2(0, Screen.height);
        gamePause.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(() =>
        {
            lockClick.SetActive(false);
        });
    }

    void HandleGameEnd()
    {
        lockClick.SetActive(true);
        if (GameManager.Instance.isWin)
        {
            gameSuccess.gameObject.SetActive(true);
            gameSuccess.anchoredPosition = new Vector2(0, Screen.height);
            gameSuccess.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(() =>
            {
                lockClick.SetActive(false);
            });
        }
        else
        {
            gameOver.gameObject.SetActive(true);
            gameOver.anchoredPosition = new Vector2(0, Screen.height);
            gameOver.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(() =>
            {
                lockClick.SetActive(false);
            });;
        }

        //Update text level
        foreach (var textlevel in textPopUp)
        {
            textlevel.text = "LEVEL " + LevelManager.Instance.level;
        }
        
    }

    void Updatelevel(int level)
    {
        foreach (var levelText in levelTexts)
        {
            levelText.text = "LEVEL " + level;
        }
    }

    void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timeText.text =  string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator ButtonLevelAnimation()
    {
        foreach (var levelButton in levelButtons)
        {
            levelButton.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(.25f);
        }
    }

    public void ChangedSound()
    {
        isMusic = !isMusic;

        if (isMusic)
        {
            AudioListener.volume = 1;
            foreach (var soundImage in soundImages)
            {
                soundImage.sprite = soundIcons[1];
            }
        }
        else
        {
            AudioListener.volume = 0;
            foreach (var soundImage in soundImages)
            {
                soundImage.sprite = soundIcons[0];
            }
        }
        
        PlayerPrefs.SetInt("isMusic", isMusic ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ButtonAnimationClick(int index)
    {
        buttonAnimationClicks[index].transform.DOScale(Vector3.one * .9f, .15f).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
        {
            buttonAnimationClicks[index].transform.DOScale(Vector3.one, .15f).SetEase(Ease.InOutQuad).SetUpdate(true);
        });
    }
}
