using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MTDescriptionImage : MonoBehaviour {

    private GameObject setumeiImages;
    private Image leftImage;
    private Image rightImage;

    public Sprite togyu;
    public Sprite same;

    private bool ischange = false;
    private int nowIndex = -1;


    private MTManager tm;

    // Use this for initialization
    void Start () {
        setumeiImages = transform.Find("ImageBack").gameObject;
        leftImage = setumeiImages.transform.Find("LeftImage").GetComponent<Image>();
        rightImage = setumeiImages.transform.Find("RightImage").GetComponent<Image>();
        setumeiImages.SetActive(false);
        tm = GameObject.Find("tutorialmanager").GetComponent<MTManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!tm.FadeEnd() || tm.TutorialIndex() == 77) return;

        if (nowIndex != tm.TutorialIndex())
        {
            nowIndex = tm.TutorialIndex();
            if(nowIndex ==11 ||nowIndex == 14) setumeiImages.SetActive(true);
            if (nowIndex == 12 || nowIndex == 15) setumeiImages.SetActive(false);
            if(nowIndex == 13)
            {
                leftImage.sprite = togyu;
                rightImage.sprite = same;
            }
        }
    }
}
