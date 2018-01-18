using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupScore2D : MonoBehaviour {

    private RectTransform rectTransform;
    private RectTransform score = null;
    private float speed = 2;
    private float startTime;

    private Text myText;
    //private float a = 1;
    public Color orengeColor;

    private bool isWaiting = true;

    void Awake()
    {
        myText = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (score == null) Destroy(gameObject);
        float time = (Time.time - startTime);
        if (!isWaiting)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, score.position + Vector3.right * score.sizeDelta.x / 2, time / speed);
            if (Vector3.Distance(rectTransform.position, score.position + Vector3.right * score.sizeDelta.x / 2) < rectTransform.sizeDelta.y / 2)
            {
                Destroy(gameObject);
            }
        }
        else if(time > 1)
        {
            isWaiting = false;
            startTime = Time.time;
        }


    }

    public void SetTarget(int n)
    {
        score = transform.parent.GetComponent<RectTransform>();
        rectTransform.position = score.position - Vector3.up * (rectTransform.sizeDelta.y/2) * (n + 4) + Vector3.right * score.sizeDelta.x / 2;
        startTime = Time.time;
    }

    public void SetText(string text)
    {
        myText.text = text;
    }

    public void SetOutColorOrange()
    {
        Outline outcolor = GetComponent<Outline>();
        outcolor.effectColor = orengeColor;
    }
}
