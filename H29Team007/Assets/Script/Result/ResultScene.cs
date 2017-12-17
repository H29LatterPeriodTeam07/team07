using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScene : MonoBehaviour {


    public SoundManagerScript sm;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            sm.PlaySE(0);
            SceneManager.LoadScene("Title");

        }
    }
    
}
