using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleSpown : MonoBehaviour {

    //上から順番に出現させる特売品のプレハブ
    [SerializeField, TooltipAttribute("上から順に出現する特売品のプレハブを入れる(ApperTimeと要素数は同じにするように)")]
    private GameObject[] m_SaleAnimals;
    //出現時間
    [SerializeField, TooltipAttribute("指定した特売品が出現する時間の指定(float型)")]
    public float[] m_ApperTime;
    public float m_SaleModeTime;
    public GameObject m_Time;
    private int m_Num = 0;
    Timer m_timer;
    int m_CurrentSaleAnimeIndex=0;
    int m_CurrentApperTimeIndex=0;
    float m_SaleMode=0;

    // Use this for initialization
    void Start()
    {
        m_timer = m_Time.GetComponent<Timer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_ApperTime.Length > m_CurrentApperTimeIndex)
        {
            m_SaleMode = m_ApperTime[m_CurrentApperTimeIndex] - m_SaleModeTime;
            if (m_timer.timer > m_ApperTime[m_CurrentApperTimeIndex])
            { 
                m_CurrentApperTimeIndex++;
                Appear();
                m_Num++;
            }
        }
    }

    void Appear()
    {
        if (m_Num < m_SaleAnimals.Length)
        {
            Instantiate(m_SaleAnimals[m_CurrentSaleAnimeIndex], transform.position, transform.rotation);
            m_CurrentSaleAnimeIndex++;
        }
    }

    public bool SaleMode()
    {
        return m_timer.timer > m_SaleMode;
    }
}
