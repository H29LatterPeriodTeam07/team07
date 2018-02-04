using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutrialFallDown : MonoBehaviour {

    private Vector3 startPos;
    private Vector3 targetPos;

    private float speed = 1.0f;
    private float startTime;
    private float distance;

    private MTRunOver runScr;

    // Use this for initialization
    void Start()
    {
        runScr = GetComponent<MTRunOver>();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            enabled = false;
            return;
        }
        float basyo = (Time.time - startTime) * speed;
        float donohenka = basyo / distance;
        transform.position = Vector3.Lerp(startPos, targetPos, donohenka);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos;
            runScr.NavPosition(targetPos);
            enabled = false;
        }
    }

    public void SetPoint(Vector3 pos, float sp)
    {
        startPos = transform.position;
        speed = sp;
        targetPos = pos;
        startTime = Time.time;
        distance = Vector3.Distance(transform.position, targetPos);
        
        runScr.NavReStart();
    }
}
