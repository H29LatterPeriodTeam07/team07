using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullHitArea : MonoBehaviour
{
    [SerializeField, Header("カートに乗った後の高さ")]
    private float runOverAfterHeight = 1.0f;

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
            var sc = transform.root.GetComponent<BullCount>();
            var rb = other.GetComponent<Rigidbody>();
            if (transform.tag == "Animal" && !sc.IsHumanMoreThanAnimal()) return;
            Vector3 v = other.transform.parent.transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            sc.AddBaggege(transform);
            //transform.parent = other.transform.root;

            sc.PlusY(runOverAfterHeight);
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }
}