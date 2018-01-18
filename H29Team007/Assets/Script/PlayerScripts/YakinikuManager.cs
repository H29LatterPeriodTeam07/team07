using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakinikuManager : MonoBehaviour
{
    //private ParticleSystem myParticle;
    private Player ps;

    // Use this for initialization
    void Start()
    {
        //myParticle = GetComponent<ParticleSystem>();
        //myParticle.Stop();
        ps = transform.root.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.root.tag != "Player")
        {
            //myParticle.Stop();
            return;
        }
        if (transform.position.y > 5.0f)
        {
            //if (ps.GetFowardSpeed() > 0)
            //{
                Yakiniku yaki = transform.parent.GetComponent<Yakiniku>();
                if (yaki == null) return;
                //myParticle.Play();
            Debug.Log("天井に着いた");
                yaki.Fire();
            //}
        }
        else {
            //myParticle.Stop();
        }
        if (ps.GetState() > Player.PlayerState.Takeover)
        {
            //myParticle.Stop();
        }
    }

    //public void OnTriggerStay(Collider other)
    //{
    //    if (transform.root.tag != "Player") return;
    //    //Player ps = transform.root.GetComponent<Player>();
    //    //if (ps == null) {
    //    //    myParticle.Stop();
    //    //    return;
    //    //}
    //    if (ps.GetFowardSpeed() > 0)
    //    {
    //        Yakiniku yaki = transform.parent.GetComponent<Yakiniku>();
    //        if (yaki == null) return;
    //        myParticle.Play();
    //        yaki.Fire();
    //        Debug.Log(other.name);
    //    }
    //    else
    //    {
    //        myParticle.Stop();
    //    }
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    myParticle.Stop();
    //}
}
