using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {

    private enum SelectState
    {
        Wait,
        LeftMove,
        RightMove
    }

    private int stageNum = 0;
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

    public SoundManagerScript sm;

    private SelectState myState;
    public GameObject m_NowLoad;
    AsyncOperation async;

    // Use this for initialization
    void Start () {
        leftStage = stages[StageNumber(-1)];
        centerStage = stages[stageNum];
        rightStage = stages[StageNumber(1)];
        startTime = Time.time;
        leftStage.localPosition = new Vector3(-1280, 0, 0);
        centerStage.localPosition = Vector3.zero;
        rightStage.localPosition = new Vector3(1280, 0, 0);
        myState = SelectState.Wait;
        m_NowLoad.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        switch (myState)
        {
            case SelectState.Wait: Wait(); break;
            case SelectState.LeftMove: LeftMove(); break;
            case SelectState.RightMove: RightMove(); break;
        }
    }

    private void ChangeState(int num)
    {
        switch (num)
        {
            case 0: myState = SelectState.Wait; break;
            case 1: myState = SelectState.LeftMove; break;
            case 2: myState = SelectState.RightMove; break;
        }
    }

    private void Wait()
    {
        if ((int)Time.time % 2 == 0)
        {
            yajirushi.SetActive(true);
        }
        else
        {
            yajirushi.SetActive(false);
        }

        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        if (inputHorizontal != 0 && Time.time - startTime > oktime)
        {
            if (inputHorizontal > 0)
            {
                ChangeState(2);
                stageNum = (stageNum + 1) % stages.Length;
                yajirushi.SetActive(false);
            }
            else
            {
                ChangeState(1);
                stageNum--;
                if (stageNum < 0) stageNum = stages.Length - 1;
                yajirushi.SetActive(false);
            }
            startTime = Time.time;
        }
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            sm.PlaySE(0);
            switch (stageNum)
            {
                case 0: //SceneManager.LoadScene("Tutorial"); 
                    async = SceneManager.LoadSceneAsync("Tutorial");
                    StartCoroutine("LoadScene");
                    break;
                case 1:
                    // SceneManager.LoadScene("Stage1");
                    async = SceneManager.LoadSceneAsync("Stage1");
                    StartCoroutine("LoadScene");
                    ScoreManager.StageChenge(1);
                    break;
                case 2:
                    //SceneManager.LoadScene("Stage2.1");
                    async = SceneManager.LoadSceneAsync("Stage2.1");
                    StartCoroutine("LoadScene");
                    ScoreManager.StageChenge(2);
                    break;
                case 3:
                    //SceneManager.LoadScene("Stage3");
                    async = SceneManager.LoadSceneAsync("Stage3");
                    StartCoroutine("LoadScene");
                    ScoreManager.StageChenge(3);
                    break;
            }
            m_NowLoad.SetActive(true);
        }
        if (Input.GetButton("XboxA") || Input.GetKeyDown(KeyCode.F))
        {
            sm.PlaySE(1);
            SceneManager.LoadScene("Title");
        }


    }

    private void LeftMove()
    {

        centerStage.localPosition = Vector3.Lerp(centerStage.localPosition, new Vector3(1280, 0, 0), ((Time.time - startTime) * 0.5f) / oktime);
        leftStage.localPosition = Vector3.Lerp(leftStage.localPosition, Vector3.zero, ((Time.time - startTime) * 0.5f) / oktime);
        if (((Time.time - startTime) * 0.5f) > oktime)
        {
            rightStage = centerStage;
            centerStage = leftStage;
            leftStage = stages[StageNumber(-1)];
            leftStage.localPosition = new Vector3(-1280, 0, 0);
            centerStage.localPosition = Vector3.zero;
            rightStage.localPosition = new Vector3(1280, 0, 0);
            ChangeState(0);
        }
    }

    private void RightMove()
    {

        centerStage.localPosition = Vector3.Lerp(centerStage.localPosition, new Vector3(-1280, 0, 0), ((Time.time - startTime) * 0.5f) / oktime);
        rightStage.localPosition = Vector3.Lerp(rightStage.localPosition, Vector3.zero, ((Time.time - startTime) * 0.5f) / oktime);
        if (((Time.time - startTime) * 0.5f) > oktime)
        {
            leftStage = centerStage;
            centerStage = rightStage;
            rightStage = stages[StageNumber(1)];
            leftStage.localPosition = new Vector3(-1280, 0, 0);
            centerStage.localPosition = Vector3.zero;
            rightStage.localPosition = new Vector3(1280, 0, 0);
            ChangeState(0);
        }
    }

    private int StageNumber(int i)
    {
        int result = stageNum + i;
        if (result < 0) result = stages.Length - 1;
        if (result >= stages.Length) result = 0;
        return result;
    }
    IEnumerator LoadScene()
    {
        async.allowSceneActivation = false;    // シーン遷移をしない

        while (async.progress < 0.9f)
        {
            Debug.Log(async.progress);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Scene Loaded");


        yield return new WaitForSeconds(1);

        async.allowSceneActivation = true;    // シーン遷移許可

    }
}
