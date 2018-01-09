using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakinikuManager : MonoBehaviour
{
    private ParticleSystem myParticle;

    // Use this for initialization
    void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
        myParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider other)
    {
        Player ps = transform.root.GetComponent<Player>();
        if (ps == null) return;
        if (ps.GetFowardSpeed() > 0)
        {
            Yakiniku yaki = transform.parent.GetComponent<Yakiniku>();
            if (yaki == null) return;
            myParticle.Play();
            yaki.Fire();
        }
        else
        {
            myParticle.Stop();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        myParticle.Stop();
    }
}
