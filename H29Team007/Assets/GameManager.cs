using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject[] SaleSpawnPosition;
    public int[] SaleSpawnTime;
    //ランダムに出現させる特売品のプレハブ
    public GameObject[] m_SaleAnimals;

    //タイマー
    private float m_timer = 0.0f;
    //何匹出現したか
    private int m_Num = 0;
    private float m_CurrentTime = 0.0f;
    private int m_number;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_timer += Time.deltaTime;
	}
}
