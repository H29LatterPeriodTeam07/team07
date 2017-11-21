using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullHitArea : MonoBehaviour
{
    private float runOverAfterHeight = 1.0f;
    Rigidbody rb;
    NavMeshAgent nav;
    Collider m_col;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetHeight()
    {
        return runOverAfterHeight;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            rb = other.GetComponent<Rigidbody>();
            nav = other.GetComponent<NavMeshAgent>();
            m_col = other.GetComponent<Collider>();
            var sc = transform.root.GetComponent<BullCount>();

            m_col.enabled = false;
            //ここにアニメ停止や変更入れるかも
            Vector3 v = transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            other.transform.position = nimotuPos;
            sc.AddBaggege(other.transform);
            //transform.parent = other.transform.root;

            sc.PlusY(runOverAfterHeight);
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            if (nav != null) nav.enabled = false;
        }
    }
}