using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour {

    private RectTransform rectTransform = null;

    private Transform target = null;

    private RectTransform score = null;

    private float speed = 1;

    private float startTime;
    //private float journeyLength;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (score == null) Destroy(gameObject);
        //rectTransform.position = score.position;
        float time = (Time.time - startTime);
        //float fracJourney = distCovered / journeyLength;
        rectTransform.position = Vector3.Lerp(rectTransform.position, score.position, time / speed);//fracJourney);
        if(Vector3.Distance( rectTransform.position,score.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform tar,float s  = 1.0f)
    {
        score = transform.parent.GetComponent<RectTransform>();
        target = tar;
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
        speed = s;

        startTime = Time.time;
        //journeyLength = Vector3.Distance(rectTransform.position, score.position);
    }

}
