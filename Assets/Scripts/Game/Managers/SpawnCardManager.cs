using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCardManager : MonoBehaviour
{
    private List<Transform> cardPoints = new List<Transform>();
    private GameObject setCard;
    public GameObject cardPrefab;
            
    [Header("Set Card Mode 8 Animals")]
    public GameObject[] setCards1;
    
    [Header("Set Card Mode 16 Animals")]
    public GameObject[] setCards2;
    
    [Header("Set Card Mode 24 Animals")]
    public GameObject[] setCards3;

    private List<int> cardTypes = new List<int>();//type in game
    private List<GameObject> cardGames = new List<GameObject>();    //Card in game

    private bool isLoadMap;
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
        switch (state)
        {
            case GameState.ChooseLevel:
                isLoadMap = false;
                break;
            case GameState.Playing:
                if (!isLoadMap)
                {
                    SpawnCard();
                    isLoadMap = true;
                }
                break;
            case GameState.GameEnd:
                isLoadMap = false;
                break;
        }
    }
    
    public void SpawnCard()
    { 
        //Destroy card and set map
        GameObject[] _setCards = GameObject.FindGameObjectsWithTag("Set Card");
        foreach (var _setCard in _setCards)
        {
            Destroy(_setCard);
        }
        
        GameObject[] cards = GameObject.FindGameObjectsWithTag("card");
        foreach (var card in cards)
        {
            Destroy(card);
        }

        
        SpawnSetCards(LevelManager.Instance.modeAnimal);
        
        cardTypes.Clear();
        cardGames.Clear();
        
        for (int i = 0; i < LevelManager.Instance.modeAnimal; i++)
        {
            cardTypes.Add(i);
        }
        
        int coutCard = cardPoints.Count / 2;
        for (int i = 0; i < coutCard; i++)
        {
            int type = cardTypes[Random.Range(0, cardTypes.Count)];
            cardTypes.Remove(type);

            for (int j = 0; j < 2; j++)
            {
                Transform randomCard = cardPoints[Random.Range(0, cardPoints.Count)];
                cardPoints.Remove(randomCard);
                
                GameObject card = Instantiate(cardPrefab, randomCard.position, quaternion.identity);
                card.GetComponent<Card>().SetCard(type);
                
                cardGames.Add(card);
            }
        }
    }

    void SpawnSetCards(int modeAnimals)
    {
        int coutSpawn = (LevelManager.Instance.level - 1) / 3;
        switch (modeAnimals)
        {
            case 8:
                setCard = Instantiate(setCards1[coutSpawn]);
                break;
            case 16:
                setCard = Instantiate(setCards2[coutSpawn]);
                break;
            case 24:
                setCard = Instantiate(setCards3[coutSpawn]);
                break;
        }
        
        cardPoints = setCard.GetComponent<SetCard>().cardPoints;
    }
    //check win the game
    bool AllCardsFacing()
    {
        foreach (GameObject card in cardGames)
        {
            if (!card.GetComponent<Card>().isFacingTrue)
            {
                return false;
            }
        }
        return true; 
    }

    private void Update()
    {
        if (cardGames.Count > 0 && AllCardsFacing() && GameManager.Instance.isPlaying)
        {
            GameManager.Instance.UpdateGameState(GameState.GameEnd, true);
        }
    }
}
