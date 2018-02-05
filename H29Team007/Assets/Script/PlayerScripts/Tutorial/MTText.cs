using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MTText : MonoBehaviour {
    
    private Text setumeititle;
    private Text setumei;
    private GameObject buttonImgG;
    private Image buttonImg;
    private Color buttonColor;
    private Color buttonColorDown;

    private int nowIndex = -1;
    private MTManager tm;
    public string[] setumeiTextTitle;
    public string[] setumeiText;

    // Use this for initialization
    void Start () {
        setumeititle = transform.Find("TextTitle").GetComponent<Text>();
        setumei = transform.Find("Text").GetComponent<Text>();
        buttonImgG = transform.Find("BImg").gameObject;
        buttonImg = buttonImgG.GetComponent<Image>();
        buttonColor = buttonImg.color;
        buttonColorDown = buttonImg.color - new Color(100,0,0,0);
        tm = GameObject.Find("tutorialmanager").GetComponent<MTManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!tm.FadeEnd() || tm.TutorialIndex() == 77) return;
        if (nowIndex != tm.TutorialIndex())
        {
            ChangeText();
            nowIndex = tm.TutorialIndex();
            if (tm.TutorialIndex() == 0
                || tm.TutorialIndex() == 3
                || tm.TutorialIndex() == 9
                || tm.TutorialIndex() == 10
                || tm.TutorialIndex() == 11
                || tm.TutorialIndex() == 12
                || tm.TutorialIndex() == 14
                || tm.TutorialIndex() == 15) ChangeButtonActive();
        }
        if (Input.anyKey)
        {
            buttonImg.color = buttonColorDown;
        }
        else
        {
            buttonImg.color = buttonColor;
        }
    }

    private void ChangeButtonActive()
    {
        if (buttonImgG.activeSelf)
        {
            buttonImgG.SetActive(false);
        }
        else
        {
            buttonImgG.SetActive(true);
        }
    }

    private void ChangeText()
    {
        if (tm.TutorialIndex() >= setumeiText.Length) return;
        string s = setumeiText[tm.TutorialIndex()];
        
        if (setumei.text != s)
        {
            setumei.text = s;
            setumeititle.text = setumeiTextTitle[tm.TutorialIndex()];
        }
    }
}
