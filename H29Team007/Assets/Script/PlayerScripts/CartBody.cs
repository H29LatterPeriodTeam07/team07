using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBody : MonoBehaviour
{

    public float wallDamage = 5.0f;

    private CartStatusWithPlayer cs;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.root.tag == "Player")
        {
            if (cs == null) cs = transform.root.GetComponent<CartStatusWithPlayer>();
        }
        else
        {
            cs = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (cs == null)
        {
            return;
        }
        switch (other.transform.tag)
        {
            case "Wall":
                cs.DamageCart(wallDamage);
                break;
        }
    }
}
