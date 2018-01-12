using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCart : MonoBehaviour {

    public GameObject cartRigidPrefab;
    private GameObject newcart;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.root.name == "Player")
        {
            Independence();
            Destroy(gameObject);
        }
	}

    public void Independence()
    {
        newcart = Instantiate(cartRigidPrefab);
        newcart.transform.position = new Vector3(transform.position.x, 0, transform.position.z);// + transform.forward;
        newcart.transform.rotation = transform.rotation;
       // transform.root.position -= transform.root.forward;
    }

    public GameObject NewCart()
    {
        return newcart;
    }
}
