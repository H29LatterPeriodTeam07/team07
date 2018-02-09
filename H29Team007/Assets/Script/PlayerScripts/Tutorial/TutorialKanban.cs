using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialKanban : MonoBehaviour {

    private Image myImg;
    public Sprite[] padImgs;
    private float time = 0;
    private float activetime = 0.7f;
    private MTManager tm;

	// Use this for initialization
	void Start () {
        myImg = GetComponent<Image>();
        tm = GameObject.Find("tutorialmanager").GetComponent<MTManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!tm.FadeEnd() || tm.TutorialIndex() == 77) {
            myImg.sprite = padImgs[0];
            time = 0.0f;
            return;
        }
		if(activetime < time)
        {
            ImageActive();
            time = 0.0f;
        }
        time += Time.deltaTime;
	}

    private void ImageActive()
    {
        if (myImg.sprite == padImgs[0])
        {
            myImg.sprite = padImgs[tm.TutorialIndex() + 1];
        }
        else
        {
            myImg.sprite = padImgs[0];
        }
    }
}
