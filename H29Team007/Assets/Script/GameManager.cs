using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //出口
    public Transform exitPoint;
    //BBAオブジェクト
    private GameObject m_BBA;
    //警備員オブジェクト
    private GameObject m_SG;


    //サウンドマネージャーオブジェクト
    GameObject SM;
    //サウンドマネージャースクリプト
    SoundManagerScript smScript;

    // Use this for initialization
    void Start () {
        m_BBA = GameObject.FindGameObjectWithTag("BBA");
        SM = GameObject.Find("SoundManager");
        smScript = SM.transform.GetComponent<SoundManagerScript>();
        smScript.PlayBGM(0);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void EnterShopBBA()
    {

    }
}
