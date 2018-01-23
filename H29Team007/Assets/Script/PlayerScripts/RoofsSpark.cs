using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofsSpark : MonoBehaviour {

    ParticleSystem myParticle;
    Player playerScript;

    // Use this for initialization
    void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
        playerScript = transform.root.GetComponent<Player>();

        myParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");

        if (playerScript.GetFowardSpeed() > 0.1f * 0.1f) 
        {
            myParticle.Play();
        }
        else
        {
            myParticle.Stop();
        }
    }
}
