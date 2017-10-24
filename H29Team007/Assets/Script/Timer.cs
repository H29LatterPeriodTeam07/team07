using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    Slider m_slider;
    public float timer = 0.0f;

	// Use this for initialization
	void Start () {
        m_slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        //タイムアップ
        if(timer > m_slider.maxValue)
        {
            timer = 0.0f;
            //リザルト画面表示
        }

        m_slider.value = timer;
	}
}
