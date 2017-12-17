using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBackCamera : MonoBehaviour {

    private Camera myCamera;

    // Use this for initialization
    void Start () {
        myCamera = GetComponent<Camera>();
        DontDestroyOnLoad(this);
        myCamera.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    myCamera.enabled = true;
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    myCamera.enabled = false;
        //}
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

    void OnPostRender()
    {
        //myCamera.enabled = false;
    }
}
