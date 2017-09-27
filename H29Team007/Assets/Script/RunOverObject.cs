using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOverObject : MonoBehaviour
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")
        {
            var sc = other.gameObject.GetComponent<ShoppingCount>();
            Vector3 v = other.transform.parent.transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            transform.parent = other.transform.root;

            sc.PlusY(runOverAfterHeight);
            //Debug.Log("a");
        }
    }
}
