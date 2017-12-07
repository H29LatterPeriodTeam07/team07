using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIDead : MonoBehaviour {

    private Text m_Text;
    private float a = 255.0f;

	// Use this for initialization
	void Start () {
        m_Text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Text.color = new Color(0, 0, 0, a/255);
        a -= 255/1 * Time.deltaTime;
        if (a < 0)
        {
            if(name == "End")
            {
               //SceneManager.LoadScene("Result");
            }
            Destroy(gameObject);
        }
	}
}
