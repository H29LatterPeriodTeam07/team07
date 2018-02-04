using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialBBA : MonoBehaviour {

    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;

    private CustomerState m_State = CustomerState.NormalMode;
    private Animator m_Animator;
    NavMeshAgent m_Agent;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    int m_rand;
    GameObject m_PatrolPoint;
    GameObject[] m_PatrolPoints;

    private MTPlayer tp;



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
        SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");

        tp = m_Player.GetComponent<MTPlayer>();

        m_EyePoint = transform.Find("LookEye");
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Agent.speed = 1.0f;
        m_Animator.SetFloat("Speed", m_Agent.speed);
        if (tp.GetState() != MTPlayer.PlayerState.Takeover) {
            m_Agent.destination = transform.position;
            return;
        }
        //巡回中
        if (m_State == CustomerState.NormalMode)
        {
            // m_Agent.speed = 1.0f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            if (HasArrived())
            {
                //     m_Agent.speed = 1.0f;
                SetNewPatrolPointToDestination();
            }
        }
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
