using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpown : MonoBehaviour {
    public GameObject[] m_Enemys;
    public int m_EnemyCount;
    public int m_MaxEnemy;

    int m_Num=0;
    int m_Random;
    float m_time;

	// Use this for initialization
	void Start () {
        m_Random = Random.Range(0, m_Enemys.Length);
        m_time += Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_time > 3)
        {
            Apper();
            m_Num++;
        }
	}

    void Apper()
    {
        if (m_Num < m_MaxEnemy)
        {
            Instantiate(m_Enemys[m_Random], transform.position, transform.rotation);
        }
    }
}
