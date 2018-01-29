using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDie : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        ParticleSystem myParticle = GetComponent<ParticleSystem>();
        var main = myParticle.main;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, main.duration);
	}

}
