﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCartSparks : MonoBehaviour {

    ParticleSystem myParticle;
    MTPlayer playerScript;

    // Use this for initialization
    void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
        playerScript = transform.root.GetComponent<MTPlayer>();

        myParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");

        if (playerScript.GetState() == MTPlayer.PlayerState.Gliding && inputHorizontal != 0)
        {
            myParticle.Play();
        }
        else
        {
            myParticle.Stop();
        }
    }
}
