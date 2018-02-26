using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikuPika : MonoBehaviour {

    private Player ps;
    private ParticleSystem m_particle;
    private bool nowPlay = false;

	// Use this for initialization
	void Start () {
        ps = transform.root.GetComponent<Player>();
        m_particle = GetComponent<ParticleSystem>();
        m_particle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		if(ps.NikuSpeed() == 2&& !nowPlay)
        {
            nowPlay = true;
            m_particle.Play();
        }
        else if (ps.NikuSpeed() == 1 && nowPlay)
        {
            nowPlay = false;
            m_particle.Stop();
        }
	}
}
