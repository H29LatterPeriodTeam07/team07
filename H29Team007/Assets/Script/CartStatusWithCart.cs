using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStatusWithCart : MonoBehaviour {


    [SerializeField, Header("全体、カート下、荷台？の下、持つところの耐久値")]
    private float[] cartStatus = {100.0f,30.0f,30.0f,30.0f };

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public float[] PassStatus()
    {
        return cartStatus;
    }

    public void SetStatus(float[] status)
    {
        cartStatus = status;
    }
}
