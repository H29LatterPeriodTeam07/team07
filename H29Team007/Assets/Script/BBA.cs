using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BBAState
{
    //ノーマルモード
    NormalMode,
    //特売品モード
    SaleMode,
    //攻撃モード
    attackMode,
    //レジへ向かうモード
    CashMode
}

public class BBA : MonoBehaviour
{
    public GameObject m_BBAplehab;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public BBACartCount m_scBBAcount;
    [System.NonSerialized]
    public NavMeshAgent m_Agent;

    private BBAState m_State = BBAState.NormalMode;
    private float m_Speed = 1.0f;
    private Rigidbody rb;
    private GameObject m_GameManager;
    private GameManager m_gmScript;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    //特売品への参照
    GameObject m_SaleAnimals;
    //特売品への注視点
    Transform m_SaleAnimalsLookPoint;
    int m_SaleSpownIndex = 0;
    BBACartCount bcScript;

    // Use this for initialization
    void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_gmScript = m_GameManager.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        bcScript = GetComponent<BBACartCount>();
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        m_EyePoint = transform.Find("BBAEye");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null) return;
        //巡回中
        if (m_State == BBAState.NormalMode)
        {
            m_Agent.speed = 1.0f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            //特売品が出てくる時間になったら特売品モードに
            if (m_gmScript.m_scSaleSpown.SaleMode())
            {
                //特売品モードに状態変更
                m_State = BBAState.SaleMode;
                //   m_Agent.destination = m_Player.transform.position;
            }
            else
            {
                if (BBAHasArrived())
                {
                    SetNewPatrolPointToDestination();
                }
            }
        }
        //特売品モード
        else if (m_State == BBAState.SaleMode)
        {
            m_Agent.speed = 3.0f;
            SetNewSalePatrolPointToDestination();

            m_ViewingDistance = 100;
            m_ViewingAngle = 180;
            Ray ray = new Ray(m_EyePoint.position, m_EyePoint.forward);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo);
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            if (hit && hitInfo.collider.tag == "Animal")
            {
                m_SaleAnimals = GameObject.FindGameObjectWithTag("Animal");
                m_Agent.destination = m_SaleAnimals.transform.position;
            }

            else if (BBAHasArrived())
            {
                transform.LookAt(m_gmScript.m_SaleAnimalSpowns[0].transform.position);
            }

            if (IsGetAnimal())
            {
                m_State = BBAState.CashMode;
            }
        }
        //レジ～出入り口へGOモード
        else if (m_State == BBAState.CashMode)
        {
            SetNewExitPointToDestination();
            m_Speed = 3;

            if (!IsGetAnimal()) m_State = BBAState.NormalMode;
        }
    }

    //次の巡回ポイントを目的地に設定する
    public void SetNewPatrolPointToDestination()
    {
        m_gmScript.m_CurrentPatrolPointIndex
            = (m_gmScript.m_CurrentPatrolPointIndex + 1) % m_gmScript.m_PatrolPoints.Length;

       m_Agent.destination = m_gmScript.m_PatrolPoints[m_gmScript.m_CurrentPatrolPointIndex].position;
    }

    public void SetNewSalePatrolPointToDestination()
    {
        m_gmScript.m_CurrentPatrolPoint2Index
            = (m_gmScript.m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_Patrolpoints2.Length;

       m_Agent.destination = m_gmScript.m_Patrolpoints2[m_gmScript.m_CurrentPatrolPoint2Index].position;
    }

    public void SetNewExitPointToDestination()
    {
        m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_ReziExitpoints.Length;

        m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
    }

    // 目的地に到着したか
    public bool BBAHasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

    public bool IsGetAnimal()
    {
        return m_scBBAcount.IsBaggegeinHuman();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")
        {
                bcScript.BaggegeFall(transform.position);        
        }
    }
}