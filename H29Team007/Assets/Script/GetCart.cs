using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCart : MonoBehaviour {


    private GameObject myCart;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (myCart != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                myCart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                myCart.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationZ |
                    RigidbodyConstraints.FreezePositionY;

                myCart.transform.parent = null;

                myCart = null;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "BackHitArea")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                myCart = other.transform.parent.transform.gameObject;
                // Debug.Log(myCart);
                //transform.LookAt(myCart.transform.forward);
                Vector3 relativePos = myCart.transform.position - transform.position;
                relativePos.y = 0; //上下方向の回転はしないように制御
                transform.rotation = Quaternion.LookRotation(relativePos);
                myCart.transform.parent = transform;
                myCart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
