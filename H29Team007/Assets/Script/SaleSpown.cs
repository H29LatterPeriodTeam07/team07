﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleSpown : MonoBehaviour {
    public AudioClip m_se;
    private AudioSource m_AS;

    //上から順番に出現させる特売品のプレハブ
    [SerializeField, Header("上から順に出現する特売品のプレハブを入れる(ApperTimeと要素数は同じにするように)")]
    private GameObject[] m_SaleAnimals;
    //出現時間
    [SerializeField, Header("指定した特売品が出現する時間の指定(float型)")]
    public float[] m_ApperTime;
    //巡回ポイント
    public Transform[] m_PatrolPoints;
    public float m_SaleModeTime;
    public GameObject m_Time;
    public GameObject m_SaleMaterial;
    public int m_Num = 0;

    Timer m_timer;
    int m_CurrentSaleAnimeIndex=0;
    public int m_CurrentApperTimeIndex=0;
    float m_SaleMode=0;
    SaleMaterial m_scSale;
    string m_Pigname, FishName, CowName;
    int m_CurrentPatrolPointIndex = 1;

    // Use this for initialization
    void Start()
    {
        m_timer = m_Time.GetComponent<Timer>();
        m_scSale = m_SaleMaterial.GetComponent<SaleMaterial>();
        m_AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ApperTime.Length > m_CurrentApperTimeIndex)
        {
            m_SaleMode = m_ApperTime[m_CurrentApperTimeIndex] - m_SaleModeTime;
            if (m_timer.timer > m_ApperTime[m_CurrentApperTimeIndex])
            {
                if (m_SaleAnimals[m_CurrentSaleAnimeIndex].gameObject.name == "SaleAnimalCow")
                {
                    m_scSale.ApperCow();
                }
                if (m_SaleAnimals[m_CurrentSaleAnimeIndex].gameObject.name == "SaleAnimalFish")
                {
                    m_scSale.ApperFish();
                }
                if (m_SaleAnimals[m_CurrentSaleAnimeIndex].gameObject.name == "SaleAnimalPig")
                {
                    m_scSale.ApperPig();
                }
            }
        }
    }

   public void Appear()
    {
        if (m_Num < m_SaleAnimals.Length)
        {
                Instantiate(m_SaleAnimals[m_CurrentSaleAnimeIndex], transform.position, transform.rotation);
                m_CurrentSaleAnimeIndex++;
            
        }
    }

    void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex
            = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

      //  m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;
    }

    public bool SaleMode()
    {
        return m_timer.timer > m_SaleMode;
    }
}
