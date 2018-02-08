using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MTCustomer : MonoBehaviour {

    private Animator m_Animator;
    NavMeshAgent m_Agent;
    int m_rand;
    GameObject m_PatrolPoint;
    GameObject[] m_PatrolPoints;


    // Use this for initialization
    void Start()
    {
        //タグでパトロールポイントの親を検索して保持
        m_PatrolPoint = GameObject.FindGameObjectWithTag("PatrolPoint");
        m_PatrolPoints = new GameObject[m_PatrolPoint.transform.childCount];
        //パトロールポイントの子を取得
        for (int i = 0; m_PatrolPoint.transform.childCount > i; i++)
        {
            m_PatrolPoints[i] = m_PatrolPoint.transform.GetChild(i).gameObject;
        }
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        if (m_Agent.enabled) SetNewPatrolPointToDestination();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        m_Agent.speed = 1.0f;
        if (HasArrived())
        {
            SetNewPatrolPointToDestination();
        }
        m_Animator.SetFloat("Speed", m_Agent.speed);
    }

    //次の巡回ポイントを目的地に設定する
    void SetNewPatrolPointToDestination()
    {
        m_rand = Random.Range(0, m_PatrolPoints.Length);
        m_Agent.destination = m_PatrolPoints[m_rand].transform.position;
    }

    // 目的地に到着したか
    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }
}
