using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTSenaka : MonoBehaviour {
    
    private SpriteRenderer mySprite;
    public Sprite[] numbers;
    private TutorialShopping sc;

    // Use this for initialization
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        sc = transform.root.GetComponent<TutorialShopping>();
    }

    // Update is called once per frame
    void Update()
    {
        int num = sc.GetHumanCount() - 1;
        if (num >= 10) num = 9;
        mySprite.sprite = numbers[num];
    }
}
