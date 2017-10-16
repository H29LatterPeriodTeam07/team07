using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleSpown : MonoBehaviour {

    //ランダムに出現させる特売品のプレハブ
    public GameObject[] m_SaleAnimals;
    //出現時間
    public float m_ApperTime = 30.0f;
    //ここから何匹出現するか
    [SerializeField, Header("何匹出現させる？")]
    public int m_Total = 1;
    //何匹出現したか
    private int m_Num = 0;
    private float m_CurrentTime = 0.0f;
    private int m_number;

    // Use this for initialization
    void Start()
    {
        m_number = Random.Range(0, m_SaleAnimals.Length);
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentTime += Time.deltaTime;

        if (m_CurrentTime > m_ApperTime)
        {
            Appear();
            m_Num++;

        }
    }

    void Appear()
    {
        if (m_Num < m_Total)
        {
            Instantiate(m_SaleAnimals[m_number], transform.position, transform.rotation);
        }
        m_CurrentTime = 0.0f;
    }
}
