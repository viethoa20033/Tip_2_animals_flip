using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D coll;
    private AudioSource source;
    
    [Header("Card State")]
    public bool isClick;
    public bool isFacingTrue;

    [Header("Card Set")] 
    public int type;
    public SpriteRenderer spr;
    public Sprite[] sprites;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(ReviewCard());
    }

    private void Update()
    {
        if (!isFacingTrue)
        { 
            anim.SetBool("isClick",isClick);
        }
        else
        {
            coll.enabled = false;
        }
    }

    public void SetCard(int _type)
    {
        type = _type;
        spr.sprite = sprites[type];
    }

    IEnumerator ReviewCard()
    {
        coll.enabled = false;
        isClick = true;

        yield return new WaitForSeconds(5f);
        coll.enabled = true;
        isClick = false;
    }

    public void FailCard()
    {
        transform.DOScale(transform.localScale * 1.25f, .5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            transform.DOScale(transform.localScale / 1.25f, .5f).SetEase(Ease.InOutQuad);
        });
    }

    public void EventSource()
    {
        source.Play();
    }
}
