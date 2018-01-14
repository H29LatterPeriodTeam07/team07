using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colortest : MonoBehaviour {

    private float r = 1.0f;
    private int rp = 1;
    private Renderer myr;

	// Use this for initialization
	void Start () {
        myr = GetComponent<Renderer>();
        //myr.material.color = new Color(r, myr.material.color.g, myr.material.color.b, myr.material.color.a);
    }
	
	// Update is called once per frame
	void Update () {
        if (r > 0)
        {
            myr.material.color = new Color(r, r, r, myr.material.color.a);
            r -= Time.deltaTime * 0.1f;
        }
        else
        {
            r = 0;
        }
        //if (r > 1 || r < 0)
        //{
        //    rp *= -1;
        //}
        //r = Mathf.Clamp(rp, 0, 1);

        //myr.material.color = new Color(r, myr.material.color.g, myr.material.color.b, myr.material.color.a);
        //r += Time.deltaTime * rp;
    }
}
