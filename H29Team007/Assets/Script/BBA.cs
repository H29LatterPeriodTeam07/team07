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
    [SerializeField, Header("GameManagerのm_gdと同じ数字を入れてケロ")]
    private int m_int;
    public GameObject m_plane;
    public GameObject m_BBAplehab;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public BBACartCount m_scBBAcount;
    [System.NonSerialized]
    public NavMeshAgent m_Agent;

    public GameObject m_BBABasket;

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
    private Animator m_Animator;
    bool m_bo = true;
    Transform m_basuket;
    Transform m_Animal;
    float radius = 5f;
    private LayerMask raycastLayer;

    // Use this for initialization
    void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_gmScript = m_GameManager.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        bcScript = GetComponent<BBACartCount>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        m_EyePoint = transform.Find("BBAEye");
        raycastLayer = 1 << LayerMask.NameToLayer("Animal");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null) return;

    //    print(m_bo);
        //巡回中
        if (m_State == BBAState.NormalMode)
        {
            m_Agent.speed = 1.0f;
            m_Animator.SetFloat("Speed", m_Agent.speed);
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
            if (m_bo == false )
            {
                m_Agent.speed = 0.0f;
                // m_Animator.SetTrigger("Kago");
                //  m_bo = true;
            }
        }
        //特売品モード
        else if (m_State == BBAState.SaleMode)
        {
            m_Agent.speed = 4.0f;
            m_Animator.SetFloat("Speed", m_Agent.speed);
           // SetNewSalePatrolPointToDestination();

            m_ViewingDistance = 100;
            m_ViewingAngle = 180;

            if (m_Animal == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, raycastLayer);
                if (hitColliders.Length > 0)
                {
                    int randomInt = Random.Range(0, hitColliders.Length);
                    m_Animal = hitColliders[randomInt].transform;
                }
            }
            else if(m_Animal != null)
            {
                m_Agent.destination = m_Animal.transform.position;
            }

            else if (BBAHasArrived())
            {
                //  transform.LookAt(m_gmScript.m_SaleAnimalSpowns[0].transform.position);
                SetNewSalePatrolPointToDestination();
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
            m_Speed = 4.0f;

            if (!IsGetAnimal()) m_State = BBAState.NormalMode;
        }
    }

    //次の巡回ポイントを目的地に設定する
    public void SetNewPatrolPointToDestination()
    {
        for (int i = 0; i < m_gmScript.m_gd.Length; i++)
        {
            if (i == 0 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPointIndex
                    = (m_gmScript.m_gd[i].m_CurrentPatrolPointIndex + 1) % m_gmScript.m_gd[i].m_PatrolPoints.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_PatrolPoints[m_gmScript.m_gd[i].m_CurrentPatrolPointIndex].position;
            }
            else if (i == 1 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPointIndex
                    = (m_gmScript.m_gd[i].m_CurrentPatrolPointIndex + 1) % m_gmScript.m_gd[i].m_PatrolPoints.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_PatrolPoints[m_gmScript.m_gd[i].m_CurrentPatrolPointIndex].position;
            }
            else if (i == 2 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPointIndex
                    = (m_gmScript.m_gd[i].m_CurrentPatrolPointIndex + 1) % m_gmScript.m_gd[i].m_PatrolPoints.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_PatrolPoints[m_gmScript.m_gd[i].m_CurrentPatrolPointIndex].position;
            }
        }
    }

    public void SetNewSalePatrolPointToDestination()
    {
        for (int i = 0; i < m_gmScript.m_gd.Length; i++)
        {
            if (i == 0 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_gd[i].m_Patrolpoints2.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_Patrolpoints2[m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index].position;
            }
            else if (i == 1 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_gd[i].m_Patrolpoints2.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_Patrolpoints2[m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index].position;
            }
            else if (i == 2 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_gd[i].m_Patrolpoints2.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_Patrolpoints2[m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index].position;
            }
        }             
    }

    public void SetNewExitPointToDestination()
    {
        for (int i = 0; i < m_gmScript.m_gd.Length; i++)
        {
            if (i == 0 && m_int == i)
            {
                m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_ReziExitpoints.Length;

                m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
            }
            else if (i == 1 && m_int == i)
            {
                m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_ReziExitpoints.Length;

                m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
            }
            else if (i == 2 && m_int == i)
            {
                m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_ReziExitpoints.Length;

                m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
            }
        }
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
            m_bo = false;
        }

        if (other.tag == "ExitPoint")
        {
            if (IsGetAnimal())
            {
                gameObject.SetActive(false);
                bcScript.BaggegeFall(transform.position);
            }
            Invoke(("BBAReset"),3);
        }

    }

    void BBAReset()
    {
        m_Animal = null;
        gameObject.SetActive(true);
        m_State = BBAState.NormalMode;
        SetNewPatrolPointToDestination();
    }
}