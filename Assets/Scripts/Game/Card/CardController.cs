using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card card1;
    public Card card2;

    public LayerMask cardLayer;

    public bool isClick;

    [Header("Sound")] 
    public AudioSource wrongSound;
    public AudioSource trueSound;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isClick && GameManager.Instance.gameState == GameState.Playing)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, cardLayer);

            if (hit.collider != null)
            {
                SetCard(hit.transform.GetComponent<Card>());
            }
        }
    }

    void SetCard(Card _card)
    {
        if (card1 == null)
        {
            card1 = _card;
            card1.isClick = true;
            return;
        }
        else
        {
            if (card1 != _card)
            {
                card2 = _card;
                card2.isClick = true;
                isClick = true;
                StartCoroutine(CheckCards());
            }
        }
    }
    
   
    IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(.5f);
        if (card1.type == card2.type)
        {
            trueSound.Play();
            
            card1.isFacingTrue = true;
            card2.isFacingTrue = true;
            
            yield return new WaitForSeconds(.5f);
            
            card1 = null;
            card2 = null;
            
            isClick = false;
        }
        else
        {
            wrongSound.Play();
            
            card1.FailCard();
            card2.FailCard();
            
            yield return new WaitForSeconds(1f);
            
            card1.isClick = false;
            card2.isClick = false;

            card1 = null;
            card2 = null;


            isClick = false;
        }
    }
}
