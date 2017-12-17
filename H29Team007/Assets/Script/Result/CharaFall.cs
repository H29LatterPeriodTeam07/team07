using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaFall : MonoBehaviour {

    private int count = 0;
    private int maxCount = 0;

    private float time = 1.0f;
    private float nowtime;

    public GameObject fallChara;

    private bool isEnd = true;

	// Use this for initialization
	void Start () {
        //nowtime = Time.time;
        //maxCount = 5;
}
	
	// Update is called once per frame
	void Update () {
        if (isEnd) return;
		if(count < maxCount)
        {
            if (nowtime < Time.time - time)
            {
                GameObject chara = Instantiate(fallChara);
                chara.transform.position = transform.position;
                nowtime = Time.time;
                count++;
            }
        }
        else
        {
            isEnd = true;
        }
	}

    public void FallStart(int count)
    {
        nowtime = Time.time;
        maxCount = count;
        isEnd = false;
    }

    public bool IsEnd()
    {
        return isEnd;
    }
}
