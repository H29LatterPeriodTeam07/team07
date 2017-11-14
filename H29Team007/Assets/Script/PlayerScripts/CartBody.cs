﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBody : MonoBehaviour
{

    public float wallDamage = 5.0f;

    private CartStatusWithPlayer cs;
    private GameObject rotatepoint;

    public Vector3 willyPoint = new Vector3(0, 0, 0.3f);
    public Vector3 motiagePoint = new Vector3(0, 0, 1.65f);

    private bool isR = false;

    private bool isWilly = false;

    // Use this for initialization
    void Start()
    {
        cs = transform.root.GetComponent<CartStatusWithPlayer>();
        rotatepoint = transform.root.Find("cartrotatepoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWilly)
        {
            Willy();
        }
        else
        {
            Normal();
        }
    }

    /// <summary>通常時</summary>
    private void Normal()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            rotatepoint.transform.localPosition = willyPoint;
            SlopeCart(-13);
            isR = true;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            rotatepoint.transform.localPosition = motiagePoint;
            SlopeCart(13);
            isR = false; 
        }
    }

    /// <summary>ウィリー時</summary>
    private void Willy()
    {
        if (isR&&Input.GetKeyUp(KeyCode.L)
            || !isR && Input.GetKeyUp(KeyCode.K))
        {
            NoSlopeCart();
        }
    }

    /// <summary>カート傾け</summary>
    /// <param name="angle">どのくらい傾けるか</param>
    private void SlopeCart(float angle)
    {
        cs.SetBasketParent(rotatepoint.transform);
        transform.parent = rotatepoint.transform;
        rotatepoint.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(1, 0, 0));
        isWilly = true;
    }

    /// <summary>カート傾けない</summary>
    private void NoSlopeCart()
    {
        rotatepoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        cs.SetBasketParent(transform.root);
        transform.parent = transform.root;
        isWilly = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        
        switch (other.transform.tag)
        {
            case "Wall":
                cs.DamageCart(wallDamage);
                break;
        }
    }
}
