using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStop : MonoBehaviour {

    private Animator m_Anim;

	// Use this for initialization
	void Start () {
        m_Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.parent != null)
        {
            m_Anim.speed = 0;
        }
        else
        {
            m_Anim.speed = 1;
        }
	}
}
