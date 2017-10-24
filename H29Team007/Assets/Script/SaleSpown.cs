using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleSpown : MonoBehaviour {

    //ランダムに出現させる特売品のプレハブ
    public GameObject[] m_SaleAnimals;
    //出現時間
    public float[] m_ApperTime;
    //ここから何匹出現するか
    [SerializeField, Header("何匹出現させる？")]
    public int m_Total;
    public GameObject m_Time;
    public Queue<GameObject> m_Sale;
    //何匹出現したか
    private int m_Num = 0;
    private float m_CurrentTime = 0.0f;
    private int m_number;
    Timer m_timer;

    // Use this for initialization
    void Start()
    {
        m_number = Random.Range(0, m_SaleAnimals.Length);
        m_timer = m_Time.GetComponent<Timer>();
        for(int i =0; m_SaleAnimals.Length > i; i++)
        {
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; m_ApperTime.Length > i; i++)
        {
            if (m_timer.timer > m_ApperTime[i])
            {
                Appear();
                m_Num++;

            }
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
