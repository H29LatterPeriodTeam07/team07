using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDebug : MonoBehaviour {

    public int stagenum = 0;

	// Use this for initialization
	void Start () {
        ScoreManager.StageChenge(stagenum);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
