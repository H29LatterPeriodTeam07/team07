﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    Slider m_slider;
    public float timer;

    public float stageTime = 100.0f;

    public CountDown countdownUI;
    public SaleSpown ss;
    private bool iscdon = false;

    public GameObject linePre;

	// Use this for initialization
	void Start () {
        m_slider = GetComponent<Slider>();
        timer = 0.0f;
        if (ss == null) return;
        for(int i = 0; i < ss.GetAppearTime().Length; i++)
        {
            Debug.Log("yes");
            GameObject line = Instantiate(linePre);
            line.transform.parent = transform.parent;
            line.GetComponent<RectTransform>().localPosition = gameObject.GetComponent<RectTransform>().localPosition;
            line.transform.parent = transform;
            line.GetComponent<Slider>().value = ss.GetAppearTime()[i];
        }
        Debug.Log("no");
    }
	
	// Update is called once per frame
	void Update () {
        if (!MainGameDate.IsStart())
        {

            return;
        }
        timer += Time.deltaTime;// * m_slider.maxValue / stageTime;
        ss.SetTime(timer);
        //タイムアップ
        if (timer > m_slider.maxValue)
        {
            timer = m_slider.maxValue;
            //リザルト画面表示
            //SceneManager.LoadScene("Result");

        }

        //終了カウントの表示
        if (m_slider.maxValue - timer <= 4 && !iscdon)
        {
            countdownUI.enabled = true;
            countdownUI.SetCount(3);// m_slider.maxValue - timer);
            iscdon = true;
        }

        m_slider.value = timer;
	}

    public float NowTime()
    {
        return timer;
    }
}
