using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySopwn : MonoBehaviour {

    public GameObject[] m_Enemys;
    public float m_ApperTime; //エネミー達を出す時間の間隔

    int m_EnemyCount; //ステージ内にいるＮＰＣの数
    int m_NUm;//ここから何体出たか
    int m_rand;//乱数
    float m_CurentTime = 0.0f;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        m_CurentTime += Time.deltaTime;

        if (m_CurentTime > m_ApperTime)
        {
            m_rand = Random.Range(0, m_Enemys.Length);
            Apper();
        }
	}

    void Apper() { 
            Instantiate(m_Enemys[m_rand],transform.position, transform.rotation);
            m_CurentTime = 0.0f;
        
    }
}
