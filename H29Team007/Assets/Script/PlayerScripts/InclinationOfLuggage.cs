using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclinationOfLuggage : MonoBehaviour
{

    private float slope;

    // Use this for initialization
    void Start()
    {
        slope = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetSlope()
    {
        return slope;
    }
}
