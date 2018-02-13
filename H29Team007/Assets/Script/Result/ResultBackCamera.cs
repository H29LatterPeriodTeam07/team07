using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBackCamera : MonoBehaviour {

    private Camera myCamera;

    public static ResultBackCamera Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        // 重複防止措置。
        // 既にある場合は自身を削除する
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        myCamera = GetComponent<Camera>();
        myCamera.enabled = false;
    }

    // Use this for initialization
    //void Start () {
    //    myCamera = GetComponent<Camera>();
    //    DontDestroyOnLoad(this);
    //    myCamera.enabled = false;
    //}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void ScreenCapture()
    {
        myCamera.enabled = true;
        Transform camPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        transform.position = camPos.position;
        transform.eulerAngles = camPos.eulerAngles;
    }

    public void Stop()
    {
        myCamera.enabled = false;
    }
    
}
