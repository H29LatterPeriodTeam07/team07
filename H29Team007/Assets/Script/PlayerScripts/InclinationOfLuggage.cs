using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclinationOfLuggage : MonoBehaviour
{

    private float slope;
    public float newslope = 30;

    private int nowChildCount;

    // Use this for initialization
    void Start()
    {
        slope = 10;
        nowChildCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount != nowChildCount)
        {
            RotateChild();
            nowChildCount = transform.childCount;
        }
        //foreach (Transform child in transform)
        //{
        //    child.RotateAround(transform.position, transform.right, 10 * Time.deltaTime);
        //}
    }

    private void RotateChild()
    {
        if(nowChildCount != 0)
        {
            foreach (Transform child in transform)
            {
                child.RotateAround(transform.position, transform.right, -slope);
            }
        }
        foreach (Transform child in transform)
        {
            child.RotateAround(transform.position, transform.right, slope);
        }
        //slope = newslope;
    }

    public void RotateNewChild(Transform newChild)
    {
        newChild.RotateAround(transform.position, transform.right, slope);
    }


    public float GetSlope()
    {
        return slope;
    }

    public void PlusSlope(float angle)
    {
        slope += angle;
    }
}
