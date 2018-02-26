using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MTOK : MonoBehaviour {

    private Text my_Text;
    private float alpha = 0.0f;

	// Use this for initialization
	void Start () {
        my_Text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (alpha == 0.0f) return;
        alpha -= Time.deltaTime;
        if (alpha < 0) alpha = 0.0f;
        if(Time.timeScale == 0) alpha = 0.0f;
        my_Text.color = new Color(my_Text.color.r, my_Text.color.g, my_Text.color.b, alpha);
	}

    public void Reborn()
    {
        alpha = 1.0f;
    }
}
