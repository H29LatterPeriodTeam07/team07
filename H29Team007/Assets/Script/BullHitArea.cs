using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullHitArea : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
          /*  var sc = transform.root.GetComponent<BullCount>();
            if (transform.tag == "Animal" && !sc.IsHumanMoreThanAnimal()) return;
            myCollider.enabled = false;
            //ここにアニメ停止や変更入れるかも
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
            if (myNav != null) myNav.enabled = false;*/
        }
    }
}