using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialKanban : MonoBehaviour {

    private Image myImg;
    public Sprite[] padImgs;
    private float time = 0;
    private float activetime = 1.0f;
    private TutorialManager tm;

	// Use this for initialization
	void Start () {
        myImg = GetComponent<Image>();
        tm = GameObject.Find("tutorialmanager").GetComponent<TutorialManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!tm.FadeEnd()) return;
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
            Sprite sprite = padImgs[0];
            switch (tm.TutorialIndex())
            {
                case 0: sprite = padImgs[1]; break;
                case 1: sprite = padImgs[2]; break;
                case 2: sprite = padImgs[3]; break;
                case 3: sprite = padImgs[4]; break;
                case 4: sprite = padImgs[5]; break;
                case 5: sprite = padImgs[6]; break;
                case 6: sprite = padImgs[7]; break;
                case 7: sprite = padImgs[8]; break;
                case 8: sprite = padImgs[9]; break;
                case 9: sprite = padImgs[10]; break;
                case 10: sprite = padImgs[11]; break;
                case 11: sprite = padImgs[12]; break;
                case 12: sprite = padImgs[13]; break;
                case 13: sprite = padImgs[14]; break;

            }
            myImg.sprite = sprite;
        }
        else
        {
            myImg.sprite = padImgs[0];
        }
    }
}
