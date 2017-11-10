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
    //巡回ポイント
    public Transform[] m_PatrolPoints;
    //巡回ポイント2
    public Transform[] m_Patrolpoints2;
    //レジから出口のポイント
    public Transform[] m_ReziExitpoints;
    //出口を指定
    public Transform m_ExitPoint;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public BBACartCount m_scBBAcount;

    [SerializeField, Header("特売品の出現場所を入れるところ")]
    private GameObject[] m_SaleAnimalSpowns;
    private BBAState m_State = BBAState.NormalMode;
    private float m_Speed = 1.0f;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    int m_CurrentPatrolPoint2Index = 1;
    int m_CurrentPatrolPoint3Index = 1;

    NavMeshAgent m_Agent;
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
    SaleSpown m_scSaleSpown;
    int m_SaleSpownIndex = 0;
    BBACartCount bcScript;

    // Use this for initialization
    void Start()
    {
        bcScript = GetComponent<BBACartCount>();
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        /* //タグでプレイヤーオブジェクトを検索して保持
         m_Player = GameObject.FindGameObjectWithTag("Player");
         //プレイヤーの注視点を名前で検索して保持
         m_PlayerLookpoint = m_Player.transform.Find("LookPoint");*/
        m_EyePoint = transform.Find("BBAEye");
        //スクリプトSaleSpownへの参照
        for (int i = 0; i < m_SaleAnimalSpowns.Length; i++)
        {
            m_scSaleSpown = m_SaleAnimalSpowns[i].GetComponent<SaleSpown>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null) return;
        print(m_State);
        //巡回中
        if (m_State == BBAState.NormalMode)
        {
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            //特売品が出てくる時間になったら特売品モードに
            if (m_scSaleSpown.SaleMode())
            {
                //特売品モードに状態変更
                m_State = BBAState.SaleMode;
                //   m_Agent.destination = m_Player.transform.position;
            }
            else
            {
                if (HasArrived())
                {
                    SetNewPatrolPointToDestination();
                }
            }
        }
        //特売品モード
        else if (m_State == BBAState.SaleMode)
        {
            SetNewSalePatrolPointToDestination();

            m_ViewingDistance = 100;
            m_ViewingAngle = 180;
            Ray ray = new Ray(m_EyePoint.position, m_EyePoint.forward);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo);
            if (hit && hitInfo.collider.tag == "Animal")
            {
                m_SaleAnimals = GameObject.FindGameObjectWithTag("Animal");
                m_Agent.destination = m_SaleAnimals.transform.position;
            }

            else if (HasArrived())
            {
                transform.LookAt(m_SaleAnimalSpowns[0].transform.position);
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
        }
    }

    //次の巡回ポイントを目的地に設定する
    void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex
            = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

        m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;
    }

    void SetNewSalePatrolPointToDestination()
    {
        m_CurrentPatrolPoint2Index
            = (m_CurrentPatrolPoint2Index + 1) % m_Patrolpoints2.Length;

        m_Agent.destination = m_Patrolpoints2[m_CurrentPatrolPoint2Index].position;
    }

    void SetNewExitPointToDestination()
    {
        m_CurrentPatrolPoint3Index
            = (m_CurrentPatrolPoint2Index + 1) % m_ReziExitpoints.Length;

        m_Agent.destination = m_ReziExitpoints[m_CurrentPatrolPoint3Index].position;
    }

    // 目的地に到着したか
    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

    public bool IsGetAnimal()
    {
        return m_scBBAcount.IsBaggegeinHuman();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExitPoint")
        {
            Destroy(gameObject);
        }
        if (other.name == "FrontHitArea")
        {
                bcScript.BaggegeFall(transform.position);
            
        }
    }
}