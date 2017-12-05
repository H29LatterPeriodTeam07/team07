using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Announce : MonoBehaviour {

    public float x;
    Vector2 v;
    Rigidbody2D rb;
    Renderer _renderer;
    Vector2 BasePosition;

    public GameObject[] _announce;

    Event m_Event;
    float time = 0;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        BasePosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {


        x = 0;
        if (time < 120f)
        {
            //gameObject.SetActive(true);
            time++;
            x -= 6f;
            v = new Vector2(x, rb.velocity.y);
            transform.Translate(v);
        }
        else
        {
            //gameObject.SetActive(false);
            time = 0;
            x = 0;
            this.transform.position = BasePosition;
            //Destroy(this.gameObject);
        }
    }
}
