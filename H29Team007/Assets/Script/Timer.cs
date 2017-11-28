﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    Slider m_slider;
    public float timer = 0.0f;

    public CountDown countdownUI;
    private bool iscdon = false;

	// Use this for initialization
	void Start () {
        m_slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!MainGameDate.IsStart())
        {

            return;
        }
        timer += Time.deltaTime * 2;

        //タイムアップ
        if (timer > m_slider.maxValue)
        {
            timer = 0.0f;
            //リザルト画面表示
            SceneManager.LoadScene("Result");

        }

        //終了カウントの表示
        if (m_slider.maxValue - timer <= 3&& !iscdon)
        {
            countdownUI.enabled = true;
            countdownUI.SetCount(m_slider.maxValue - timer);
            iscdon = true;
        }

        m_slider.value = timer;
	}
}
