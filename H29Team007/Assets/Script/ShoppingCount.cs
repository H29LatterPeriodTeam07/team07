using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCount : MonoBehaviour {

    private float onPosition;

	// Use this for initialization
	void Start () {
        onPosition = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlusY(float y)
    {
        onPosition += y;
    }

    public void Reset()
    {
        onPosition = 0.5f;
    }

    public float GetY()
    {
        return onPosition;
    }
}
