using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketFly : MonoBehaviour
{

    private Rigidbody m_rigid;

    // Use this for initialization
    void Start()
    {
        //enabled = false;
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.AddForce(transform.forward * 20.0f, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_rigid.velocity == Vector3.zero)
        {

            m_rigid.constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
    
    
}
