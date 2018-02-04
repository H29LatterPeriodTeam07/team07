using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnnounce : MonoBehaviour {

    private RectTransform myTransform;
    private Text announce;
    private RectTransform textTransform;

    private int nowIndex = 0;
    private MTManager tm;
    public string[] announceText;

    public float speed = 3.0f;

    // Use this for initialization
    void Start () {
        myTransform = GetComponent<RectTransform>();
        announce = transform.Find("Text").GetComponent<Text>();
        textTransform = transform.Find("Text").GetComponent<RectTransform>();
        tm = GameObject.Find("tutorialmanager").GetComponent<MTManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if(nowIndex != tm.TutorialIndex())
        {
            ChangeText();
            nowIndex = tm.TutorialIndex();
        }
        else if(tm.FadeEnd())
        {
            myTransform.position -= Vector3.right * speed;
            if(myTransform.position.x < -(textTransform.sizeDelta.x + 640))
            {
                myTransform.localPosition = Vector3.zero;
            }
        }
	}

    private void ChangeText()
    {
        string s = announce.text;
        switch (tm.TutorialIndex())
        {
            case 0: s = announceText[0]; break;
            case 1: s = announceText[1]; break;
            case 2: s = announceText[2]; break;
            case 3: s = announceText[3]; break;
            case 4: s = announceText[4]; break;
            case 5: s = announceText[5]; break;
            case 6: s = announceText[6]; break;
            case 7: s = announceText[7]; break;
            case 8: s = announceText[8]; break;
            case 9: s = announceText[9]; break;
            case 10: s = announceText[10]; break;
            case 11: s = announceText[11]; break;
            case 12: s = announceText[12]; break;
            case 13: s = announceText[13]; break;

        }
        if(announce.text != s)
        {
            myTransform.localPosition = Vector3.zero;
            announce.text = s;
        }
    }
}
