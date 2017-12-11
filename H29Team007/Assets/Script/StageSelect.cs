using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {

    private int stageNum = 1;
    private bool isMove = false;
    public RectTransform[] stages;
    private RectTransform leftStage;
    private RectTransform rightStage;
    private RectTransform centerStage;

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    private float oktime = 0.3f;

    public GameObject yajirushi;

    // Use this for initialization
    void Start () {
        leftStage = stages[stageNum-1];
        centerStage = stages[stageNum];
        rightStage = stages[stageNum+1];
        startTime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
        if ((int)Time.time % 2 == 0)
        {
            yajirushi.SetActive(true);
        }
        else
        {
            yajirushi.SetActive(false);
        }

        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        if(inputHorizontal != 0 && Time.time - startTime > oktime)
        {
            if(inputHorizontal > 0)
            {
                stageNum = (stageNum + 1) % stages.Length;
                centerStage.localPosition -= new Vector3(1280, 0, 0);
                centerStage = stages[stageNum];
                centerStage.localPosition = Vector3.zero;
            }
            else
            {
                stageNum--;
                if (stageNum < 0) stageNum = stages.Length - 1;
                centerStage.localPosition += new Vector3(1280, 0, 0);
                centerStage = stages[stageNum];
                centerStage.localPosition = Vector3.zero;
            }
            startTime = Time.time;
        }
        if(Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            switch (stageNum)
            {
                case 0: SceneManager.LoadScene("Tutorial"); break;
                case 1: SceneManager.LoadScene("Stage1"); break;
                case 2: SceneManager.LoadScene("Stage2.1"); break;
                case 3: SceneManager.LoadScene("Stage3"); break;
            }
        }
        if (Input.GetButton("XboxA") || Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("Title");
        }
        //if(inputHorizontal != 0 && !isMove)
        //{
        //    startTime = Time.time;
        //    journeyLength = Vector.Distance()
        //}

    }
}
