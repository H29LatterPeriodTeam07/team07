using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReciptLogo : MonoBehaviour {

    public Sprite[] resultLogo;
    private Image myImage;

	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
        myImage.sprite = resultLogo[ScoreManager.GetStageNumber()-1];
        myImage.SetNativeSize(); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
