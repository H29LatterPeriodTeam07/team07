using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour {

    public CharaFall[] falls;
    private int index = 0;

	// Use this for initialization
	void Start () {
        falls[index].FallStart(ScoreManager.GetCount(index+1));
	}
	
	// Update is called once per frame
	void Update () {
        if (index == falls.Length - 1) return;
        if (falls[index].IsEnd())
        {
            index++;
            falls[index].FallStart(ScoreManager.GetCount(index+1));
        }
    }
}
